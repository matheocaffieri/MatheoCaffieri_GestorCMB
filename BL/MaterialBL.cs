using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;

namespace BL
{
    public class MaterialBL : IMaterialRepository
    {
        private readonly IMaterialRepository materialRepository;

        public MaterialBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            materialRepository = new MaterialRepository(uow);
        }

        // ===== CRUD (dejados como NotImplemented igual que tu InventarioBL) =====
        public void Add(Material entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Material entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Material entity)
        {
            throw new NotImplementedException();
        }

        // ===== Implementación explícita del IGenericRepository =====
        List<Material> IGenericRepository<Material>.GetAll()
        {
            return materialRepository.GetAll();
        }

        Material IGenericRepository<Material>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
