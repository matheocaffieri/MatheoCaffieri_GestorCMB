using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Entity;                    // EF6
using System.Data.Entity.Infrastructure;     // IObjectContextAdapter
using System.Data.Entity.Core.EntityClient;  // EntityConnection
using DomainModel.Interfaces;

namespace DAL.FactoryDAL
{
    public sealed class SqlUnitOfWork : IUnitOfWork, IDisposable
    {
        public GestorCMBEntities Context { get; }
        private DbContextTransaction _tx;

        public SqlUnitOfWork(GestorCMBEntities context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Begin()
        {
            if (_tx != null) return;
            _tx = Context.Database.BeginTransaction();
        }

        public void Commit()
        {
            Context.SaveChanges();
            _tx?.Commit();
            _tx?.Dispose();
            _tx = null;
        }

        public void Rollback()
        {
            _tx?.Rollback();
            _tx?.Dispose();
            _tx = null;
        }

        public void Dispose()
        {
            _tx?.Dispose();
            Context?.Dispose();
        }
    }
}
