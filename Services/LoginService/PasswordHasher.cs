using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginService
{
    public class PasswordHasher 
    {
        public string Hash(string plainText)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool Verify(string hash, string plainText)
        {
            return Hash(plainText) == hash;
        }
    }
}
