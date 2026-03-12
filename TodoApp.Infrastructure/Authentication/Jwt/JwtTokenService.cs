using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Authentication.Jwt
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateAccessToken(AppUser user)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SigningKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public GeneratedRefreshToken GenerateRefreshToken(
            Guid userId,
            string? deviceInfo)
        {
            var rawToken = GenerateSecureToken();

            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = userId,
                TokenHash = HashToken(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetime),
                DeviceInfo = deviceInfo
            };

            return new GeneratedRefreshToken(refreshToken, rawToken);
        }

        // ===== helpers =====

        private static string GenerateSecureToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
