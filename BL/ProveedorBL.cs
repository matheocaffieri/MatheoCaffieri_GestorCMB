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
            _uow = new SqlUnitOfWork(ctx);          // ✅ NO connectionString
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
                LoggerLogic.Info($"[ProveedorBL] Proveedor agregado OK: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Error al agregar proveedor ({entity?.Descripcion ?? "desconocido"})", ex);
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
                LoggerLogic.Info($"[ProveedorBL] Proveedor actualizado OK: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Error al actualizar proveedor ({entity?.IdProveedor})", ex);
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
                LoggerLogic.Info($"[ProveedorBL] Proveedor eliminado OK: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ProveedorBL] Error al eliminar proveedor ({entity?.IdProveedor})", ex);
                throw;
            }
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");

            try
            {
                var prov = _repo.GetById(id);
                if (prov == null) LoggerLogic.Warn($"[ProveedorBL] Proveedor no encontrado. Id={id}");
                return prov;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ProveedorBL] Error en GetById (Id={id})", ex);
                throw;
            }
        }

        public List<DomainModel.Proveedor> GetAll()
        {
            try
            {
                var list = _repo.GetAll();
                LoggerLogic.Info($"[ProveedorBL] Listado de proveedores OK: {list.Count} filas.");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[ProveedorBL] Error al obtener listado de proveedores", ex);
                throw;
            }
        }

        // ===== Implementación explícita de IGenericRepository =====
        List<DomainModel.Proveedor> IGenericRepository<DomainModel.Proveedor>.GetAll() => GetAll();
        DomainModel.Proveedor IGenericRepository<DomainModel.Proveedor>.GetById(Guid id) => GetById(id);
    }

}
