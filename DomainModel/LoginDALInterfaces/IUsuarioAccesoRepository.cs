using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.LoginDALInterfaces
{
    public interface IUsuarioAccesoRepository
    {
        List<Acceso> GetDirectos(Guid idUsuario);
        void ReplaceDirectos(Guid idUsuario, IEnumerable<Guid> idsAcceso);
    }
}
