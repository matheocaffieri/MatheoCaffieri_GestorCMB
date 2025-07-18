using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Material
    {
        public Guid IdMaterial { get; set; }
        public string DescripcionArticulo { get; set; }
        public string TipoMaterial { get; set; }
        public string TipoUnidad { get; set; }
        public float CostoPorUnidad { get; set; }
        public Guid IdProveedor { get; set; }
        public List<DetalleProyectoMaterial> DetallesProyectos { get; set; } = new List<DetalleProyectoMaterial>();
        public Proveedor Proveedor { get; set; }
    }
}
