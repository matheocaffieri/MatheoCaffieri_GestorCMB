using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class DetalleInformeMaterialFaltante
    {
        public Guid IdDetalleMaterialFaltante { get; set; }
        public Guid IdInformeCompra { get; set; }
        public Guid IdMaterialFaltante { get; set; }
    }
}
