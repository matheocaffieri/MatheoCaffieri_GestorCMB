using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.ProjectRepo;

namespace BL
{
    public class ProveedorBLL : IProveedorRepository
    {
        IProveedorRepository _proveedorRepository = new ProveedorRepo();
        public void Add(Proveedor entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Proveedor entity)
        {
            throw new NotImplementedException();
        }

        public List<Proveedor> GetAll()
        {
            return _proveedorRepository.GetAll();
        }

        public Proveedor GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Proveedor entity)
        {
            throw new NotImplementedException();
        }
    }
}
