using DomainModel.Login;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.LoginDALInterfaces;

namespace Services.RoleService.Logic
{
    public class UsuarioPermisosService
    {
        private readonly IUsuarioAccesoRepository _uaRepo;
        private readonly IAccesoRepository _accRepo;

        public UsuarioPermisosService(IUsuarioAccesoRepository uaRepo, IAccesoRepository accRepo)
        {
            _uaRepo = uaRepo ?? throw new ArgumentNullException(nameof(uaRepo));
            _accRepo = accRepo ?? throw new ArgumentNullException(nameof(accRepo));
        }

        public List<Acceso> ObtenerDirectos(Guid idUsuario)
            => _uaRepo.GetDirectos(idUsuario);

        public void ReemplazarDirectos(Guid idUsuario, IEnumerable<TipoPermiso> permisos)
        {
            var target = new HashSet<TipoPermiso>(permisos ?? Enumerable.Empty<TipoPermiso>());

            var ids = _accRepo.GetAll()
                .Where(a => target.Contains(a.DataKey))
                .Select(a => a.Id)
                .ToList();

            _uaRepo.ReplaceDirectos(idUsuario, ids);
        }
    }
}
