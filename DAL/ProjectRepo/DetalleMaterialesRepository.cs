using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using DAL.FactoryDAL;

using DomainModel;
using DomainModel.Interfaces;

// Aliases EF (ajusta namespaces si difieren)
using DetMatEf = DAL.Detalle_proyecto_material;
using MatEf = DAL.Material;

namespace DAL.ProjectRepo
{
    public class DetalleMaterialesRepository : IDetalleMaterialesRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<DetMatEf> _set;

        // Proyección EF -> Dominio (100% traducible a SQL)
        private static readonly Expression<Func<DetMatEf, DomainModel.DetalleProyectoMaterial>> ToDomainExpr =
            d => new DomainModel.DetalleProyectoMaterial
            {
                IdDetalleMaterial = d.idDetalleMaterial,
                IdProyecto = d.idProyecto,
                IdMaterial = d.idMaterial,
                Cantidad = d.cantidad,
                ValorGanancia = (float)d.valorGanancia,
                FechaIngresoMaterial = d.fechaIngresoMaterial,

                Material = new DomainModel.Material
                {
                    IdMaterial = d.Material.idMaterial,
                    DescripcionArticulo = d.Material.descripcionArticulo,
                    TipoMaterial = d.Material.tipoMaterial,
                    TipoUnidad = d.Material.tipoUnidad,
                    CostoPorUnidad = (float)d.Material.costoPorUnidad,
                    IdProveedor = d.Material.idProveedor
                }
            };

        public DetalleMaterialesRepository(IUnitOfWork uow)
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
                var ws = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;
                var econn = new EntityConnection(ws, sqlConn);
                _context = new GestorCMBEntities(econn, contextOwnsConnection: false);
            }

            // Compartir transacción del UoW si existe
            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<DetMatEf>();
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
