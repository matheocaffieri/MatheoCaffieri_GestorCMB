using DAL.ProjectRepo;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
using DAL;

namespace BL
{
    public class ProveedorBL : IProveedorRepository
    {
        private readonly IProveedorRepository proveedorRepository;

        public ProveedorBL()
        {
            
            var context = new GestorCMBEntities();
            proveedorRepository = new ProveedorRepository(context);
        }

        

        public void Add(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }

        

        public void Delete(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }

       

        public DomainModel.Proveedor GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }

        List<DomainModel.Proveedor> IGenericRepository<DomainModel.Proveedor>.GetAll()
        {
            return proveedorRepository.GetAll();
        }
    }

}
