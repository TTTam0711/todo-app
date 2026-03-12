using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Contracts.Auth;
using TodoApp.Infrastructure.Authentication.Jwt;
using TodoApp.Infrastructure.Authentication.Security.Passwords;
using TodoApp.Infrastructure.Persistence.Models;
using TodoApp.Infrastructure.Repositories;

namespace ToDoApp.Api.Controllers
{
    
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly AppUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public AuthController(
            AppUserRepository userRepo,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IRefreshTokenRepository refreshTokenRepo)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _refreshTokenRepo = refreshTokenRepo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(
             [FromBody] RegisterRequest request,
             CancellationToken ct)
        {
            var email = request.Email?.Trim().ToLowerInvariant();
            var password = request.Password;
            var displayName = request.DisplayName?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required");

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("Password is required");

            if (password.Length < 6)
                return BadRequest("Password must be at least 6 characters");

            if (await _userRepo.ExistsByEmailAsync(email, ct))
                return Conflict("Email already exists");

            var passwordHash = _passwordHasher.Hash(password);

            var user = new AppUser
            {
                UserId = Guid.NewGuid(),
                Email = email,
                DisplayName = displayName,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user, ct);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var email = request.Email?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required");
            }

            var user = await _userRepo.GetByEmailAsync(email, ct);

            if (user is null || !user.IsActive)
            {
                return Unauthorized("Invalid credentials");
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash!))
            {
                return Unauthorized("Invalid credentials");
            }

            // ===== ISSUE TOKENS =====

            // 1️⃣ Access token
            var accessToken = _jwtTokenService.GenerateAccessToken(user);

            // 2️⃣ Refresh token
            var generated = _jwtTokenService.GenerateRefreshToken(
                user.UserId,
                HttpContext.Request.Headers.UserAgent.ToString());

                    // 🔹 DB chỉ lưu entity
                    await _refreshTokenRepo.AddAsync(generated.Token, ct);
            await _refreshTokenRepo.SaveChangesAsync(ct);

            // 🔹 Cookie dùng RAW token
            SetRefreshTokenCookie(
                generated.RawToken,
                generated.Token.ExpiresAt);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = 60 * 30,
                User = new AuthUserResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    DisplayName = user.DisplayName
                }
            });

        }
        private void SetRefreshTokenCookie(string rawToken, DateTime expiresAt)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // 🔥 đổi từ Strict -> None
                Expires = expiresAt,
                Path = "/" // 🔥 đổi từ /api/auth -> /
            };

            Response.Cookies.Append("refresh_token", rawToken, options);
        }
        private void ClearRefreshTokenCookie()
        {
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            // 1️⃣ Lấy refresh token từ cookie
            if (!Request.Cookies.TryGetValue("refresh_token", out var rawToken))
            {
                ClearRefreshTokenCookie();
                return Unauthorized("Missing refresh token");
            }

            // 2️⃣ Hash raw token
            var tokenHash = JwtTokenService.HashToken(rawToken);

            // 3️⃣ Tìm refresh token trong DB
            var existingToken = await _refreshTokenRepo.GetByTokenHashAsync(tokenHash, ct);

            if (existingToken is null)
            {
                ClearRefreshTokenCookie();
                return Unauthorized("Invalid refresh token");
            }

            // 4️⃣ Detect refresh token reuse (CỰC KỲ QUAN TRỌNG)
            if (existingToken.IsReuseDetected)
            {
                await _refreshTokenRepo.RevokeAllForUserAsync(existingToken.UserId, ct);

                ClearRefreshTokenCookie();
                return Unauthorized("Refresh token reuse detected");
            }

            // 5️⃣ Token không còn active (expired / revoked)
            if (!existingToken.IsActive)
            {
                ClearRefreshTokenCookie();
                return Unauthorized("Refresh token expired or revoked");
            }

            var user = existingToken.User;

            // 6️⃣ Check user state
            if (!user.IsActive)
            {
                ClearRefreshTokenCookie();
                return Unauthorized("User inactive");
            }

            // 7️⃣ Generate new refresh token (ROTATION)
            var generated = _jwtTokenService.GenerateRefreshToken(
                user.UserId,
                HttpContext.Request.Headers.UserAgent.ToString());

            // 8️⃣ Revoke old token & link to new one
            await _refreshTokenRepo.RevokeAsync(
                existingToken,
                generated.Token.TokenHash,
                ct);

            // 9️⃣ Persist new refresh token
            await _refreshTokenRepo.AddAsync(generated.Token, ct);
            await _refreshTokenRepo.SaveChangesAsync(ct);

            // 🔟 Set new refresh token cookie (RAW token)
            SetRefreshTokenCookie(generated.RawToken, generated.Token.ExpiresAt);

            // 1️⃣1️⃣ Issue new access token
            var accessToken = _jwtTokenService.GenerateAccessToken(user);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = 60 * 30,
                User = new AuthUserResponse
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    DisplayName = user.DisplayName
                }
            });
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var rawToken))
            {
                var hash = JwtTokenService.HashToken(rawToken);

                var token = await _refreshTokenRepo.GetByTokenHashAsync(hash, ct);

                if (token != null && token.IsActive)
                {
                    await _refreshTokenRepo.RevokeAsync(token, null, ct);
                    await _refreshTokenRepo.SaveChangesAsync(ct);
                }
            }

            // 🔥 Luôn luôn xóa cookie (dù có token hay không)
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Path = "/",
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true
            });

            return Ok(new { message = "Logged out" });
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst("sub")?.Value
                              ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var user = await _userRepo.GetByIdAsync(userId, ct);
            if (user is null || !user.IsActive)
                return Unauthorized();

            return Ok(new AuthUserResponse
            {
                UserId = user.UserId,
                Email = user.Email,
                DisplayName = user.DisplayName
            });
        }

    }
}
