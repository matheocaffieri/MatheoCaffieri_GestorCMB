using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IDetalleInformeMaterialFaltanteRepository : IGenericRepository<DetalleInformeMaterialFaltante>
    {
        List<DetalleInformeMaterialFaltante> GetByInforme(Guid idInformeCompra);
        bool Exists(Guid idInformeCompra, Guid idMaterialFaltante);
        void DeleteByMaterialFaltanteIds(IEnumerable<Guid> ids);
    }
}
