using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;

namespace BL
{
    /// <summary>
    /// Capa de negocio para Inventario: valida, loguea y delega en el repositorio.
    /// </summary>
    public class InventarioBL : IInventarioRepository, IGenericRepository<DomainModel.Inventario>
    {
        private readonly IUnitOfWork _uow;
        private readonly IInventarioRepository _repo;

        public InventarioBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);
            _repo = new InventarioRepository(_uow);
        }

        // DI / tests
        public InventarioBL(IUnitOfWork uow, IInventarioRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private static void Validate(DomainModel.Inventario inv, bool isUpdate = false)
        {
            if (inv == null) throw new AppException("err_entity_null");
            if (isUpdate && inv.IdMaterialInventario == Guid.Empty)
                throw new AppException("err_inventario_id_required");
            if (inv.IdMaterial == Guid.Empty)
                throw new AppException("err_inventario_material_required");
            if (inv.Cantidad < 0)
                throw new AppException("err_inventario_cantidad_negativa");
        }

        // ========= CRUD (WRITE => Begin/Commit) =========

        public void Add(DomainModel.Inventario entity)
        {
            Validate(entity);

            if (entity.IdMaterialInventario == Guid.Empty)
                entity.IdMaterialInventario = Guid.NewGuid();

            _uow.Begin();
            try
            {
                _repo.Add(entity);
                _uow.Commit();
                LoggerLogic.Info($"[InventarioBL] Inventario creado. Id={entity.IdMaterialInventario} Material={entity.IdMaterial}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[InventarioBL] Validación al crear inventario: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Falla al crear inventario. Id={entity.IdMaterialInventario}", ex);
                throw;
            }
        }

        // No loguea: los cambios de cantidad de inventario están explícitamente excluidos del historial de eventos.
        public void Update(DomainModel.Inventario entity)
        {
            Validate(entity, isUpdate: true);

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[InventarioBL] Validación al actualizar inventario: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Falla al actualizar inventario. Id={entity.IdMaterialInventario}", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Inventario entity)
        {
            if (entity == null) throw new AppException("err_entity_null");
            if (entity.IdMaterialInventario == Guid.Empty)
                throw new AppException("err_inventario_id_delete");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();
                LoggerLogic.Info($"[InventarioBL] Inventario eliminado. Id={entity.IdMaterialInventario}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[InventarioBL] Validación al eliminar inventario: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Falla al eliminar inventario. Id={entity.IdMaterialInventario}", ex);
                throw;
            }
        }

        // ========= READ (sin transacción ni log) =========

        public DomainModel.Inventario GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");
            return _repo.GetById(id);
        }

        public List<DomainModel.Inventario> GetAll() => _repo.GetAll();

        public DomainModel.Inventario GetByMaterialId(Guid idMaterial)
        {
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");
            return _repo.GetByMaterialId(idMaterial);
        }

        public decimal GetCantidad(Guid idMaterial)
        {
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");
            return _repo.GetCantidad(idMaterial);
        }

        // ========= Extra: cambia cantidad (WRITE => Begin/Commit, sin log de éxito por ser cambio de stock) =========

        public int CambiarCantidad(Guid idInventario, int delta)
        {
            if (idInventario == Guid.Empty) throw new AppException("err_id_required");
            if (delta == 0) return GetById(idInventario)?.Cantidad ?? 0;

            _uow.Begin();
            try
            {
                var inv = _repo.GetById(idInventario);
                if (inv == null) throw new AppException("err_inventario_not_found");

                var nuevaCantidad = inv.Cantidad + delta;
                if (nuevaCantidad < 0)
                {
                    _uow.Rollback();
                    return inv.Cantidad;
                }

                inv.Cantidad = nuevaCantidad;
                _repo.Update(inv);

                _uow.Commit();
                return nuevaCantidad;
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[InventarioBL] Validación al cambiar cantidad de inventario: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Falla al cambiar cantidad de inventario. Id={idInventario}", ex);
                throw;
            }
        }

        // ========= IGenericRepository explícito =========
        List<DomainModel.Inventario> IGenericRepository<DomainModel.Inventario>.GetAll() => GetAll();
        DomainModel.Inventario IGenericRepository<DomainModel.Inventario>.GetById(Guid id) => GetById(id);
    }
}

