using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetAllUsuarios();
        Usuario GetUsuarioById(Guid id);
        Usuario FindByEmail(string mail);
        void CreateUsuario(Usuario u);
        void UpdateUsuario(Usuario u);
        void DeleteUsuario(Usuario u);
    }
}
