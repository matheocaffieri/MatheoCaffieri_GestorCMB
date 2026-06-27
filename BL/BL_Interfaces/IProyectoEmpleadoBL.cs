using System;

namespace BL.BL_Interfaces
{
    public interface IProyectoEmpleadoBL
    {
        void AgregarEmpleadoDetalleProyecto(Guid idProyecto, Guid idEmpleado, double valorGanancia);
        void QuitarEmpleadoDelProyecto(Guid idProyecto, Guid idEmpleado);
    }
}
