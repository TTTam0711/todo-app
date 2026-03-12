using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Authentication.Jwt
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string SigningKey { get; init; } = null!;

        // minutes
        public int AccessTokenLifetime { get; init; }
        public int RefreshTokenLifetime { get; init; }
    }
}
