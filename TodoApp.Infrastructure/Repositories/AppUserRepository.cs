using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Persistence.Context;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Repositories
{
    public sealed class AppUserRepository
    {
        private readonly TodoAppDbContext _db;

        public AppUserRepository(TodoAppDbContext db)
        {
            _db = db;
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return _db.AppUsers.AnyAsync(u => u.Email == email, ct);
        }

        public async Task AddAsync(AppUser user, CancellationToken ct = default)
        {
            _db.AppUsers.Add(user);
            await _db.SaveChangesAsync(ct);
        }
        public Task<AppUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return _db.AppUsers
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }
        public async Task<AppUser?> GetByIdAsync(Guid userId, CancellationToken ct)
        {
            return await _db.AppUsers
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }
    }

}
