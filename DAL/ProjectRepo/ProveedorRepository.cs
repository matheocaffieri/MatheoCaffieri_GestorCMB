using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using DAL.FactoryDAL;
using System.Linq;
using System.Linq.Expressions;

using DomainModel;
using DomainModel.Interfaces;

// Alias a la entidad EF (ajustá namespace si difiere)
using ProveedorEf = DAL.Proveedor;

namespace DAL.ProjectRepo
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly GestorCMBEntities _context;
        private readonly DbSet<ProveedorEf> _set;

        // EF -> Dominio (traducible a SQL)
        private static readonly Expression<Func<ProveedorEf, DomainModel.Proveedor>> ToDomainExpr =
            p => new DomainModel.Proveedor
            {
                IdProveedor = p.idProveedor,
                Descripcion = p.descripcion,
                Telefono = p.telefono,
                IsActive = p.isActive
            };

        public ProveedorRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _context = uow.Context;
            _set = _context.Set<ProveedorEf>();
        }

        private static void MapToEf(DomainModel.Proveedor src, ProveedorEf dst)
        {
            dst.idProveedor = src.IdProveedor;
            dst.descripcion = src.Descripcion;
            dst.telefono = src.Telefono;
            dst.isActive = src.IsActive;
        }

        public void Add(DomainModel.Proveedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = new ProveedorEf();
            MapToEf(entity, ef);
            _set.Add(ef);
            // NO SaveChanges (lo hace el UoW)
        }

        public void Update(DomainModel.Proveedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdProveedor);
            if (ef == null) throw new InvalidOperationException("Proveedor no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
            // NO SaveChanges
        }

        public void Delete(DomainModel.Proveedor entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var ef = _set.Find(entity.IdProveedor);
            if (ef == null) return;

            _set.Remove(ef);
            // NO SaveChanges
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
            return _set.AsNoTracking()
                       .Where(p => p.idProveedor == id)
                       .Select(ToDomainExpr)
                       .FirstOrDefault();
        }

        public List<DomainModel.Proveedor> GetAll()
        {
            return _set.AsNoTracking()
                       .Select(ToDomainExpr)
                       .ToList();
        }
    }
}
