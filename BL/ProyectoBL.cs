using DAL.ProjectRepo;
using DAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Entities;
using System.Threading.Tasks;

namespace BL
{
    public class ProyectoBL : IProyectoRepository
    {
        public void Add(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Add(entity);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public void Update(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Update(entity);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public void Delete(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Delete(entity);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public List<DomainModel.Proyecto> GetAll()
        {
            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                var repo = new ProyectoRepository(uow);
                return repo.GetAll();
            }
        }

        public DomainModel.Proyecto GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id requerido.", nameof(id));

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                var repo = new ProyectoRepository(uow);
                return repo.GetById(id);
            }
        }
    }
}
