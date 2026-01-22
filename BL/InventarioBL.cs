using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
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
            if (inv == null) throw new ArgumentNullException(nameof(inv));
            if (isUpdate && inv.IdMaterialInventario == Guid.Empty)
                throw new ArgumentException("IdMaterialInventario requerido para actualizar.");
            if (inv.IdMaterial == Guid.Empty)
                throw new ArgumentException("IdMaterial requerido.");
            if (inv.Cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");
        }

        // ========= CRUD (WRITE => Begin/Commit) =========

        public void Add(DomainModel.Inventario entity)
        {
            Validate(entity);

            if (entity.IdMaterialInventario == Guid.Empty)
                entity.IdMaterialInventario = Guid.NewGuid();

            LoggerLogic.Info($"[InventarioBL] Add START. InvId={entity.IdMaterialInventario} Material={entity.IdMaterial} Cant={entity.Cantidad}");

            _uow.Begin();
            try
            {
                _repo.Add(entity);      // pendiente commit
                _uow.Commit();

                LoggerLogic.Info($"[InventarioBL] Add OK. InvId={entity.IdMaterialInventario}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Add ERROR. InvId={entity.IdMaterialInventario}. {ex.Message}");
                throw;
            }
        }

        public void Update(DomainModel.Inventario entity)
        {
            Validate(entity, isUpdate: true);

            LoggerLogic.Info($"[InventarioBL] Update START. InvId={entity.IdMaterialInventario} Cant={entity.Cantidad}");

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();

                LoggerLogic.Info($"[InventarioBL] Update OK. InvId={entity.IdMaterialInventario}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Update ERROR. InvId={entity.IdMaterialInventario}. {ex.Message}");
                throw;
            }
        }

        public void Delete(DomainModel.Inventario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdMaterialInventario == Guid.Empty)
                throw new ArgumentException("IdMaterialInventario requerido para eliminar.");

            LoggerLogic.Info($"[InventarioBL] Delete START. InvId={entity.IdMaterialInventario}");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();

                LoggerLogic.Info($"[InventarioBL] Delete OK. InvId={entity.IdMaterialInventario}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] Delete ERROR. InvId={entity.IdMaterialInventario}. {ex.Message}");
                throw;
            }
        }

        // ========= READ (sin transacción) =========

        public DomainModel.Inventario GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id requerido.", nameof(id));

            try
            {
                var inv = _repo.GetById(id);
                if (inv == null) LoggerLogic.Warn($"[InventarioBL] GetById: no encontrado. InvId={id}");
                return inv;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] GetById ERROR. InvId={id}. {ex.Message}");
                throw;
            }
        }

        public List<DomainModel.Inventario> GetAll()
        {
            try
            {
                var list = _repo.GetAll();
                LoggerLogic.Info($"[InventarioBL] GetAll OK. Count={list.Count}");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] GetAll ERROR. {ex.Message}");
                throw;
            }
        }

        public DomainModel.Inventario GetByMaterialId(Guid idMaterial)
        {
            if (idMaterial == Guid.Empty) throw new ArgumentException("idMaterial requerido.", nameof(idMaterial));

            try
            {
                var inv = _repo.GetByMaterialId(idMaterial);
                if (inv == null) LoggerLogic.Warn($"[InventarioBL] GetByMaterialId: sin inventario. Material={idMaterial}");
                return inv;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] GetByMaterialId ERROR. Material={idMaterial}. {ex.Message}");
                throw;
            }
        }

        public decimal GetCantidad(Guid idMaterial)
        {
            if (idMaterial == Guid.Empty) throw new ArgumentException("idMaterial requerido.", nameof(idMaterial));

            try
            {
                return _repo.GetCantidad(idMaterial);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] GetCantidad ERROR. Material={idMaterial}. {ex.Message}");
                throw;
            }
        }

        // ========= Extra: cambia cantidad (WRITE => Begin/Commit) =========

        public int CambiarCantidad(Guid idInventario, int delta)
        {
            if (idInventario == Guid.Empty) throw new ArgumentException("idInventario requerido.", nameof(idInventario));
            if (delta == 0) return GetById(idInventario)?.Cantidad ?? 0;

            LoggerLogic.Info($"[InventarioBL] CambiarCantidad START. InvId={idInventario} Delta={delta}");

            _uow.Begin();
            try
            {
                var inv = _repo.GetById(idInventario);
                if (inv == null) throw new ArgumentException("Inventario no encontrado.", nameof(idInventario));

                var nuevaCantidad = inv.Cantidad + delta;
                if (nuevaCantidad < 0)
                {
                    LoggerLogic.Warn($"[InventarioBL] CambiarCantidad: queda negativa, no aplica. InvId={idInventario} Actual={inv.Cantidad} Delta={delta}");
                    _uow.Rollback();
                    return inv.Cantidad;
                }

                inv.Cantidad = nuevaCantidad;
                _repo.Update(inv);

                _uow.Commit();
                LoggerLogic.Info($"[InventarioBL] CambiarCantidad OK. InvId={idInventario} NuevaCantidad={nuevaCantidad}");
                return nuevaCantidad;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[InventarioBL] CambiarCantidad ERROR. InvId={idInventario}. {ex.Message}");
                throw;
            }
        }

        // ========= IGenericRepository explícito =========
        List<DomainModel.Inventario> IGenericRepository<DomainModel.Inventario>.GetAll() => GetAll();
        DomainModel.Inventario IGenericRepository<DomainModel.Inventario>.GetById(Guid id) => GetById(id);
    }
}

