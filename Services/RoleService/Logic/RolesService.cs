using DomainModel.LoginDALInterfaces; // IUsuarioRepository
using DomainModel.Login;
using Interfaces;                 // IFamiliaRepository
using Interfaces.LoginInterfaces; // IAccesoRepository
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.RoleService.Logic
{
    public class RolesService
    {
        private readonly IFamiliaRepository _famRepo;
        private readonly IAccesoRepository _accRepo;

        public RolesService(IFamiliaRepository famRepo, IAccesoRepository accRepo)
        {
            _famRepo = famRepo ?? throw new ArgumentNullException(nameof(famRepo));
            _accRepo = accRepo ?? throw new ArgumentNullException(nameof(accRepo));
        }

        // ----- Roles + sus accesos (Composite) -----

        public List<RolCompuesto> ListarRoles()
        {
            var roles = new List<RolCompuesto>();

            foreach (var tupla in _famRepo.GetAll()) // (Id, Nombre)
            {
                var rol = new RolCompuesto(tupla.Id, tupla.Nombre);

                var accesos = _famRepo.GetAccesos(tupla.Id); // List<Acceso>
                foreach (var acc in accesos)
                    rol.AgregarHijo(acc);

                roles.Add(rol);
            }

            return roles;
        }

        // ----- CRUD de rol / relaciones -----

        public Guid CrearRol(string nombre) => _famRepo.Create(nombre);

        public void AsignarPermisoARol(Guid idRol, Guid idAcceso) => _famRepo.AddAcceso(idRol, idAcceso);
        public void QuitarPermisoDeRol(Guid idRol, Guid idAcceso) => _famRepo.RemoveAcceso(idRol, idAcceso);

        public List<Usuario> UsuariosDelRol(Guid idRol) => _famRepo.GetUsuarios(idRol);

        public void AsignarUsuarioARol(Guid idRol, Guid idUsuario) => _famRepo.AddUsuario(idRol, idUsuario);
        public void QuitarUsuarioDeRol(Guid idRol, Guid idUsuario) => _famRepo.RemoveUsuario(idRol, idUsuario);

        // ----- Accesos sueltos (si querés mantenerlo acá) -----

        public List<Acceso> ListarAccesos() => _accRepo.GetAll();
        public Acceso CrearAcceso(string nombre, TipoPermiso key) => _accRepo.Create(nombre, key);

        // ----- Permisos (TipoPermiso) de un rol -----

        public IEnumerable<TipoPermiso> ObtenerPermisosDeRol(Guid rolId)
            => _famRepo.GetAccesos(rolId).Select(a => a.DataKey).ToList();

        public void ReemplazarPermisosDeRol(Guid rolId, IEnumerable<TipoPermiso> nuevos)
        {
            var target = new HashSet<TipoPermiso>(nuevos ?? Enumerable.Empty<TipoPermiso>());

            var actuales = _famRepo.GetAccesos(rolId);
            var actualesSet = new HashSet<TipoPermiso>(actuales.Select(a => a.DataKey));

            // Indexar catálogo por DataKey
            var catalogo = _accRepo.GetAll();
            var porKey = catalogo
                .GroupBy(a => a.DataKey)
                .ToDictionary(g => g.Key, g => g.First());

            // Quitar los que ya no van
            foreach (var acc in actuales)
            {
                if (!target.Contains(acc.DataKey))
                    _famRepo.RemoveAcceso(rolId, acc.Id);
            }

            // Agregar faltantes
            foreach (var p in target)
            {
                if (actualesSet.Contains(p)) continue;

                if (!porKey.TryGetValue(p, out var acc))
                {
                    acc = _accRepo.Create(p.ToString().Replace('_', ' '), p);
                    porKey[p] = acc;
                }

                _famRepo.AddAcceso(rolId, acc.Id);
            }
        }

        public sealed class RolPlano
        {
            public Guid IdRol { get; set; }
            public string Nombre { get; set; }
        }

        public List<RolPlano> RolesDeUsuario(Guid idUsuario)
        {
            var raws = _famRepo.GetRolesDeUsuario(idUsuario); // (Id, Nombre)
            return raws.Select(t => new RolPlano { IdRol = t.Id, Nombre = t.Nombre }).ToList();
        }
    }
}
