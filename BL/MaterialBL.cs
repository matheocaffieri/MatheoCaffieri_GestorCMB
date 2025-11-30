using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using Services.Logs.Strategy;
using Services.Logs;

namespace BL
{
    public class MaterialBL : IMaterialRepository
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _uow;

        public MaterialBL()
        {
            var ctx = new DAL.GestorCMBEntities();
            _uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx.Database.Connection.ConnectionString);
            _materialRepository = new MaterialRepository(_uow);
        }

        public MaterialBL(IUnitOfWork uow, IMaterialRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _materialRepository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // ===== Validación común =====
        private static void Validate(Material m, bool isUpdate = false)
        {
            if (m == null) throw new ArgumentNullException(nameof(m));
            if (isUpdate && m.IdMaterial == Guid.Empty)
                throw new ArgumentException("IdMaterial requerido para actualizar.");
            if (string.IsNullOrWhiteSpace(m.DescripcionArticulo))
                throw new ArgumentException("La descripción del artículo es obligatoria.");
            if (string.IsNullOrWhiteSpace(m.TipoUnidad))
                throw new ArgumentException("La unidad es obligatoria.");
            if (m.CostoPorUnidad < 0)
                throw new ArgumentException("El costo por unidad no puede ser negativo.");
        }

        // ===========================================================
        //                       MÉTODOS CRUD
        // ===========================================================

        public void Add(Material entity)
        {
            Validate(entity);

            try
            {
                if (entity.IdMaterial == Guid.Empty)
                    entity.IdMaterial = Guid.NewGuid();

                _materialRepository.Add(entity);   // <-- SOLO ESTO

                LoggerLogic.Info($"[MaterialBL] Nuevo material agregado: {entity.DescripcionArticulo} ({entity.IdMaterial})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialBL] Error al agregar material ({entity?.DescripcionArticulo ?? "desconocido"})", ex);
                throw;
            }
        }


        public void Update(Material entity)
        {
            Validate(entity, isUpdate: true);

            try
            {
                _materialRepository.Update(entity);
                LoggerLogic.Info($"[MaterialBL] Material actualizado: {entity.DescripcionArticulo} ({entity.IdMaterial})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialBL] Error al actualizar material ({entity?.IdMaterial})", ex);
                throw;
            }
        }

        public void Delete(Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.IdMaterial == Guid.Empty)
                throw new ArgumentException("IdMaterial requerido para eliminar.");

            try
            {
                _materialRepository.Delete(entity);
                LoggerLogic.Info($"[MaterialBL] Material eliminado: {entity.DescripcionArticulo} ({entity.IdMaterial})");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialBL] Error al eliminar material ({entity?.IdMaterial})", ex);
                throw;
            }
        }

        public Material GetById(Guid id)
        {
            try
            {
                var mat = _materialRepository.GetById(id);
                if (mat == null)
                    LoggerLogic.Warn($"[MaterialBL] Material no encontrado con ID: {id}");

                return mat;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialBL] Error al obtener material ({id})", ex);
                throw;
            }
        }

        public List<Material> GetAll()
        {
            try
            {
                var list = _materialRepository.GetAll();
                LoggerLogic.Info($"[MaterialBL] Se recuperaron {list.Count} materiales.");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[MaterialBL] Error al obtener listado de materiales", ex);
                throw;
            }
        }

        // ===== Implementación explícita de IGenericRepository =====
        List<Material> IGenericRepository<Material>.GetAll() => GetAll();
        Material IGenericRepository<Material>.GetById(Guid id) => GetById(id);

    }
}
