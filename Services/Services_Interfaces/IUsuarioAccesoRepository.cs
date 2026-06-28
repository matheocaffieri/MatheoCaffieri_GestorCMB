using Services.Login;
using System;
using System.Collections.Generic;

namespace Services.Services_Interfaces
{
    public interface IUsuarioAccesoRepository
    {
        List<Acceso> GetDirectos(Guid idUsuario);
        void ReplaceDirectos(Guid idUsuario, IEnumerable<Guid> idsAcceso);
    }
}
