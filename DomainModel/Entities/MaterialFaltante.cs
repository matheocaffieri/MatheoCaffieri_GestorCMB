using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class MaterialFaltante
    {
        public Guid IdMaterialFaltante { get; set; }
        public string DescripcionArticuloFaltante { get; set; }
        public string TipoMaterialFaltante { get; set; }
        public string TipoUnidadMaterialFaltante { get; set; }
        public Guid IdProyecto { get; set; }
        public int CantidadFaltante { get; set; }
    }
}
