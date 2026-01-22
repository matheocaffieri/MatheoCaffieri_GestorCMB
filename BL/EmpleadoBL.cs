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
    public class EmpleadoBL : IEmpleadoRepository
    {
        private readonly IEmpleadoRepository _repo;
        private readonly IUnitOfWork _uow;

        public EmpleadoBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _repo = new EmpleadoRepository(_uow);
        }

        // (Opcional) ctor para DI/tests
        public EmpleadoBL(IUnitOfWork uow, IEmpleadoRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Add(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity.IdEmpleado == Guid.Empty)
                entity.IdEmpleado = Guid.NewGuid();

            LoggerLogic.Info($"[EmpleadoBL] Add START. Id={entity.IdEmpleado}");

            _uow.Begin();
            try
            {
                _repo.Add(entity);  // pendiente commit
                _uow.Commit();

                LoggerLogic.Info($"[EmpleadoBL] Add OK. Id={entity.IdEmpleado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Add ERROR. Id={entity.IdEmpleado}. {ex.Message}");
                throw;
            }
        }

        public void Update(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdEmpleado == Guid.Empty) throw new ArgumentException("IdEmpleado requerido.", nameof(entity));

            LoggerLogic.Info($"[EmpleadoBL] Update START. Id={entity.IdEmpleado}");

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();

                LoggerLogic.Info($"[EmpleadoBL] Update OK. Id={entity.IdEmpleado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Update ERROR. Id={entity.IdEmpleado}. {ex.Message}");
                throw;
            }
        }

        public void Delete(DomainModel.Empleado entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdEmpleado == Guid.Empty) throw new ArgumentException("IdEmpleado requerido.", nameof(entity));

            LoggerLogic.Info($"[EmpleadoBL] Delete START. Id={entity.IdEmpleado}");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();

                LoggerLogic.Info($"[EmpleadoBL] Delete OK. Id={entity.IdEmpleado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Delete ERROR. Id={entity.IdEmpleado}. {ex.Message}");
                throw;
            }
        }

        // ===== Lecturas (sin transacción) =====

        List<DomainModel.Empleado> IGenericRepository<DomainModel.Empleado>.GetAll()
        {
            try
            {
                return _repo.GetAll();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EmpleadoBL] GetAll ERROR. {ex.Message}");
                throw;
            }
        }

        DomainModel.Empleado IGenericRepository<DomainModel.Empleado>.GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id requerido.", nameof(id));
            try
            {
                return _repo.GetById(id);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EmpleadoBL] GetById ERROR. Id={id}. {ex.Message}");
                throw;
            }
        }
    }
}
