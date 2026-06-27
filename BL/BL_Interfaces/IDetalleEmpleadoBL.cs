using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IDetalleEmpleadoBL
    {
        List<DetalleProyectoEmpleado> GetAll(Guid idProyecto);
        bool Exists(Guid idProyecto, Guid idEmpleado);
        DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado);
        void Add(DetalleProyectoEmpleado detalle, string estado = "1");
        void Update(DetalleProyectoEmpleado detalle);
        void SetEstado(Guid idDetalleEmpleado, string estado);
    }
}
