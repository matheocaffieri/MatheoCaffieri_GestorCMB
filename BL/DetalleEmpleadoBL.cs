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
    public class DetalleEmpleadoBL : IDetalleEmpleadosRepository
    {
        private readonly IDetalleEmpleadosRepository _repo;
        private readonly IUnitOfWork _uow;

        public DetalleEmpleadoBL()
        {
            var ctx = new GestorCMBEntities(); // usa config
            _uow = new SqlUnitOfWork(ctx);     // ctor nuevo: recibe Context

            _repo = new DetalleEmpleadosRepository(_uow);
        }

        // (Opcional) ctor para DI/tests
        public DetalleEmpleadoBL(IUnitOfWork uow, IDetalleEmpleadosRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // ===== Lecturas (sin transacción) =====

        public List<DetalleProyectoEmpleado> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            try
            {
                return _repo.GetAll(idProyecto);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleEmpleadoBL] GetAll ERROR. idProyecto={idProyecto}. {ex.Message}");
                throw;
            }
        }

        public bool Exists(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idEmpleado == Guid.Empty) throw new ArgumentException("idEmpleado requerido.", nameof(idEmpleado));

            try
            {
                return _repo.Exists(idProyecto, idEmpleado);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleEmpleadoBL] Exists ERROR. idProyecto={idProyecto}, idEmpleado={idEmpleado}. {ex.Message}");
                throw;
            }
        }

        public DetalleProyectoEmpleado GetByProyectoEmpleado(Guid idProyecto, Guid idEmpleado)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idEmpleado == Guid.Empty) throw new ArgumentException("idEmpleado requerido.", nameof(idEmpleado));

            try
            {
                return _repo.GetByProyectoEmpleado(idProyecto, idEmpleado);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleEmpleadoBL] GetByProyectoEmpleado ERROR. idProyecto={idProyecto}, idEmpleado={idEmpleado}. {ex.Message}");
                throw;
            }
        }

        // ===== Writes (con Begin/Commit/Rollback) =====

        // estado varchar: "1" activo, "0" inactivo
        public void Add(DetalleProyectoEmpleado detalle, string estado = "1")
        {
            if (detalle == null) throw new ArgumentNullException(nameof(detalle));
            if (detalle.IdProyecto == Guid.Empty) throw new ArgumentException("IdProyecto requerido.", nameof(detalle));
            if (detalle.IdEmpleado == Guid.Empty) throw new ArgumentException("IdEmpleado requerido.", nameof(detalle));

            LoggerLogic.Info($"[DetalleEmpleadoBL] Add START. idProyecto={detalle.IdProyecto}, idEmpleado={detalle.IdEmpleado}");

            _uow.Begin();
            try
            {
                _repo.Add(detalle, estado); // queda pendiente en context
                _uow.Commit();              // SaveChanges + commit trans

                LoggerLogic.Info($"[DetalleEmpleadoBL] Add OK. idDetalle={detalle.IdDetalleProyectoEmpleado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] Add ERROR. idProyecto={detalle.IdProyecto}, idEmpleado={detalle.IdEmpleado}. {ex.Message}");
                throw;
            }
        }

        public void Update(DetalleProyectoEmpleado detalle)
        {
            if (detalle == null) throw new ArgumentNullException(nameof(detalle));
            if (detalle.IdDetalleProyectoEmpleado == Guid.Empty)
                throw new ArgumentException("IdDetalleProyectoEmpleado requerido.", nameof(detalle));

            LoggerLogic.Info($"[DetalleEmpleadoBL] Update START. idDetalle={detalle.IdDetalleProyectoEmpleado}");

            _uow.Begin();
            try
            {
                _repo.Update(detalle);
                _uow.Commit();

                LoggerLogic.Info($"[DetalleEmpleadoBL] Update OK. idDetalle={detalle.IdDetalleProyectoEmpleado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] Update ERROR. idDetalle={detalle.IdDetalleProyectoEmpleado}. {ex.Message}");
                throw;
            }
        }

        public void SetEstado(Guid idDetalleEmpleado, string estado)
        {
            if (idDetalleEmpleado == Guid.Empty)
                throw new ArgumentException("idDetalleEmpleado requerido.", nameof(idDetalleEmpleado));

            LoggerLogic.Info($"[DetalleEmpleadoBL] SetEstado START. idDetalle={idDetalleEmpleado}, estado={estado}");

            _uow.Begin();
            try
            {
                _repo.SetEstado(idDetalleEmpleado, estado);
                _uow.Commit();

                LoggerLogic.Info($"[DetalleEmpleadoBL] SetEstado OK. idDetalle={idDetalleEmpleado}, estado={estado}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleEmpleadoBL] SetEstado ERROR. idDetalle={idDetalleEmpleado}, estado={estado}. {ex.Message}");
                throw;
            }
        }
    }
}
