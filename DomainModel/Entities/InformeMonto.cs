using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class InformeMonto
    {
        public Guid IdInformeMonto { get; set; }
        public Guid IdProyecto { get; set; }
        public float TotalMateriales { get; set; }
        public float TotalEmpleados { get; set; }
        public float MontoTotal { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}
