using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.LoginInterfaces;

namespace DomainModel.Login
{
    public class Usuario
    {
        public Guid IdUsuario { get; set; }
        public string Mail { get; set; }
        public string Contraseña { get; set; }
        public bool IsActive { get; set; }
        public int Telefono { get; set; }
        public string Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }

        private readonly List<IPermiso> _permisos = new List<IPermiso>();
        public IReadOnlyCollection<IPermiso> Permisos => _permisos;

        public void AgregarPermiso(IPermiso p)
        {
            _permisos.Add(p);
        }

        public bool TienePermiso(TipoPermiso permiso)
        {
            return _permisos.Any(x => x.TienePermiso(permiso));
        }
    }
}
