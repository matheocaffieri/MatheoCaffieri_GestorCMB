using DAL.ProjectRepo;
using DAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.FactoryDAL;
using System.Threading.Tasks;

namespace BL
{
    public class DetalleMaterialBL : IDetalleMaterialesRepository
    {
        private readonly IDetalleMaterialesRepository detalleMaterialRepository;
        public DetalleMaterialBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            detalleMaterialRepository = new DetalleMaterialesRepository(uow);
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            return detalleMaterialRepository.GetAll(idProyecto);
        }
    }
}
