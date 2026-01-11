using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IMaterialesFaltantesRepository : IDetalleGeneric<MaterialFaltante>
    {
        void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad);
    }
}
