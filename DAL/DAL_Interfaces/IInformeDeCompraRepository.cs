using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Entities;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IInformeDeCompraRepository : IGenericRepository<InformeDeCompra>
    {
        List<InformeDeCompra> GetByProyecto(Guid idProyecto);
        bool ExistsForProyectoOnDate(Guid idProyecto, DateTime fecha);
        List<InformeDeCompra> GetHistorial();
        HashSet<Guid> GetMaterialesConInformesPendientes();
    }
}
