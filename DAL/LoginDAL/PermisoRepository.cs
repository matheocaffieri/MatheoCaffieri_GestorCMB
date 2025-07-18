using DomainModel.Interfaces;
using DomainModel.Login;
using Services.LoginComposite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LoginDAL
{
    public class PermisoRepository : IPermisoRepository
    {
        public IEnumerable<IPermiso> GetAll() => throw new NotImplementedException();
        public IPermiso GetById(Guid id) => throw new NotImplementedException();
        public void Add(IPermiso entity) => throw new NotImplementedException();
        public void Update(IPermiso entity) => throw new NotImplementedException();

        public void Delete(IPermiso entity)
        {
            throw new NotImplementedException();
        }

        List<IPermiso> IGenericRepository<IPermiso>.GetAll()
        {
            throw new NotImplementedException();
        }
    }

}
