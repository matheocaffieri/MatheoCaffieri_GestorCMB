using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IDetalleMaterialesRepository : IDetalleGeneric<DetalleProyectoMaterial>
    {
        void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso);
    }
}
