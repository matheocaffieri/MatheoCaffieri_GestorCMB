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

namespace BL
{
    public class ProveedorBL : IProveedorRepository, IGenericRepository<DomainModel.Proveedor>, IDisposable
    {
        private readonly IUnitOfWork _uow;
        private readonly IProveedorRepository _repo;

        public ProveedorBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);
            _repo = new ProveedorRepository(_uow);
        }

        // Ctor para DI / tests
        public ProveedorBL(IUnitOfWork uow, IProveedorRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }

        // --------- Validaciones ----------
        private static void Validate(DomainModel.Proveedor p, bool isUpdate = false)
        {
            if (p == null) throw new AppException("err_entity_null");
            if (isUpdate && p.IdProveedor == Guid.Empty)
                throw new AppException("err_proveedor_id_required");
            if (string.IsNullOrWhiteSpace(p.Descripcion))
                throw new AppException("err_proveedor_descripcion_required");
        }

        // ================= CRUD =================

        public void Add(DomainModel.Proveedor entity)
        {
            Validate(entity);

            if (entity.IdProveedor == Guid.Empty)
                entity.IdProveedor = Guid.NewGuid();

            _uow.Begin();
            try
            {
                _repo.Add(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ProveedorBL] Proveedor agregado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ProveedorBL] Validación al agregar proveedor: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Falla al agregar proveedor ({entity?.Descripcion ?? "desconocido"})", ex);
                throw;
            }
        }

        public void Update(DomainModel.Proveedor entity)
        {
            Validate(entity, isUpdate: true);

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ProveedorBL] Proveedor actualizado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ProveedorBL] Validación al actualizar proveedor: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Falla al actualizar proveedor ({entity?.IdProveedor})", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Proveedor entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdProveedor == Guid.Empty)
                throw new AppException("err_proveedor_id_delete");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ProveedorBL] Proveedor eliminado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ProveedorBL] Validación al eliminar proveedor: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Falla al eliminar proveedor ({entity?.IdProveedor})", ex);
                throw;
            }
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");
            return _repo.GetById(id);
        }

        public List<DomainModel.Proveedor> GetAll() => _repo.GetAll();

        // ===== Implementación explícita de IGenericRepository =====
        List<DomainModel.Proveedor> IGenericRepository<DomainModel.Proveedor>.GetAll() => GetAll();
        DomainModel.Proveedor IGenericRepository<DomainModel.Proveedor>.GetById(Guid id) => GetById(id);
    }

}
