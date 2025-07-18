using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class DetalleProyectoEmpleado
    {
        public Guid IdDetalleProyectoEmpleado { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdEmpleado { get; set; }
        public DateTime FechaIngresoEmpleado { get; set; }
        public float ValorGanancia { get; set; }
        public Proyecto Proyecto { get; set; }
        public virtual Empleado Empleado { get; set; }

    }
}
