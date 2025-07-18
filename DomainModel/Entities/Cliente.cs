using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class Cliente
    {
        public Guid IdCliente { get; set; }
        public string NombreContacto { get; set; }
        public string RazonSocial { get; set; }
        public string Mail { get; set; }
        public int Telefono { get; set; }
        public bool IsActive { get; set; }
        public List<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
    }
}
