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

// Aliases a entidades EF (ajusta namespaces si difieren)
using InventarioEf = DAL.Inventario;
using MaterialEf = DAL.Material;
using ProveedorEf = DAL.Proveedor;

namespace DAL.ProjectRepo
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<InventarioEf> _set;

        // ===== Proyección EF -> Dominio (100% traducible a SQL) =====
        private static readonly Expression<Func<InventarioEf, DomainModel.Inventario>> ToDomainExpr =
            inv => new DomainModel.Inventario
            {
                IdMaterialInventario = inv.idMaterialInventario,
                Cantidad = inv.cantidad,
                IdMaterial = inv.idMaterial,

                Material = new DomainModel.Material
                {
                    IdMaterial = inv.Material.idMaterial,
                    DescripcionArticulo = inv.Material.descripcionArticulo,
                    TipoMaterial = inv.Material.tipoMaterial,
                    TipoUnidad = inv.Material.tipoUnidad,
                    CostoPorUnidad = (float)inv.Material.costoPorUnidad, // decimal -> float
                    IdProveedor = inv.Material.idProveedor,

                    Proveedor = new DomainModel.Proveedor
                    {
                        IdProveedor = inv.Material.Proveedor.idProveedor,
                        Descripcion = inv.Material.Proveedor.descripcion,
                        Telefono = inv.Material.Proveedor.telefono
                    }
                }
            };

        public InventarioRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _uow = uow;

            var sqlUow = (DAL.FactoryDAL.SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            // Contexto temporal para obtener el MetadataWorkspace del EDMX
            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;

                // EntityConnection que reutiliza la MISMA SqlConnection del UoW
                var entityConn = new EntityConnection(workspace, sqlConn);

                // Instancia del contexto real (no es dueño de la conexión)
                _context = new GestorCMBEntities(entityConn, contextOwnsConnection: false);
            }

            // Si el UoW ya tiene transacción, la compartimos
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<InventarioEf>();
        }

        // ===== Map Dominio -> EF (para altas/ediciones) =====
        private static void MapToEf(DomainModel.Inventario src, InventarioEf dst)
        {
            // Claves / campos propios de Inventario
            dst.idMaterialInventario = src.IdMaterialInventario;
            dst.cantidad = src.Cantidad;
            dst.idMaterial = src.IdMaterial;

            // Nota: NO tocamos Material/Proveedor acá.
            // Si querés permitir actualizar Material/Proveedor desde acá,
            // deberíamos mapear entidades relacionadas y sus estados por separado.
        }

        // ===== CRUD =====

        public void Add(DomainModel.Inventario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new InventarioEf();
            MapToEf(entity, ef);

            // Adjuntar solo la FK del material (evitamos traer todo el material)
            // Si EF ya conoce Material por id, basta con setear idMaterial como arriba.
            _set.Add(ef);
            _context.SaveChanges();
        }

        public void Update(DomainModel.Inventario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterialInventario);
            if (ef == null) throw new InvalidOperationException("Inventario no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(DomainModel.Inventario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdMaterialInventario);
            if (ef == null) return;

            _set.Remove(ef);
            _context.SaveChanges();
        }

        public DomainModel.Inventario GetById(Guid id)
        {
            // Incluimos Material y Proveedor SOLO para que la proyección tenga los campos.
            // Como proyectamos con Expression, EF genera un SELECT con los JOINs necesarios.
            return _set.AsNoTracking()
                       .Where(inv => inv.idMaterialInventario == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<DomainModel.Inventario> GetAll()
        {
            // Igual que en EmpleadoRepository: proyección pura (traducible a SQL).
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
