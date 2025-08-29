using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.LoginInterfaces;

namespace DomainModel.Login
{
    public class RolCompuesto : IPermiso
    {
        public string Nombre { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();
        private readonly List<IPermiso> _hijos = new List<IPermiso>();

        public RolCompuesto(string nombre)
        {
            Nombre = nombre;
        }

        public void AgregarHijo(IPermiso componente) => _hijos.Add(componente);
        public void QuitarHijo(IPermiso componente) => _hijos.Remove(componente);

        public bool TienePermiso(TipoPermiso permiso)
        {
            foreach (var hijo in _hijos)
            {
                if (hijo.TienePermiso(permiso))
                    return true;
            }
            return false;
        }

        public IReadOnlyCollection<IPermiso> ObtenerHijos() => _hijos.AsReadOnly();
    }

}
