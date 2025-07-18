using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Login
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Usuario FindByEmail(string mail);
    }
}
