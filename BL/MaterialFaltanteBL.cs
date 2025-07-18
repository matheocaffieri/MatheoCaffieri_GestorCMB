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
    public class MaterialFaltanteBL : IMaterialesFaltantesRepository
    {
        private readonly IMaterialesFaltantesRepository materialesFaltantesRepository;

        public MaterialFaltanteBL()
        {
            var context = new GestorCMBEntities();
            materialesFaltantesRepository = new MaterialFaltanteRepository(context);
        }

        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            return materialesFaltantesRepository.GetAll(idProyecto);
        }
    }
}
