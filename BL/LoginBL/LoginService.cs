using DomainModel.Login;
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

        public Usuario Login(string mail, string plainPassword)
        {
            var usuario = _usuarioRepo.FindByEmail(mail);
            if (usuario == null)
                return null;

            if (!_hasher.Verify(usuario.Contraseña, plainPassword))
                return null;

            return usuario;
        }
    }
}
