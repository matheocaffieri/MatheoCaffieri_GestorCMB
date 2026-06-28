using Services.Login;
using System.Collections.Generic;

namespace Services.Services_Interfaces
{
    public interface IAccesoRepository
    {
        List<Acceso> GetAll();
        Acceso Create(string nombre, TipoPermiso dataKey);
    }
}
