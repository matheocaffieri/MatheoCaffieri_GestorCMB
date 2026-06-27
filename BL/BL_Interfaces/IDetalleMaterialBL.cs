using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IDetalleMaterialBL
    {
        void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso);
        List<DetalleProyectoMaterial> GetAll(Guid idProyecto);
        int Delete(Guid idProyecto, Guid idMaterial);
    }
}
