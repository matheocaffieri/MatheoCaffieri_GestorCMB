using DAL.AccessDAL;
using DomainModel.Login;
using Interfaces;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AccessBL
{
    public class UsuarioPermisosService
    {
        private readonly IUsuarioAccesoRepository _uaRepo;
        private readonly IAccesoRepository _accRepo;

        internal UsuarioPermisosService(IUsuarioAccesoRepository uaRepo, IAccesoRepository accRepo)
        { _uaRepo = uaRepo; _accRepo = accRepo; }

        public UsuarioPermisosService(string cs)
        {
            _uaRepo = new DAL.AccessDAL.UsuarioAccesoRepository(cs);
            _accRepo = new DAL.AccessDAL.AccesoRepository(cs);
        }

        public List<Acceso> ObtenerDirectos(Guid idUsuario) => _uaRepo.GetDirectos(idUsuario);

        public void ReemplazarDirectos(Guid idUsuario, IEnumerable<TipoPermiso> permisos)
        {
            var catalogo = _accRepo.GetAll();
            var ids = catalogo.Where(a => permisos.Contains(a.DataKey))
                              .Select(a => (Guid)typeof(Acceso).GetProperty("Id").GetValue(a))
                              .ToList();
            _uaRepo.ReplaceDirectos(idUsuario, ids);
        }
    }
}
