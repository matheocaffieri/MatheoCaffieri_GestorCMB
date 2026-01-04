using DomainModel.Login;
using Interfaces.LoginInterfaces; // IAccesoRepository
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.LoginDALInterfaces;

namespace Services.RoleService.Logic
{
    public class AccesoService
    {
        private readonly IAccesoRepository _repo;

        public AccesoService(IAccesoRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public List<Acceso> Listar() => _repo.GetAll();

        public Acceso Crear(string nombre, TipoPermiso key)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nombre requerido.", nameof(nombre));

            return _repo.Create(nombre.Trim(), key);
        }

        public Acceso BuscarPorKey(TipoPermiso key)
            => _repo.GetAll().FirstOrDefault(a => a.DataKey == key);

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
            return acc.Id;
        }

        public int SeedDesdeEnum()
        {
            var actuales = _repo.GetAll();
            var set = new HashSet<TipoPermiso>(actuales.Select(a => a.DataKey));

            var creados = 0;
            foreach (TipoPermiso p in Enum.GetValues(typeof(TipoPermiso)))
            {
                if (!set.Contains(p))
                {
                    _repo.Create(p.ToString().Replace('_', ' '), p);
                    creados++;
                }
            }
            return creados;
        }
    }
}
