using DAL.ProjectRepo;
using DAL;
using DAL.FactoryDAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DetalleEmpleadoBL : IDetalleEmpleadosRepository
    {

        private readonly IDetalleEmpleadosRepository detalleEmpleadoRepository;
        public DetalleEmpleadoBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            detalleEmpleadoRepository = new DetalleEmpleadosRepository(uow);
        }



        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            return detalleEmpleadoRepository.GetAll(idProyecto);
        }



    }
}
