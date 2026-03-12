using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Persistence.Models;

namespace TodoApp.Infrastructure.Authentication.Jwt
{
    public sealed record GeneratedRefreshToken(
        RefreshToken Token,
        string RawToken
    );
}
