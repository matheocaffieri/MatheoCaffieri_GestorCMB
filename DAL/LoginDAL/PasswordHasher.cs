using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.LoginDAL
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string plainText)
        {
            // Implementación real, p.ej. BCrypt
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }
        public bool Verify(string hash, string plainText)
        {
            var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(hash));
            return decoded == plainText;
        }
    }
}
