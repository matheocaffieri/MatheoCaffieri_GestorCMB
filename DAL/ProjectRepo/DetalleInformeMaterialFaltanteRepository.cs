using DAL.FactoryDAL;
using DomainModel;
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
using DetEf = DAL.Detalle_informe_material_faltante;
using DomainModel.Interfaces;
using DomainModel.Entities;



namespace DAL.ProjectRepo
{
    public class DetalleInformeMaterialFaltanteRepository : IDetalleInformeMaterialFaltanteRepository
    {
        private readonly GestorCMBEntities _context;
        private readonly DbSet<DetEf> _set;

        private static readonly Expression<Func<DetEf, DomainModel.DetalleInformeMaterialFaltante>> ToDomainExpr =
            x => new DomainModel.DetalleInformeMaterialFaltante
            {
                IdDetalleMaterialFaltante = x.idDetalleMaterialFaltante,
                IdInformeCompra = x.idInformeCompra,
                IdMaterialFaltante = x.idMaterialFaltante
            };

        public DetalleInformeMaterialFaltanteRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<DetEf>();
        }

        private static void MapToEf(DomainModel.DetalleInformeMaterialFaltante src, DetEf dst)
        {
            dst.idDetalleMaterialFaltante = src.IdDetalleMaterialFaltante;
            dst.idInformeCompra = src.IdInformeCompra;
            dst.idMaterialFaltante = src.IdMaterialFaltante;
        }

        public void Add(DetalleInformeMaterialFaltante entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new DetEf();
            MapToEf(entity, ef);

            _set.Add(ef);
            // NO SaveChanges
        }

        public void Update(DetalleInformeMaterialFaltante entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdDetalleMaterialFaltante);
            if (ef == null) throw new InvalidOperationException("Detalle no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            // NO SaveChanges
        }

        public void Delete(DetalleInformeMaterialFaltante entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdDetalleMaterialFaltante);
            if (ef == null) return;

            _set.Remove(ef);
            // NO SaveChanges
        }

        public DetalleInformeMaterialFaltante GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idDetalleMaterialFaltante == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<DetalleInformeMaterialFaltante> GetAll()
        {
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public List<DetalleInformeMaterialFaltante> GetByInforme(Guid idInformeCompra)
        {
            return _set.AsNoTracking()
                       .Where(x => x.idInformeCompra == idInformeCompra)
                       .Select(ToDomainExpr)
                       .ToList();
        }

        public bool Exists(Guid idInformeCompra, Guid idMaterialFaltante)
        {
            return _set.AsNoTracking()
                       .Any(x => x.idInformeCompra == idInformeCompra &&
                                 x.idMaterialFaltante == idMaterialFaltante);
        }
    }
}
