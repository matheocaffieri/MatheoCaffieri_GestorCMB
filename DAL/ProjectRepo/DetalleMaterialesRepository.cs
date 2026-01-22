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
        private readonly GestorCMBEntities _context;
        private readonly DbSet<DetMatEf> _set;

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
            _context = uow.Context;
            _set = _context.Set<DetMatEf>();
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(d => d.idProyecto == idProyecto)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public void AddOrUpdate(Guid idProyecto, Guid idMaterial, int cantidad, double valorGanancia, DateTime fechaIngreso)
        {
            if (cantidad <= 0) return;

            var row = _set.FirstOrDefault(d => d.idProyecto == idProyecto && d.idMaterial == idMaterial);

            if (row == null)
            {
                var nuevo = new DetMatEf
                {
                    idDetalleMaterial = Guid.NewGuid(),
                    idProyecto = idProyecto,
                    idMaterial = idMaterial,
                    cantidad = cantidad,
                    valorGanancia = valorGanancia,
                    fechaIngresoMaterial = fechaIngreso
                };

                _set.Add(nuevo);
            }
            else
            {
                row.cantidad += cantidad;
                row.valorGanancia = valorGanancia;
                row.fechaIngresoMaterial = fechaIngreso;
                _context.Entry(row).State = EntityState.Modified;
            }

            // NO SaveChanges (lo hace el UoW)
        }
    }
}
