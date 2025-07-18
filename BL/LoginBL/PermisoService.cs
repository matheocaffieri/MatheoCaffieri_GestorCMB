using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
    public class PermisoService : IPermisoService
    {
        private readonly IPermisoRepository _permRepo;
        public PermisoService(IPermisoRepository permRepo)
        {
            _permRepo = permRepo;
        }
        public void AgregarPermisoAUsuario(Usuario u, IPermiso p)
        {
            u.AgregarPermiso(p);
            // opcionalmente actualizar en base
        }
        public bool UsuarioTienePermiso(Usuario u, TipoPermiso permiso)
        {
            return u.TienePermiso(permiso);
        }
    }
}
