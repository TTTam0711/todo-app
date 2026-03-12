using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Authentication.Security.Passwords
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password);
        bool Verify(string password, byte[] passwordHash);
    }
}
