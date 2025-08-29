using DomainModel.Login;
using Interfaces;
using Interfaces.LoginInterfaces; // IAccesoRepository
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL.AccessBL
{
    public class AccesoService
    {
        private readonly IAccesoRepository _repo;

        // Oculto para UI: inyección directa
        internal AccesoService(IAccesoRepository repo)
        {
            if (repo == null) throw new ArgumentNullException(nameof(repo));
            _repo = repo;
        }

        // Público para UI: recibe connection string
        public AccesoService(string connectionString)
            : this(new DAL.AccessDAL.AccesoRepository(connectionString))
        { }

        // ----- Operaciones sobre Acceso -----

        public List<Acceso> Listar()
        {
            return _repo.GetAll();
        }

        public Acceso Crear(string nombre, TipoPermiso key)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre requerido.", "nombre");

            return _repo.Create(nombre.Trim(), key);
        }

        // Búsqueda simple por DataKey (sin tocar interfaz del repo)
        public Acceso BuscarPorKey(TipoPermiso key)
        {
            var todos = _repo.GetAll();
            return todos.FirstOrDefault(a => a.DataKey == key);
        }

        // Devuelve el Id del Acceso para un TipoPermiso; si no existe, lo crea
        public Guid GetOrCreateId(TipoPermiso key, string nombreFallback)
        {
            var acc = BuscarPorKey(key);
            if (acc == null)
            {
                var nombre = string.IsNullOrWhiteSpace(nombreFallback)
                    ? key.ToString().Replace('_', ' ')
                    : nombreFallback.Trim();

                acc = _repo.Create(nombre, key);
            }

            var prop = typeof(Acceso).GetProperty("Id");
            return (Guid)prop.GetValue(acc, null);
        }

        // Seed: crea accesos faltantes a partir del enum
        public int SeedDesdeEnum()
        {
            var actuales = _repo.GetAll();
            var set = new HashSet<string>(
                actuales.Select(a => a.DataKey.ToString()),
                StringComparer.OrdinalIgnoreCase);

            var creados = 0;
            foreach (TipoPermiso p in Enum.GetValues(typeof(TipoPermiso)))
            {
                if (!set.Contains(p.ToString()))
                {
                    _repo.Create(p.ToString().Replace('_', ' '), p);
                    creados++;
                }
            }
            return creados;
        }
    }
}
