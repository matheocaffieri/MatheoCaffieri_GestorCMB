using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IMaterialFaltanteBL
    {
        void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad);
        List<MaterialFaltante> GetAll(Guid idProyecto);
    }
}
