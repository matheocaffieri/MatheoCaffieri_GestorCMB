using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DAL.DAL_Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class EmpleadoBL : IEmpleadoBL
    {
        private readonly IEmpleadoRepository _repo;
        private readonly IUnitOfWork _uow;

        public EmpleadoBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _repo = new EmpleadoRepository(_uow);
        }

        // DI / tests
        public EmpleadoBL(IUnitOfWork uow, IEmpleadoRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Add(DomainModel.Empleado entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            if (entity.IdEmpleado == Guid.Empty)
                entity.IdEmpleado = Guid.NewGuid();

            _uow.Begin();
            try
            {
                _repo.Add(entity);
                _uow.Commit();
                LoggerLogic.Info($"[EmpleadoBL] Empleado agregado. Id={entity.IdEmpleado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[EmpleadoBL] Validación al agregar empleado: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Falla al agregar empleado. Id={entity.IdEmpleado}", ex);
                throw;
            }
        }

        public void Update(DomainModel.Empleado entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();
                LoggerLogic.Info($"[EmpleadoBL] Empleado actualizado. Id={entity.IdEmpleado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[EmpleadoBL] Validación al actualizar empleado: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Falla al actualizar empleado. Id={entity.IdEmpleado}", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Empleado entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();
                LoggerLogic.Info($"[EmpleadoBL] Empleado eliminado. Id={entity.IdEmpleado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[EmpleadoBL] Validación al eliminar empleado: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[EmpleadoBL] Falla al eliminar empleado. Id={entity.IdEmpleado}", ex);
                throw;
            }
        }

        // ===== Lecturas (sin transacción ni log) =====

        public List<DomainModel.Empleado> GetAll() => _repo.GetAll();

        public DomainModel.Empleado GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");
            return _repo.GetById(id);
        }
    }
}
