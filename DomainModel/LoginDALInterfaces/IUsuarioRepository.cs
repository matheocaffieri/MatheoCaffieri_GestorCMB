using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Login;

namespace DAL.LoginDAL
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario FindByEmail(string mail);
        // Interfaces.LoginInterfaces
        void SetActivo(Guid idUsuario, bool activo);
        

    }
}
