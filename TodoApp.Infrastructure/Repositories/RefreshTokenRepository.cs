using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Authentication.Jwt;
using TodoApp.Infrastructure.Persistence.Context;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TodoAppDbContext _db;

        public RefreshTokenRepository(TodoAppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(
            RefreshToken token,
            CancellationToken ct = default)
        {
            await _db.RefreshTokens.AddAsync(token, ct);
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken ct = default)
        {
            return await _db.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    ct);
        }

        public Task RevokeAsync(
            RefreshToken token,
            string? replacedByTokenHash,
            CancellationToken ct = default)
        {
            token.RevokedAt = DateTime.UtcNow;
            token.ReplacedByTokenHash = replacedByTokenHash;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _db.SaveChangesAsync(ct);
        }

        public async Task RevokeAllForUserAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            var tokens = await _db.RefreshTokens
                .Where(x => x.UserId == userId && x.RevokedAt == null)
                .ToListAsync(ct);

            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
        }
    }
}
