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
            // NO SaveChanges
        }

        public void Update(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) throw new InvalidOperationException("Informe de compra no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            // NO SaveChanges
        }

        public void Delete(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) return;

            _set.Remove(ef);
            // NO SaveChanges
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
    }
}
