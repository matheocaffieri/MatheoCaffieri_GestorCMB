using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginService
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainText)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool Verify(string hash, string plainText)
        {
            if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(plainText))
                return false;

            return Hash(plainText) == hash;
        }
    }
}
