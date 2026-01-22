using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginService
{
    public enum LoginResult
    {
        Ok = 0,
        CredencialesInvalidas = 1,
        UsuarioInactivo = 2
    }
}
