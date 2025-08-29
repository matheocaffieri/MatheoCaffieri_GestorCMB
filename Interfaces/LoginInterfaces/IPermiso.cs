using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.LoginInterfaces
{
    public interface IPermiso
    {
        bool TienePermiso(TipoPermiso permiso);
        string Nombre { get; }
        Guid Id { get; }
    }
}
