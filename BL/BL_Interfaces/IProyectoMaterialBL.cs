using System;
using DomainModel.Entities;

namespace BL.BL_Interfaces
{
    public interface IProyectoMaterialBL
    {
        AsignacionMaterialResult AgregarMaterialDetalleProyectoDesdeInventario(
            Guid idProyecto, Guid idMaterial, int cantidadSolicitada, double valorGanancia);
        void QuitarMaterialDelProyecto(Guid idProyecto, Guid idMaterial);
    }
}
