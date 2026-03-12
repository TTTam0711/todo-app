using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Persistence.Models
{
    public partial class RefreshToken
    {
        public bool IsExpired =>
            ExpiresAt <= DateTime.UtcNow;

        public bool IsRevoked =>
            RevokedAt != null;

        public bool IsActive =>
            !IsExpired && !IsRevoked;

        public bool IsReuseDetected =>
            IsRevoked && ReplacedByTokenHash != null;
    }
}
