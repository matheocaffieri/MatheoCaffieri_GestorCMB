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
    public class InventarioBL : IInventarioRepository, IGenericRepository<Inventario>
    {
        private readonly IUnitOfWork _uow;
        private readonly IInventarioRepository _inventarioRepository;

        // Ctor por defecto: arma UoW desde el connection string del EDMX
        public InventarioBL()
        {
            var ctx = new DAL.GestorCMBEntities();
            _uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx.Database.Connection.ConnectionString);
            _inventarioRepository = new InventarioRepository(_uow);
        }

        // Ctor para DI / tests
        public InventarioBL(IUnitOfWork uow, IInventarioRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _inventarioRepository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // ---------- Validaciones ----------
        private static void Validate(Inventario inv, bool isUpdate = false)
        {
            if (inv == null) throw new ArgumentNullException(nameof(inv));
            if (isUpdate && inv.IdMaterialInventario == Guid.Empty)
                throw new ArgumentException("IdMaterialInventario requerido para actualizar.");
            if (inv.IdMaterial == Guid.Empty)
                throw new ArgumentException("IdMaterial requerido.");
            if (inv.Cantidad < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");
        }

        // ============= CRUD =============

        public void Add(Inventario entity)
        {
            Validate(entity);

            try
            {
                if (entity.IdMaterialInventario == Guid.Empty)
                    entity.IdMaterialInventario = Guid.NewGuid();

                _inventarioRepository.Add(entity);
                LoggerLogic.Info($"[InventarioBL] Alta inventario ok. Material={entity.IdMaterial} InvId={entity.IdMaterialInventario} Cant={entity.Cantidad}");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error al dar de alta inventario (Material={entity?.IdMaterial})", ex);
                throw;
            }
        }

        public void Update(Inventario entity)
        {
            Validate(entity, isUpdate: true);

            try
            {
                _inventarioRepository.Update(entity);
                LoggerLogic.Info($"[InventarioBL] Inventario actualizado. InvId={entity.IdMaterialInventario} Cant={entity.Cantidad}");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error al actualizar inventario (InvId={entity?.IdMaterialInventario})", ex);
                throw;
            }
        }

        public void Delete(Inventario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdMaterialInventario == Guid.Empty)
                throw new ArgumentException("IdMaterialInventario requerido para eliminar.");

            try
            {
                _inventarioRepository.Delete(entity);
                LoggerLogic.Info($"[InventarioBL] Inventario eliminado. InvId={entity.IdMaterialInventario}");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error al eliminar inventario (InvId={entity?.IdMaterialInventario})", ex);
                throw;
            }
        }

        public Inventario GetById(Guid id)
        {
            try
            {
                var inv = _inventarioRepository.GetById(id);
                if (inv == null) LoggerLogic.Warn($"[InventarioBL] Inventario no encontrado. InvId={id}");
                return inv;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error al obtener inventario (InvId={id})", ex);
                throw;
            }
        }

        public List<Inventario> GetAll()
        {
            try
            {
                var list = _inventarioRepository.GetAll();
                LoggerLogic.Info($"[InventarioBL] Listado inventario: {list.Count} filas.");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[InventarioBL] Error al obtener listado de inventario", ex);
                throw;
            }
        }

        // ============= Extras típicos del repo (si tu interfaz los define) =============

        public Inventario GetByMaterialId(Guid idMaterial)
        {
            try
            {
                var inv = _inventarioRepository.GetByMaterialId(idMaterial);
                if (inv == null) LoggerLogic.Warn($"[InventarioBL] Sin inventario para Material={idMaterial}");
                return inv;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error en GetByMaterialId (Material={idMaterial})", ex);
                throw;
            }
        }

        public decimal GetCantidad(Guid idMaterial)
        {
            try
            {
                return _inventarioRepository.GetCantidad(idMaterial);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioBL] Error en GetCantidad (Material={idMaterial})", ex);
                throw;
            }
        }

        // ============= Implementación explícita de IGenericRepository =============
        List<Inventario> IGenericRepository<Inventario>.GetAll() => GetAll();
        Inventario IGenericRepository<Inventario>.GetById(Guid id) => GetById(id);
    }
}
