using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class InformeDeCompra
    {
        public Guid IdInformeCompra { get; set; }
        public Guid IdProyecto { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}
