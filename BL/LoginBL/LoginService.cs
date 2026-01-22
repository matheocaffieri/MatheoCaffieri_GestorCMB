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

        // ✅ DI
        public LoginService(IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        // ✅ Legacy: recibe "nombre de CS" o "CS directa" (LOGIN), usa SqlLoginUnitOfWork
        public LoginService(string connectionStringUsers)
            : this(
                new UsuarioRepository(new SqlLoginUnitOfWork(connectionStringUsers)),
                new PasswordHasher())
        {
            if (string.IsNullOrWhiteSpace(connectionStringUsers))
                throw new ArgumentNullException(nameof(connectionStringUsers),
                    "La cadena (o nombre) de conexión no puede ser nula o vacía.");
        }

        public LoginResult TryLogin(string mail, string password, out Usuario usuario)
        {
            usuario = null;

            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                return LoginResult.CredencialesInvalidas;

            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null)
                return LoginResult.CredencialesInvalidas;

            if (!user.IsActive)
                return LoginResult.UsuarioInactivo;

            if (!_hasher.Verify(user.Contraseña, password))
                return LoginResult.CredencialesInvalidas;

            usuario = user;
            return LoginResult.Ok;
        }

        public Usuario Login(string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null) return null;
            if (!user.IsActive) return null;
            if (!_hasher.Verify(user.Contraseña, password)) return null;

            return user;
        }
    }
}
