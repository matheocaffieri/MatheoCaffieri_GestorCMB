using DomainModel.Interfaces;
using DomainModel.Login;
using Services.LoginComposite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginDAL
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // Aquí iría tu DbContext o equivalente
        public IEnumerable<Usuario> GetAll() => throw new NotImplementedException();
        public Usuario GetById(Guid id) => throw new NotImplementedException();
        public void Add(Usuario entity) => throw new NotImplementedException();
        public void Update(Usuario entity) => throw new NotImplementedException();
        public Usuario FindByEmail(string mail) => throw new NotImplementedException();

        public void Delete(Usuario entity)
        {
            throw new NotImplementedException();
        }

        List<Usuario> IGenericRepository<Usuario>.GetAll()
        {
            throw new NotImplementedException();
        }
    }


}
