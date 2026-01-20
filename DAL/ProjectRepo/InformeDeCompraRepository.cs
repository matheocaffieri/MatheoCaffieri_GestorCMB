using DAL.FactoryDAL;
using DomainModel;
using DomainModel.Entities;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InformeCompraEf = DAL.Informe_compra;


namespace DAL.ProjectRepo
{
    public class InformeDeCompraRepository : IGenericRepository<InformeDeCompra>, IInformeDeCompraRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<InformeCompraEf> _set;

        // EF -> Dominio (SQL-friendly)
        // IMPORTANTE: No proyectar "Proyecto" acá.
        private static readonly Expression<Func<InformeCompraEf, DomainModel.InformeDeCompra>> ToDomainExpr =
            x => new DomainModel.InformeDeCompra
            {
                IdInformeCompra = x.idInformeCompra,
                IdProyecto = x.idProyecto,
                FechaRealizacion = x.fechaRealizacion
                // Proyecto NO
            };

        public InformeDeCompraRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _uow = uow;

            var sqlUow = (SqlUnitOfWork)uow;
            var sqlConn = (SqlConnection)sqlUow.Connection;

            using (var tmp = new GestorCMBEntities(
                       new EntityConnection("name=GestorCMBEntities"),
                       contextOwnsConnection: true))
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;
                var entityConn = new EntityConnection(workspace, sqlConn);
                _context = new GestorCMBEntities(entityConn, contextOwnsConnection: false);
            }

            if (sqlUow.Transaction != null)
                _context.Database.UseTransaction((DbTransaction)sqlUow.Transaction);

            _set = _context.Set<InformeCompraEf>();
        }

        private static void MapToEf(DomainModel.InformeDeCompra src, InformeCompraEf dst)
        {
            dst.idInformeCompra = src.IdInformeCompra;
            dst.idProyecto = src.IdProyecto;
            dst.fechaRealizacion = src.FechaRealizacion;
        }

        public void Add(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new InformeCompraEf();
            MapToEf(entity, ef);

            _set.Add(ef);
            _context.SaveChanges();
        }

        public void Update(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) throw new InvalidOperationException("Informe de compra no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(InformeDeCompra entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdInformeCompra);
            if (ef == null) return;

            _set.Remove(ef);
            _context.SaveChanges();
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
                       .OrderByDescending(x => x.fechaRealizacion)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public List<InformeDeCompra> GetByProyecto(Guid idProyecto)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idProyecto == idProyecto)
                       .OrderByDescending(x => x.fechaRealizacion)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public bool ExistsForProyectoOnDate(Guid idProyecto, DateTime fecha)
        {
            var d = fecha.Date;

            return _set.AsNoTracking()
                       .Any(x => x.idProyecto == idProyecto &&
                                 DbFunctions.TruncateTime(x.fechaRealizacion) == d);
        }
    }
}
