using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Login
{
    public interface IPermiso
    {
        bool TienePermiso(TipoPermiso permiso);
        string Nombre { get; }
        Guid Id { get; }
    }
}
