using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Proyecto
    {
        public Guid IdProyecto { get; set; }
        public Guid IdCliente { get; set; }
        public string Descripcion { get; set; }
        public EnumEstado Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public Cliente Cliente { get; set; }
        public List<InformeDeCompra> InformesCompra { get; set; } = new List<InformeDeCompra>();
        public List<InformeMonto> InformesMontos { get; set; } = new List<InformeMonto>();
        public List<DetalleProyectoEmpleado> DetallesEmpleados { get; set; } = new List<DetalleProyectoEmpleado>();
        public List<DetalleProyectoMaterial> DetallesMateriales { get; set; } = new List<DetalleProyectoMaterial>();
        public List<MaterialFaltante> MaterialFaltantes { get; set; } = new List<MaterialFaltante>();

    }
}
