using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class DetalleEmpleadoBL : IDetalleEmpleadosRepository, IDetalleEmpleadoBL
    {
        private readonly IDetalleEmpleadosRepository _repo;
        private readonly IUnitOfWork _uow;

        public DetalleEmpleadoBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _repo = new DetalleEmpleadosRepository(_uow);
        }

        // DI / tests
        public DetalleEmpleadoBL(IUnitOfWork uow, IDetalleEmpleadosRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // ===== Lecturas (sin transacción) =====

        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            return _repo.GetAll(idProyecto);
        }

        public bool Exists(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");
            return _repo.Exists(idProyecto, idEmpleado);
        }

        public DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");
            return _repo.GetByProyectoEmpleado(idProyecto, idEmpleado);
        }

        // ===== Writes (con Begin/Commit/Rollback) =====

        // Importante: el parámetro `estado` es varchar en BD — "1" = activo, "0" = inactivo.
        public void Add(DetalleProyectoEmpleado detalle, string estado = "1")
        {
            if (detalle == null) throw new AppException("err_entity_null");
            if (detalle.IdProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (detalle.IdEmpleado == Guid.Empty) throw new AppException("err_empleado_id_required");

            _uow.Begin();
            try
            {
                _repo.Add(detalle, estado);
                _uow.Commit();
                LoggerLogic.Info($"[DetalleEmpleadoBL] Detalle agregado. idDetalle={detalle.IdDetalleProyectoEmpleado} Proy={detalle.IdProyecto} Emp={detalle.IdEmpleado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[DetalleEmpleadoBL] Validación al agregar detalle: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] Falla al agregar detalle. Proy={detalle.IdProyecto} Emp={detalle.IdEmpleado}", ex);
                throw;
            }
        }

        public void Update(DetalleProyectoEmpleado detalle)
        {
            if (detalle == null) throw new AppException("err_entity_null");
            if (detalle.IdDetalleProyectoEmpleado == Guid.Empty)
                throw new AppException("err_empleado_id_detalle_required");

            _uow.Begin();
            try
            {
                _repo.Update(detalle);
                _uow.Commit();
                LoggerLogic.Info($"[DetalleEmpleadoBL] Detalle actualizado. idDetalle={detalle.IdDetalleProyectoEmpleado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[DetalleEmpleadoBL] Validación al actualizar detalle: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] Falla al actualizar detalle. Id={detalle.IdDetalleProyectoEmpleado}", ex);
                throw;
            }
        }

        public void SetEstado(Guid idDetalleEmpleado, string estado)
        {
            if (idDetalleEmpleado == Guid.Empty)
                throw new AppException("err_empleado_id_detalle_required");

            _uow.Begin();
            try
            {
                _repo.SetEstado(idDetalleEmpleado, estado);
                _uow.Commit();
                LoggerLogic.Info($"[DetalleEmpleadoBL] Detalle estado actualizado. idDetalle={idDetalleEmpleado} estado={estado}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[DetalleEmpleadoBL] Validación al cambiar estado de detalle: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] Falla al cambiar estado de detalle. Id={idDetalleEmpleado}", ex);
                throw;
            }
        }
    }
}
