using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IDetalleInformeCompraBL
    {
        List<MaterialFaltante> GetMaterialesFaltantesDelInforme(Guid idInformeCompra);
    }
}
