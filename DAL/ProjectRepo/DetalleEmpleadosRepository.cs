using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.ProjectRepo
{
    public class DetalleEmpleadosRepository : IDetalleEmpleadosRepository
    {
        private readonly GestorCMBEntities _context;

        public DetalleEmpleadosRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        

        public List<DomainModel.DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            return _context.Detalle_proyecto_empleado
                .AsNoTracking()
                .Where(d => d.idProyecto == idProyecto) // Filtra por el ID del proyecto
                .Include(d => d.Empleado) // Incluye los datos del empleado
                .Select(d => new DomainModel.DetalleProyectoEmpleado
                {
                    IdDetalleProyectoEmpleado = d.idDetalleEmpleado,
                    IdProyecto = d.idProyecto,
                    IdEmpleado = d.idEmpleado,
                    FechaIngresoEmpleado = d.fechaIngresoEmpleado,
                    ValorGanancia = (float)d.valorGanancia,

                    Empleado = new DomainModel.Empleado
                    {
                        IdEmpleado = d.Empleado.idEmpleado,
                        Nombre = d.Empleado.nombre,
                        Apellido = d.Empleado.apellido,
                        NroDocumento = d.Empleado.nroDocumento,
                        Sueldo = (float)d.Empleado.sueldo
                    }
                })
                .ToList();
        }





       
    }
}
