using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IMaterialesFaltantesRepository : IDetalleGeneric<MaterialFaltante>
    {
        void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad);
        List<Guid> GetIdsByProyecto(Guid idProyecto);
        List<MaterialFaltante> GetByIds(IEnumerable<Guid> ids);
        List<MaterialFaltante> GetByIdsAndProyecto(IEnumerable<Guid> ids, Guid idProyecto);
        void DeleteByIds(IEnumerable<Guid> ids);
    }
}
