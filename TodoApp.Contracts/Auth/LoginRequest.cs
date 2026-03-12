using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Contracts.Auth
{
    public sealed class LoginRequest
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
