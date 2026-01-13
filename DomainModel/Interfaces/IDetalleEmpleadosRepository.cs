using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces
{
    public interface IDetalleEmpleadosRepository : IDetalleGeneric<DetalleProyectoEmpleado>
    {
        bool Exists(Guid idProyecto, Guid idEmpleado);
        void Add(DetalleProyectoEmpleado detalle, string estado);
        void SetEstado(Guid idDetalleEmpleado, string estado);
        DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado); // si no está
        void Update(DetalleProyectoEmpleado detalle);
    }
}
