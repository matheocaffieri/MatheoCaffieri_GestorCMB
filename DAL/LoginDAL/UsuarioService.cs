using Services.LoginComposite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginDAL
{
    public class UsuarioService
    {
        private UsuarioRepository _usuarioRepo;

        public UsuarioService()
        {
            _usuarioRepo = new UsuarioRepository();
        }

       /* public Usuario Login(string mail, string contraseña)
        {
            // 1️⃣ Validar entrada
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(contraseña))
                throw new ArgumentException("El email y la contraseña son obligatorios.");

            // 2️⃣ Buscar usuario en la BD
            Usuario usuario = _usuarioRepo.GetUsuarioPorMail(mail);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!usuario.IsActive)
                throw new Exception("El usuario está deshabilitado.");

            // 3️⃣ Comparar contraseña (hasheada en la BD)
            if (usuario.Contraseña != HashPassword(contraseña))
                throw new Exception("Contraseña incorrecta.");

            return usuario; // 4️⃣ Autenticación exitosa, devolver usuario autenticado
        }*/

        // Función para hashear contraseñas (SHA-256)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
