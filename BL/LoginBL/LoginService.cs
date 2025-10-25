using DAL.LoginDAL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
    public class LoginService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;

        public LoginService(IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _usuarioRepo = usuarioRepo;
            _hasher = hasher;
        }


        // Constructor para ser llamado fácilmente desde UI (pasando solo la conexión)
        public LoginService(string connectionStringUsers)
            : this(new UsuarioRepository(connectionStringUsers), new PasswordHasher()) // <--- CORRECCIÓN AQUÍ
        {
            if (string.IsNullOrEmpty(connectionStringUsers))
            {
                throw new ArgumentNullException(nameof(connectionStringUsers), "La cadena de conexión no puede ser nula o vacía.");
            }
            // La llamada a ': this(...)' ejecuta el constructor principal
            // pasando las instancias recién creadas, AHORA UsuarioRepository
            // recibe la cadena de conexión como debe ser.
        }
   

        public Usuario Login(string mail, string password)
        {
            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null || !_hasher.Verify(user.Contraseña, password))
                return null;

            return user;
        }
    }
}
