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
    public class DetalleMaterialBL : IDetalleMaterialesRepository
    {
        private readonly IDetalleMaterialesRepository detalleMaterialRepository;
        public DetalleMaterialBL()
        {
            var context = new GestorCMBEntities();
            detalleMaterialRepository = new DetalleMaterialesRepository(context);
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            return detalleMaterialRepository.GetAll(idProyecto);
        }
    }
}
