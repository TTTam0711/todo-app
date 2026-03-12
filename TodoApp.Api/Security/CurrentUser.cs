using System.IdentityModel.Tokens.Jwt;
using TodoApp.Application.Common.Security;

namespace ToDoApp.Api.Security
{
    internal sealed class CurrentUser : ICurrentUser
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated => UserId.HasValue;

        public CurrentUser(IHttpContextAccessor accessor)
        {
            var sub = accessor.HttpContext?
                .User?
                .FindFirst(JwtRegisteredClaimNames.Sub)?
                .Value;

            if (Guid.TryParse(sub, out var userId))
                UserId = userId;
        }
    }
}
