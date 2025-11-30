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
    public class ProveedorBL : IProveedorRepository, IGenericRepository<DomainModel.Proveedor>
    {
        private readonly IUnitOfWork _uow;
        private readonly IProveedorRepository _repo;

        // Ctor por defecto: arma UoW desde el connection string del EDMX
        public ProveedorBL()
        {
            var ctx = new DAL.GestorCMBEntities();
            _uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx.Database.Connection.ConnectionString);
            _repo = new ProveedorRepository(_uow);
        }

        // Ctor para DI / tests
        public ProveedorBL(IUnitOfWork uow, IProveedorRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // --------- Validaciones ----------
        private static void Validate(DomainModel.Proveedor p, bool isUpdate = false)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (isUpdate && p.IdProveedor == Guid.Empty)
                throw new ArgumentException("IdProveedor requerido para actualizar.");
            if (string.IsNullOrWhiteSpace(p.Descripcion))
                throw new ArgumentException("La descripción (nombre del proveedor) es obligatoria.");
            // Telefono/CUIT/etc.: agregá reglas si las definís como obligatorias
        }

        // ================= CRUD =================

        public void Add(DomainModel.Proveedor entity)
        {
            Validate(entity, isUpdate: false);

            try
            {
                if (entity.IdProveedor == Guid.Empty)
                    entity.IdProveedor = Guid.NewGuid();

                _repo.Add(entity);
                LoggerLogic.Info($"[ProveedorBL] Proveedor agregado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ProveedorBL] Error al agregar proveedor ({entity?.Descripcion ?? "desconocido"})", ex);
                throw;
            }
        }

        public void Update(DomainModel.Proveedor entity)
        {
            Validate(entity, isUpdate: true);

            try
            {
                _repo.Update(entity);
                LoggerLogic.Info($"[ProveedorBL] Proveedor actualizado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ProveedorBL] Error al actualizar proveedor ({entity?.IdProveedor})", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Proveedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdProveedor == Guid.Empty)
                throw new ArgumentException("IdProveedor requerido para eliminar.");

            try
            {
                _repo.Delete(entity);
                LoggerLogic.Info($"[ProveedorBL] Proveedor eliminado: {entity.Descripcion} ({entity.IdProveedor})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ProveedorBL] Error al eliminar proveedor ({entity?.IdProveedor})", ex);
                throw;
            }
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
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
                LoggerLogic.Info($"[ProveedorBL] Listado de proveedores: {list.Count} filas.");
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
