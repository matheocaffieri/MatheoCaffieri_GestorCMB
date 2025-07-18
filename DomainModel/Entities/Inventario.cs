using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Inventario
    {
        public Guid IdMaterialInventario { get; set; }
        public int Cantidad { get; set; }
        public Guid IdMaterial { get; set; }
        public Material Material { get; set; }
    }
}
