using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class DetalleProyectoMaterial
    {
        public Guid IdDetalleMaterial { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdMaterial { get; set; }
        public int Cantidad { get; set; }
        public float ValorGanancia { get; set; }
        public DateTime FechaIngresoMaterial { get; set; }
        public Proyecto Proyecto { get; set; }
        public virtual Material Material { get; set; }
    }
}
