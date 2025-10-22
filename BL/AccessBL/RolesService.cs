using DAL.AccessDAL;
using DomainModel.Login;
using Interfaces;                 // IFamiliaRepository
using Interfaces.LoginInterfaces; // IAccesoRepository
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.AccessBL
{
    public class RolesService
    {
        private readonly IFamiliaRepository _famRepo;
        private readonly IAccesoRepository _accRepo;

        internal RolesService(IFamiliaRepository famRepo, IAccesoRepository accRepo)
        {
            if (famRepo == null) throw new ArgumentNullException(nameof(famRepo));
            if (accRepo == null) throw new ArgumentNullException(nameof(accRepo));
            _famRepo = famRepo;
            _accRepo = accRepo;
        }

        public RolesService(string cs)
        {
            _famRepo = new DAL.AccessDAL.FamiliaRepository(cs);
            _accRepo = new DAL.AccessDAL.AccesoRepository(cs);
        }

        // ----- Roles + sus accesos (objetos compuestos para UI si los usás) -----

        public List<RolCompuesto> ListarRoles()
        {
            var roles = new List<RolCompuesto>();

            // GetAll(): List<(Guid Id, string Nombre)>
            foreach (var tupla in _famRepo.GetAll())
            {
                Guid id = tupla.Id;
                string nombre = tupla.Nombre;

                var rol = new RolCompuesto(id, nombre);
                var propId = typeof(RolCompuesto).GetProperty("Id");
                if (propId != null) propId.SetValue(rol, id, null);

                var accesos = _famRepo.GetAccesos(id); // List<Acceso>
                foreach (var acc in accesos)
                    rol.AgregarHijo(acc);

                roles.Add(rol);
            }
            return roles;
        }

        // CRUD de rol
        public Guid CrearRol(string nombre) { return _famRepo.Create(nombre); }
        public void AsignarPermisoARol(Guid idRol, Guid idAcceso) { _famRepo.AddAcceso(idRol, idAcceso); }
        public void QuitarPermisoDeRol(Guid idRol, Guid idAcceso) { _famRepo.RemoveAcceso(idRol, idAcceso); }
        public List<Usuario> UsuariosDelRol(Guid idRol) { return _famRepo.GetUsuarios(idRol); }
        public void AsignarUsuarioARol(Guid idRol, Guid idUsuario) { _famRepo.AddUsuario(idRol, idUsuario); }
        public void QuitarUsuarioDeRol(Guid idRol, Guid idUsuario) { _famRepo.RemoveUsuario(idRol, idUsuario); }

        // Exponer accesos “sueltos” si los necesitás desde acá
        public List<Acceso> ListarAccesos() { return _accRepo.GetAll(); }
        public Acceso CrearAcceso(string nombre, TipoPermiso key) { return _accRepo.Create(nombre, key); }

        // ----- LO NUEVO: permisos (TipoPermiso) de un rol -----

        // a) Obtener los TipoPermiso vigentes de un rol
        public IEnumerable<TipoPermiso> ObtenerPermisosDeRol(Guid rolId)
        {
            var accesos = _famRepo.GetAccesos(rolId);       // List<Acceso>
            return accesos.Select(a => a.DataKey).ToList(); // materializo por seguridad
        }

        // b) Reemplazar completamente los permisos (TipoPermiso) de un rol
        public void ReemplazarPermisosDeRol(Guid rolId, IEnumerable<TipoPermiso> nuevos)
        {
            var target = new HashSet<TipoPermiso>(
                nuevos ?? Enumerable.Empty<TipoPermiso>());

            // Accesos actuales del rol
            var actuales = _famRepo.GetAccesos(rolId); // List<Acceso>
            var actualesSet = new HashSet<TipoPermiso>(actuales.Select(a => a.DataKey));

            // Mapa DataKey -> Acceso (para resolver IdAcceso)
            var todos = _accRepo.GetAll();
            var porKey = new Dictionary<TipoPermiso, Acceso>();
            foreach (var a in todos)
            {
                if (!porKey.ContainsKey(a.DataKey))
                    porKey.Add(a.DataKey, a);
            }

            // Quitar los que ya no deben estar
            foreach (var acc in actuales)
            {
                if (!target.Contains(acc.DataKey))
                    _famRepo.RemoveAcceso(rolId, acc.Id);
            }

            // Agregar los que faltan
            foreach (var p in target)
            {
                if (actualesSet.Contains(p)) continue;

                Acceso acc;
                if (!porKey.TryGetValue(p, out acc))
                {
                    // Si no existe en tabla Acceso, lo creo con nombre por defecto
                    acc = _accRepo.Create(p.ToString().Replace('_', ' '), p);
                    porKey[p] = acc;
                }
                _famRepo.AddAcceso(rolId, acc.Id);
            }
        }



        public class RolPlano
        {
            public Guid IdRol { get; set; }
            public string Nombre { get; set; }
        }

        public List<RolPlano> RolesDeUsuario(Guid idUsuario)
        {
            var raws = _famRepo.GetRolesDeUsuario(idUsuario); // (Id, Nombre)
            return raws.Select(t => new RolPlano { IdRol = t.Id, Nombre = t.Nombre })
                       .ToList();
        }
    }
}
