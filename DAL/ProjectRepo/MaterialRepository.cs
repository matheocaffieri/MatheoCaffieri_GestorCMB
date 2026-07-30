using DAL.FactoryDAL;
using DomainModel;
using DAL.DAL_Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
// Aliases EF para evitar choque de nombres con DomainModel.*
using MaterialEf = DAL.Material;
using ProveedorEf = DAL.Proveedor;

namespace DAL.ProjectRepo
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<MaterialEf> _set;

        // Proyección EF -> Dominio (100% traducible a SQL)
        private static readonly Expression<Func<MaterialEf, DomainModel.Material>> ToDomainExpr =
            m => new DomainModel.Material
            {
                IdMaterial = m.idMaterial,
                DescripcionArticulo = m.descripcionArticulo,
                TipoMaterial = m.tipoMaterial,
                TipoUnidad = m.tipoUnidad,
                CostoPorUnidad = (float)m.costoPorUnidad,
                IdProveedor = m.idProveedor,

                Proveedor = new DomainModel.Proveedor
                {
                    IdProveedor = m.Proveedor.idProveedor,
                    Descripcion = m.Proveedor.descripcion,
                    Telefono = m.Proveedor.telefono
                }
            };

        public MaterialRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<MaterialEf>();
        }

        // ===== Map Dominio -> EF (para altas/ediciones) =====
        private static void MapToEf(DomainModel.Material src, MaterialEf dst)
        {
            dst.idMaterial = src.IdMaterial;
            dst.descripcionArticulo = src.DescripcionArticulo;
            dst.tipoMaterial = src.TipoMaterial;
            dst.tipoUnidad = src.TipoUnidad;
            dst.costoPorUnidad = src.CostoPorUnidad;
            // Link por FK: no tocamos la propiedad de navegación Proveedor para no arrastrar el grafo.
            dst.idProveedor = src.IdProveedor;
        }

        // ===== CRUD =====

        public void Add(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new MaterialEf();
            MapToEf(entity, ef);

            _set.Add(ef);

        }


        public void Update(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterial);
            if (ef == null) throw new InvalidOperationException("Material no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
        }

        public void Delete(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterial);
            if (ef == null) return;

            _set.Remove(ef);
        }

        public DomainModel.Material GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(m => m.idMaterial == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<DomainModel.Material> GetAll()
        {
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public Guid FindIdByDescripcionTipoUnidad(string descripcion, string tipoMaterial, string tipoUnidad)
        {
            var descLow = (descripcion ?? "").Trim().ToLowerInvariant();
            var tipoLow = (tipoMaterial ?? "").Trim().ToLowerInvariant();
            var unidLow = (tipoUnidad ?? "").Trim().ToLowerInvariant();

            // Prefiltro por igualdad exacta en SQL; el match final (trim + case-insensitive)
            // se resuelve en memoria porque SQL no colapsa espacios/mayúsculas igual que acá.
            return _set.AsNoTracking()
                       .Where(m => m.descripcionArticulo == descripcion)
                       .AsEnumerable()
                       .Where(m => (m.descripcionArticulo ?? "").Trim().ToLowerInvariant() == descLow
                                && (m.tipoMaterial ?? "").Trim().ToLowerInvariant() == tipoLow
                                && (m.tipoUnidad ?? "").Trim().ToLowerInvariant() == unidLow)
                       .Select(m => m.idMaterial)
                       .FirstOrDefault();
        }
    }
}
