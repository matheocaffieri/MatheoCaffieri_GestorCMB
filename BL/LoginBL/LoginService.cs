using DAL.LoginDAL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
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

        public Usuario Login(string mail, string password)
        {
            var user = _usuarioRepo.FindByEmail(mail);
            if (user == null || !_hasher.Verify(user.Contraseña, password))
                return null;

            return user;
        }
    }
}
