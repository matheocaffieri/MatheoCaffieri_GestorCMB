using DAL.ProjectRepo;
using DAL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.FactoryDAL;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MaterialFaltanteBL : IMaterialesFaltantesRepository
    {
        private readonly IMaterialesFaltantesRepository materialesFaltantesRepository;

        public MaterialFaltanteBL()
        {
            var context = new DAL.GestorCMBEntities();
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(context.Database.Connection.ConnectionString);
            materialesFaltantesRepository = new MaterialFaltanteRepository(uow);
        }

        public void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad)
        {
            materialesFaltantesRepository.AddOrUpdate(idProyecto, descripcion, tipoMaterial, tipoUnidad, cantidad);
        }

        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            return materialesFaltantesRepository.GetAll(idProyecto);
        }
    }
}
