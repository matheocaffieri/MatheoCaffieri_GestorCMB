using DomainModel.Login;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
    public class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _lock = new object();

        private Usuario _usuarioActual;

        private SessionManager() { }

        public static SessionManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new SessionManager();
                    return _instance;
                }
            }
        }

        public void Login(Usuario usuario)
        {
            _usuarioActual = usuario;
        }

        public void Logout()
        {
            _usuarioActual = null;
        }

        public bool IsLoggedIn()
        {
            return _usuarioActual != null;
        }

        public Usuario UsuarioActual => _usuarioActual;

        public bool TienePermiso(TipoPermiso permiso)
        {
            if (_usuarioActual == null)
                return false;

            return _usuarioActual.TienePermiso(permiso);
        }
    }
}
