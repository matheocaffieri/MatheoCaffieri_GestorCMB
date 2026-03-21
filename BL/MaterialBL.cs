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

            LoggerLogic.Info($"[MaterialBL] Add START. MaterialId={entity.IdMaterial} Desc='{entity.DescripcionArticulo}' Prov={entity.IdProveedor}");

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

                LoggerLogic.Info($"[MaterialBL] Add OK. MaterialId={entity.IdMaterial} InvId={inventario.IdMaterialInventario}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Add ERROR. MaterialId={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public void Update(DomainModel.Material entity)
        {
            Validate(entity, isUpdate: true);

            LoggerLogic.Info($"[MaterialBL] Update START. MaterialId={entity.IdMaterial}");

            _uow.Begin();
            try
            {
                _materialRepository.Update(entity);
                _uow.Commit();

                LoggerLogic.Info($"[MaterialBL] Update OK. MaterialId={entity.IdMaterial}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Update ERROR. MaterialId={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Material entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdMaterial == Guid.Empty)
                throw new AppException("err_material_id_delete");

            LoggerLogic.Info($"[MaterialBL] Delete START. MaterialId={entity.IdMaterial}");

            _uow.Begin();
            try
            {
                // OJO: si tu DB tiene FK Inventario->Material, primero deberías borrar inventario.
                // Como no tenemos método DeleteByMaterialId en repo, dejamos “mínimo cambio”:
                // 1) intentamos borrar inventario si existe
                var inv = _inventarioRepository.GetByMaterialId(entity.IdMaterial);
                if (inv != null)
                    _inventarioRepository.Delete(inv);

                // 2) borramos material
                _materialRepository.Delete(entity);

                _uow.Commit();

                LoggerLogic.Info($"[MaterialBL] Delete OK. MaterialId={entity.IdMaterial}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialBL] Delete ERROR. MaterialId={entity?.IdMaterial}", ex);
                throw;
            }
        }

        public DomainModel.Material GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");

            try
            {
                var mat = _materialRepository.GetById(id);
                if (mat == null) LoggerLogic.Warn($"[MaterialBL] GetById: no encontrado. MaterialId={id}");
                return mat;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialBL] GetById ERROR. MaterialId={id}", ex);
                throw;
            }
        }

        public List<DomainModel.Material> GetAll()
        {
            try
            {
                var list = _materialRepository.GetAll();
                LoggerLogic.Info($"[MaterialBL] GetAll OK. Count={list.Count}");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[MaterialBL] GetAll ERROR.", ex);
                throw;
            }
        }

        // ===== Implementación explícita de IGenericRepository =====
        List<DomainModel.Material> IGenericRepository<DomainModel.Material>.GetAll() => GetAll();
        DomainModel.Material IGenericRepository<DomainModel.Material>.GetById(Guid id) => GetById(id);
    }
}
