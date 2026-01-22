using DAL.LoginDAL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.FactoryDAL;
using DomainModel.LoginDALInterfaces;
using DomainModel.Interfaces;    

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


        public LoginService(string connectionStringUsers)
        : this(new UsuarioRepository(new SqlUnitOfWork(connectionStringUsers)), new PasswordHasher())
        {
            if (string.IsNullOrEmpty(connectionStringUsers))
            {
                throw new ArgumentNullException(nameof(connectionStringUsers), "La cadena de conexión no puede ser nula o vacía.");
            }
            // La llamada a ': this(...)' ejecuta el constructor principal
            // pasando las instancias recién creadas
            // recibe la cadena de conexión
        }

        public LoginResult TryLogin(string mail, string password, out Usuario usuario)
        {
            usuario = null;

            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null)
                return LoginResult.CredencialesInvalidas;

            // Bloquear si no está activo (antes o después de password; así es explícito)
            if (!user.IsActive)
                return LoginResult.UsuarioInactivo;

            // Validar contraseña
            if (!_hasher.Verify(user.Contraseña, password))
                return LoginResult.CredencialesInvalidas;

            usuario = user;
            return LoginResult.Ok;
        }




        public Usuario Login(string mail, string password)
        {
            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null)
                return null;

            if (!user.IsActive)
                return null;

            if (!_hasher.Verify(user.Contraseña, password))
                return null;

            return user;
        }

    }
}
