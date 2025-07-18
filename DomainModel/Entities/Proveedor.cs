using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Proveedor
    {
        public Guid IdProveedor { get; set; }
        public string Descripcion { get; set; }
        public int Telefono { get; set; }
        public bool IsActive { get; set; }
        public List<Material> Materiales { get; set; } = new List<Material>();
    }
}
