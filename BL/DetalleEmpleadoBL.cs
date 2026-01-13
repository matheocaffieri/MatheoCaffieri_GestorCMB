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
        private readonly IDetalleEmpleadosRepository _repo;

        public DetalleEmpleadoBL()
        {
            var context = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(context.Database.Connection.ConnectionString);
            _repo = new DetalleEmpleadosRepository(uow);
        }

        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
            => _repo.GetAll(idProyecto);

        public bool Exists(Guid idProyecto, Guid idEmpleado)
            => _repo.Exists(idProyecto, idEmpleado);

        // estado varchar: "1" activo, "0" inactivo
        public void Add(DetalleProyectoEmpleado detalle, string estado = "1")
            => _repo.Add(detalle, estado);

        public DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado)
            => _repo.GetByProyectoEmpleado(idProyecto, idEmpleado);

        public void Update(DetalleProyectoEmpleado detalle)
            => _repo.Update(detalle);

        public void SetEstado(Guid idDetalleEmpleado, string estado)
            => _repo.SetEstado(idDetalleEmpleado, estado);

    }
}
