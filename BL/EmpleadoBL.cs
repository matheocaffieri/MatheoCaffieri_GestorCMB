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
    public class EmpleadoBL : IEmpleadoRepository
    {
        private readonly IEmpleadoRepository empleadoRepository;
        public EmpleadoBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            empleadoRepository = new EmpleadoRepository(uow);
        }

        

        public void Add(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            empleadoRepository.Add(entity);
        }

       

        public void Delete(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            empleadoRepository.Delete(entity);
        }


        public void Update(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            empleadoRepository.Update(entity);
        }

        List<DomainModel.Empleado> IGenericRepository<DomainModel.Empleado>.GetAll()
        {
            return empleadoRepository.GetAll();
        }

        DomainModel.Empleado IGenericRepository<DomainModel.Empleado>.GetById(Guid id)
        {
            return empleadoRepository.GetById(id);
        }
    }
}
