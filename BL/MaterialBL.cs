using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Logs;
using Services.Logs.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class MaterialBL : IMaterialRepository
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IInventarioRepository _inventarioRepository;
        private readonly IUnitOfWork _uow;

        public MaterialBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);

            _materialRepository = new MaterialRepository(_uow);
            _inventarioRepository = new InventarioRepository(_uow);
        }

        // ctor para DI/tests
        public MaterialBL(IUnitOfWork uow, IMaterialRepository repo, IInventarioRepository inventarioRepo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _materialRepository = repo ?? throw new ArgumentNullException(nameof(repo));
            _inventarioRepository = inventarioRepo ?? throw new ArgumentNullException(nameof(inventarioRepo));
        }

        private static void Validate(DomainModel.Material m, bool isUpdate = false)
        {
            if (m == null) throw new AppException("err_entity_null");
            if (isUpdate && m.IdMaterial == Guid.Empty)
                throw new AppException("err_material_id_required");
            if (string.IsNullOrWhiteSpace(m.DescripcionArticulo))
                throw new AppException("err_material_descripcion_required");
            if (string.IsNullOrWhiteSpace(m.TipoUnidad))
                throw new AppException("err_material_unidad_required");
            if (m.CostoPorUnidad < 0)
                throw new AppException("err_material_costo_negativo");
            if (m.IdProveedor == Guid.Empty)
                throw new AppException("err_material_proveedor_required");
        }

        // ================= CRUD =================

        public void Add(DomainModel.Material entity)
        {
            Validate(entity);

            if (entity.IdMaterial == Guid.Empty)
                entity.IdMaterial = Guid.NewGuid();

            _uow.Begin();
            try
            {
                // 1) Alta material
                _materialRepository.Add(entity);

                // 2) Crear inventario inicial (0)
                var inventario = new DomainModel.Inventario
                {
                    IdMaterialInventario = Guid.NewGuid(),
                    IdMaterial = entity.IdMaterial,
                    Cantidad = 0
                };
                _inventarioRepository.Add(inventario);

                _uow.Commit();
                LoggerLogic.Info($"[MaterialBL] Material agregado. Id={entity.IdMaterial} Desc='{entity.DescripcionArticulo}'");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[MaterialBL] Validación al agregar material: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Falla al agregar material. Id={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public void Update(DomainModel.Material entity)
        {
            Validate(entity, isUpdate: true);

            _uow.Begin();
            try
            {
                _materialRepository.Update(entity);
                _uow.Commit();
                LoggerLogic.Info($"[MaterialBL] Material actualizado. Id={entity.IdMaterial}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[MaterialBL] Validación al actualizar material: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Falla al actualizar material. Id={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Material entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdMaterial == Guid.Empty)
                throw new AppException("err_material_id_delete");

            // La FK de Detalle_proyecto_material bloquea el delete: avisar con un mensaje claro
            // en vez de dejar que explote el constraint en la DB.
            bool asociadoAProyecto = _uow.Context.Detalle_proyecto_material
                .Any(d => d.idMaterial == entity.IdMaterial);
            if (asociadoAProyecto)
            {
                LoggerLogic.Warn($"[MaterialBL] Intento de eliminar material asociado a un proyecto. Id={entity.IdMaterial}");
                throw new AppException("err_material_asociado_proyecto");
            }

            _uow.Begin();
            try
            {
                // El inventario tiene FK al material, así que primero se borra el inventario y después el material.
                // 1) Inventario asociado (si existe)
                var inv = _inventarioRepository.GetByMaterialId(entity.IdMaterial);
                if (inv != null)
                    _inventarioRepository.Delete(inv);

                // 2) Material
                _materialRepository.Delete(entity);

                _uow.Commit();
                LoggerLogic.Info($"[MaterialBL] Material eliminado. Id={entity.IdMaterial}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[MaterialBL] Validación al eliminar material: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Falla al eliminar material. Id={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public DomainModel.Material GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");
            return _materialRepository.GetById(id);
        }

        public List<DomainModel.Material> GetAll() => _materialRepository.GetAll();

        // ===== Implementación explícita de IGenericRepository =====
        List<DomainModel.Material> IGenericRepository<DomainModel.Material>.GetAll() => GetAll();
        DomainModel.Material IGenericRepository<DomainModel.Material>.GetById(Guid id) => GetById(id);
    }
}
