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
    public class DetalleEmpleadoBL : IDetalleEmpleadosRepository
    {

        private readonly IDetalleEmpleadosRepository detalleEmpleadoRepository;
        public DetalleEmpleadoBL()
        {
            var context = new GestorCMBEntities();
            detalleEmpleadoRepository = new DetalleEmpleadosRepository(context);
        }



        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            return detalleEmpleadoRepository.GetAll(idProyecto);
        }



    }
}
