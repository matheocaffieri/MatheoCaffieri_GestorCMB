using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
    public interface IPermisoService
    {
        void AgregarPermisoAUsuario(Usuario u, IPermiso p);
        bool UsuarioTienePermiso(Usuario u, TipoPermiso permiso);
    }
}
