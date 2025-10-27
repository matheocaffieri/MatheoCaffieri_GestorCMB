using DAL.ProjectRepo;
using DAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class InventarioBL : IInventarioRepository
    {
        private readonly IInventarioRepository inventarioRepository;
        public InventarioBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            inventarioRepository = new InventarioRepository(uow);
        }

        

        public void Add(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }


        public void Update(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }

        List<DomainModel.Inventario> IGenericRepository<DomainModel.Inventario>.GetAll()
        {
            return inventarioRepository.GetAll();
        }

        DomainModel.Inventario IGenericRepository<DomainModel.Inventario>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
