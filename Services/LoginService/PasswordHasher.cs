using Services.Services_Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Services.LoginService
{
    public class PasswordHasher : IPasswordHasher
    {
        // work factor 12: ~250ms por hash, buen balance seguridad/UX
        private const int WorkFactor = 12;

        public string Hash(string plainText)
            => BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);

        public bool Verify(string hash, string plainText)
        {
            if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(plainText))
                return false;

            // Hashes BCrypt empiezan con $2a$ / $2b$ / $2y$
            if (hash.StartsWith("$2"))
                return BCrypt.Net.BCrypt.Verify(plainText, hash);

            // Compatibilidad: hashes SHA256 legacy (Base64 sin $2)
            return LegacySha256(plainText) == hash;
        }

        private static string LegacySha256(string plainText)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }
    }
}
