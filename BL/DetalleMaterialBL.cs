using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DetalleMaterialBL : IDetalleMaterialesRepository
    {
        private readonly IDetalleMaterialesRepository _repo;
        private readonly IUnitOfWork _uow;

        public DetalleMaterialBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _repo = new DetalleMaterialesRepository(_uow);
        }

        // (Opcional) ctor para DI/tests
        public DetalleMaterialBL(IUnitOfWork uow, IDetalleMaterialesRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idMaterial == Guid.Empty) throw new ArgumentException("idMaterial requerido.", nameof(idMaterial));
            if (cantidad <= 0) return;

            LoggerLogic.Info($"[DetalleMaterialBL] AddOrUpdate START. idProyecto={idProyecto}, idMaterial={idMaterial}, cant={cantidad}");

            _uow.Begin();
            try
            {
                _repo.AddOrUpdate(idProyecto, idMaterial, cantidad, valorGanancia, fechaIngreso); // pendiente commit
                _uow.Commit();

                LoggerLogic.Info($"[DetalleMaterialBL] AddOrUpdate OK. idProyecto={idProyecto}, idMaterial={idMaterial}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleMaterialBL] AddOrUpdate ERROR. idProyecto={idProyecto}, idMaterial={idMaterial}. {ex.Message}");
                throw;
            }
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));

            try
            {
                return _repo.GetAll(idProyecto);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleMaterialBL] GetAll ERROR. idProyecto={idProyecto}. {ex.Message}");
                throw;
            }
        }
    }
}
