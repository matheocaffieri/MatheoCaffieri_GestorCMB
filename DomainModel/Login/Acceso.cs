using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Login
{
    public class Acceso : IPermiso
    {
        public string Nombre { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public TipoPermiso DataKey { get; private set; }

        public Acceso(string nombre, TipoPermiso dataKey)
        {
            Nombre = nombre;
            DataKey = dataKey;
        }

        public bool TienePermiso(TipoPermiso permiso)
        {
            return DataKey == permiso;
        }
    }
}
