using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Empleado
    {
        public Guid IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int NroDocumento { get; set; }
        public float Sueldo { get; set; }
        public int CantidadProyectosActivos { get; set; }
        public bool IsActive { get; set; }
        public List<DetalleProyectoEmpleado> DetallesProyectos { get; set; } = new List<DetalleProyectoEmpleado>();
    }
}
