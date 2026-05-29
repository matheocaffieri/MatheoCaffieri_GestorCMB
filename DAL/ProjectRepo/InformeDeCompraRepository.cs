using DAL.FactoryDAL;
using DomainModel;
using DomainModel.Entities;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using InformeCompraEf = DAL.Informe_compra;


namespace DAL.ProjectRepo
{
    // Los métodos de escritura NO llaman a SaveChanges: la persistencia la dispara el UnitOfWork al Commit.
    public class InformeDeCompraRepository : IGenericRepository<InformeDeCompra>, IInformeDeCompraRepository
    {
        private readonly GestorCMBEntities _context;
        private readonly DbSet<InformeCompraEf> _set;

        private static readonly Expression<Func<InformeCompraEf, DomainModel.InformeDeCompra>> ToDomainExpr =
            x => new DomainModel.InformeDeCompra
            {
                IdInformeCompra = x.idInformeCompra,
                IdProyecto = x.idProyecto,
                FechaRealizacion = x.fechaRealizacion,
                Estado = x.estado
            };

        public InformeDeCompraRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<InformeCompraEf>();
        }

        private static void MapToEf(DomainModel.InformeDeCompra src, InformeCompraEf dst)
        {
            dst.idInformeCompra = src.IdInformeCompra;
            dst.idProyecto = src.IdProyecto;
            dst.fechaRealizacion = src.FechaRealizacion;
            dst.estado = src.Estado;
        }

        public void Add(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new InformeCompraEf();
            MapToEf(entity, ef);

            _set.Add(ef);
        }

        public void Update(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) throw new InvalidOperationException("Informe de compra no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
        }

        public void Delete(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) return;

            _set.Remove(ef);
        }

        public InformeDeCompra GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idInformeCompra == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<InformeDeCompra> GetAll()
        {
            return _set.AsNoTracking()
                       .Where(x => x.estado == "pendiente")
                       .OrderByDescending(x => x.fechaRealizacion)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public List<InformeDeCompra> GetHistorial()
        {
            return _set.AsNoTracking()
                       .Where(x => x.estado == "cancelado" || x.estado == "finalizado")
                       .OrderByDescending(x => x.fechaRealizacion)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public List<InformeDeCompra> GetByProyecto(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idProyecto == idProyecto && x.estado == "pendiente")
                       .OrderByDescending(x => x.fechaRealizacion)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public bool ExistsForProyectoOnDate(Guid idProyecto, DateTime fecha)
        {
            var d = fecha.Date;

            return _set.AsNoTracking()
                       .Any(x => x.idProyecto == idProyecto &&
                                 x.estado == "pendiente" &&
                                 DbFunctions.TruncateTime(x.fechaRealizacion) == d);
        }

        public HashSet<Guid> GetMaterialesConInformesPendientes()
        {
            var faltantes = (from d in _context.Detalle_informe_material_faltante.AsNoTracking()
                             join i in _context.Informe_compra.AsNoTracking() on d.idInformeCompra equals i.idInformeCompra
                             join f in _context.Material_faltante.AsNoTracking() on d.idMaterialFaltante equals f.idMaterialFaltante
                             where i.estado == "pendiente"
                             select new
                             {
                                 f.descripcionArticuloFaltante,
                                 f.tipoMaterialFaltante,
                                 f.tipoUnidadMaterialFaltante
                             })
                            .Distinct()
                            .ToList();

            if (faltantes.Count == 0) return new HashSet<Guid>();

            var descripciones = faltantes
                .Select(f => f.descripcionArticuloFaltante)
                .Where(d => d != null)
                .Distinct()
                .ToList();

            var candidatos = _context.Material.AsNoTracking()
                .Where(m => descripciones.Contains(m.descripcionArticulo))
                .Select(m => new { m.idMaterial, m.descripcionArticulo, m.tipoMaterial, m.tipoUnidad })
                .ToList();

            var faltantesSet = new HashSet<string>(
                faltantes.Select(f => NormKey(f.descripcionArticuloFaltante, f.tipoMaterialFaltante, f.tipoUnidadMaterialFaltante))
            );

            var result = new HashSet<Guid>();
            foreach (var m in candidatos)
            {
                if (faltantesSet.Contains(NormKey(m.descripcionArticulo, m.tipoMaterial, m.tipoUnidad)))
                    result.Add(m.idMaterial);
            }
            return result;
        }

        private static string NormKey(string desc, string tipo, string unidad)
            => (desc ?? "").Trim().ToLowerInvariant() + "|" +
               (tipo ?? "").Trim().ToLowerInvariant() + "|" +
               (unidad ?? "").Trim().ToLowerInvariant();
    }
}
