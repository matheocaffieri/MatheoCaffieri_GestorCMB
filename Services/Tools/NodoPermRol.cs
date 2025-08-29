using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tools
{
    public class NodoPermRol
    {
        public Guid RolId { get; set; }
        public TipoPermiso Permiso { get; set; }
    }
}
