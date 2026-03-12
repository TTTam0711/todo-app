using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.Auth
{
    public sealed class AuthResponse
    {
        public string AccessToken { get; init; } = default!;
        public int ExpiresIn { get; init; }

        public AuthUserResponse User { get; init; } = default!;
    }

    public sealed class AuthUserResponse
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string DisplayName { get; init; } = default!;
    }
    
}
