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
using DAL.DAL_Interfaces;

// Aliases EF para evitar choque de nombres con DomainModel.*
using DetMatEf = DAL.Detalle_proyecto_material;
using MatEf = DAL.Material;

namespace DAL.ProjectRepo
{
    // Los métodos de escritura NO llaman a SaveChanges: la persistencia la dispara el UnitOfWork al Commit.
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
        }

        public int Delete(Guid idProyecto, Guid idMaterial)
        {
            var row = _set.FirstOrDefault(d => d.idProyecto == idProyecto && d.idMaterial == idMaterial);
            if (row == null) return 0;

            int cantidad = row.cantidad;
            _set.Remove(row);
            return cantidad;
        }

        // ¿Hay algún detalle de proyecto que use este material? (chequeo de FK previo a borrar el material)
        public bool ExistsByMaterial(Guid idMaterial)
            => _set.AsNoTracking().Any(d => d.idMaterial == idMaterial);
    }
}
