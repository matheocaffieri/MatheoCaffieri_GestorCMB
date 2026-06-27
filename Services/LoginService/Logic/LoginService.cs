using Services.LoginService.DataAccess;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Services_Interfaces;

namespace Services.LoginService.Logic
{
    public class LoginService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;

        public LoginService(IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        // Recibe "nombre de CS" definido en App.config o una cadena de conexión literal (LOGIN).
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
            {
                LoggerLogic.Warn($"[Login] Intento de login con campos vacíos. Mail='{mail}'");
                return LoginResult.CredencialesInvalidas;
            }

            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null)
            {
                LoggerLogic.Warn($"[Login] Credenciales inválidas: mail no encontrado ('{mail}').");
                return LoginResult.CredencialesInvalidas;
            }

            if (!user.IsActive)
            {
                LoggerLogic.Warn($"[Login] Intento de login con usuario inactivo. Mail='{mail}' Id={user.IdUsuario}");
                return LoginResult.UsuarioInactivo;
            }

            if (!_hasher.Verify(user.Contraseña, password))
            {
                LoggerLogic.Warn($"[Login] Credenciales inválidas: contraseña incorrecta. Mail='{mail}'");
                return LoginResult.CredencialesInvalidas;
            }

            usuario = user;
            LoggerLogic.Info($"[Login] Sesión iniciada. Mail='{mail}' Id={user.IdUsuario}");
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
