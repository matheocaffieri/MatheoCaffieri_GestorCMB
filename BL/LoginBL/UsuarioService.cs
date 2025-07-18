using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.LoginBL
{
   public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }
        public IEnumerable<Usuario> GetAllUsuarios() => _repo.GetAll();
        public Usuario GetUsuarioById(Guid id) => _repo.GetById(id);
        public Usuario FindByEmail(string mail) => _repo.FindByEmail(mail);
        public void CreateUsuario(Usuario u) => _repo.Add(u);
        public void UpdateUsuario(Usuario u) => _repo.Update(u);
        public void DeleteUsuario(Usuario u) => _repo.Delete(u);
    } 
}
