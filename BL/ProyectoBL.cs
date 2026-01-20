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
    public class ProyectoBL : IProyectoRepository
    {

        private readonly IProyectoRepository proyectoRepository;
        public ProyectoBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            proyectoRepository = new ProyectoRepository(uow);
        }



        public void Add(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            proyectoRepository.Add(entity);
        }

        public void Delete(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            proyectoRepository.Delete(entity);
        }

        public List<DomainModel.Proyecto> GetAll()
        {
            return proyectoRepository.GetAll();
        }

        public DomainModel.Proyecto GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id requerido.", nameof(id));
            return proyectoRepository.GetById(id);
        }

        public void Update(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            proyectoRepository.Update(entity);
        }

    }
}
