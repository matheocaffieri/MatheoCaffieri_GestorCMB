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
            throw new NotImplementedException();
        }

       

        public void Delete(DomainModel.Empleado entity)
        {
            throw new NotImplementedException();
        }


        public void Update(DomainModel.Empleado entity)
        {
            throw new NotImplementedException();
        }

        List<DomainModel.Empleado> IGenericRepository<DomainModel.Empleado>.GetAll()
        {
            return empleadoRepository.GetAll();
        }

        DomainModel.Empleado IGenericRepository<DomainModel.Empleado>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
