using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IInformeDeCompraBL
    {
        Guid GenerarDesdeFaltantes(Guid idProyecto, bool unicoPorDia = true);
        List<InformeDeCompra> GetAll();
        List<InformeDeCompra> GetHistorial();
        HashSet<Guid> GetMaterialesConInformesPendientes();
        void EliminarInforme(Guid idInformeCompra);
        void ConfirmarCompraYAplicar(Guid idProyecto, Guid idInformeCompra);
    }
}
