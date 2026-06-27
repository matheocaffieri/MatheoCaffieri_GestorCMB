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
    public class DetalleMaterialBL : IDetalleMaterialesRepository, IDetalleMaterialBL
    {
        private readonly IDetalleMaterialesRepository _repo;
        private readonly IUnitOfWork _uow;

        public DetalleMaterialBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _repo = new DetalleMaterialesRepository(_uow);
        }

        // DI / tests
        public DetalleMaterialBL(IUnitOfWork uow, IDetalleMaterialesRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");
            if (cantidad <= 0) return;

            _uow.Begin();
            try
            {
                _repo.AddOrUpdate(idProyecto, idMaterial, cantidad, valorGanancia, fechaIngreso);
                _uow.Commit();
                LoggerLogic.Info($"[DetalleMaterialBL] Detalle material agregado/actualizado. Proy={idProyecto} Mat={idMaterial} Cant={cantidad}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[DetalleMaterialBL] Validación al agregar/actualizar detalle: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleMaterialBL] Falla al agregar/actualizar detalle. Proy={idProyecto} Mat={idMaterial}", ex);
                throw;
            }
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            return _repo.GetAll(idProyecto);
        }

        public int Delete(Guid idProyecto, Guid idMaterial)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");

            _uow.Begin();
            try
            {
                int cantidad = _repo.Delete(idProyecto, idMaterial);
                _uow.Commit();
                LoggerLogic.Info($"[DetalleMaterialBL] Detalle material eliminado. Proy={idProyecto} Mat={idMaterial} CantDevuelta={cantidad}");
                return cantidad;
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[DetalleMaterialBL] Validación al eliminar detalle: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[DetalleMaterialBL] Falla al eliminar detalle. Proy={idProyecto} Mat={idMaterial}", ex);
                throw;
            }
        }
    }
}
