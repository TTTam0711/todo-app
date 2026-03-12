using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Authentication.Security.Passwords
{
    internal sealed class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;   // 128-bit
        private const int KeySize = 32;   // 256-bit
        private const int Iterations = 100_000;

        public byte[] Hash(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            var key = pbkdf2.GetBytes(KeySize);

            // layout: [salt][key]
            var result = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(key, 0, result, SaltSize, KeySize);

            return result;
        }

        public bool Verify(string password, byte[] passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (passwordHash is null || passwordHash.Length != SaltSize + KeySize)
                return false;

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(passwordHash, 0, salt, 0, SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            var computedKey = pbkdf2.GetBytes(KeySize);

            // constant-time comparison
            for (int i = 0; i < KeySize; i++)
            {
                if (passwordHash[SaltSize + i] != computedKey[i])
                    return false;
            }

            return true;
        }
    }
}
