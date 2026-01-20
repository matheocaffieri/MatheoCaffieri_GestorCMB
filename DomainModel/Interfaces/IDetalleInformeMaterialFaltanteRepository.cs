using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IDetalleInformeMaterialFaltanteRepository : IGenericRepository<DetalleInformeMaterialFaltante>
    {
        List<DetalleInformeMaterialFaltante> GetByInforme(Guid idInformeCompra);
        bool Exists(Guid idInformeCompra, Guid idMaterialFaltante);
    }
}
