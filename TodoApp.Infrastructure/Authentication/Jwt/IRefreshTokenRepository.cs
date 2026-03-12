using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Authentication.Jwt
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token, CancellationToken ct = default);

        Task<RefreshToken?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken ct = default);

        Task RevokeAsync(
            RefreshToken token,
            string? replacedByTokenHash,
            CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
        Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);
    }
}
