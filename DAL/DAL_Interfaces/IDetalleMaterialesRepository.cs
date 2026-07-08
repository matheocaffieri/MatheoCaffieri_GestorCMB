using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IDetalleMaterialesRepository : IDetalleGeneric<DetalleProyectoMaterial>
    {
        void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso);
        int Delete(Guid idProyecto, Guid idMaterial);
        bool ExistsByMaterial(Guid idMaterial);
    }
}
