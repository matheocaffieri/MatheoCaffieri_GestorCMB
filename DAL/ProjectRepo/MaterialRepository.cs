using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

using DomainModel;
using DomainModel.Interfaces;

// Aliases a las entidades EF (ajusta namespaces si difieren)
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
                CostoPorUnidad = (float)m.costoPorUnidad, // DB: float -> dominio: float
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
            _uow = uow;

            var sqlUow = (DAL.FactoryDAL.SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            // Contexto temporal para obtener MetadataWorkspace del EDMX
            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;

                // EntityConnection que reutiliza la MISMA SqlConnection del UoW
                var entityConn = new EntityConnection(workspace, sqlConn);

                // Contexto real (no dueño de la conexión)
                _context = new GestorCMBEntities(entityConn, contextOwnsConnection: false);
            }

            // Compartimos transacción del UoW si existe
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<MaterialEf>();
        }

        // ===== Map Dominio -> EF (para altas/ediciones) =====
        private static void MapToEf(DomainModel.Material src, MaterialEf dst)
        {
            dst.idMaterial = src.IdMaterial;
            dst.descripcionArticulo = src.DescripcionArticulo;
            dst.tipoMaterial = src.TipoMaterial;
            dst.tipoUnidad = src.TipoUnidad;
            dst.costoPorUnidad = src.CostoPorUnidad; // ambos son float
            dst.idProveedor = src.IdProveedor;    // link por FK, sin tocar navegación
        }

        // ===== CRUD =====

        public void Add(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new MaterialEf();
            MapToEf(entity, ef);

            _set.Add(ef);
            _context.SaveChanges();
        }

        public void Update(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterial);
            if (ef == null) throw new InvalidOperationException("Material no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(DomainModel.Material entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterial);
            if (ef == null) return;

            _set.Remove(ef);
            _context.SaveChanges();
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
    }
}
