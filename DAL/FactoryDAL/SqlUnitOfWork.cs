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
        private readonly SqlConnection _conn;
        private SqlTransaction _tx;
        private DAL.GestorCMBEntities _ctx;
        private bool _disposed;

        public DbConnection Connection => _conn;
        public DbTransaction Transaction => _tx;
        public DAL.GestorCMBEntities Context => _ctx;

        public SqlUnitOfWork(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("connectionString vacío", nameof(connectionString));

            _conn = new SqlConnection(connectionString);
        }

        public void Begin()
        {
            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            // 1) Comenzar transacción explícita en la MISMA SqlConnection
            _tx = _conn.BeginTransaction(IsolationLevel.ReadCommitted);

            // 2) Obtener el MetadataWorkspace desde el ctor "name=GestorCMBEntities"
            using (var tmp = new DAL.GestorCMBEntities())
            {
                var workspace = ((IObjectContextAdapter)tmp).ObjectContext.MetadataWorkspace;

                // 3) Envolver la misma SqlConnection en un EntityConnection (EF6 Database-First)
                var econn = new EntityConnection(workspace, _conn);

                // 4) Crear el DbContext sobre esa EntityConnection SIN adueñarse de la conexión
                _ctx = new DAL.GestorCMBEntities(econn, contextOwnsConnection: false);
            }

            // 5) Asegurar conexión abierta en el contexto
            if (_ctx.Database.Connection.State != ConnectionState.Open)
                _ctx.Database.Connection.Open();

            // 6) Enlazar EF6 a la transacción existente
            _ctx.Database.UseTransaction(_tx);
        }

        public void Commit()
        {
            // Guardar primero en EF; si falla, cae en catch del llamador
            _ctx?.SaveChanges();

            // Confirmar transacción ADO.NET
            _tx?.Commit();
            CleanupTransactionOnly();
        }

        public void Rollback()
        {
            try
            {
                _tx?.Rollback();
            }
            finally
            {
                CleanupTransactionOnly();
            }
        }

        private void CleanupTransactionOnly()
        {
            _tx?.Dispose();
            _tx = null;

            // No cierres la _conn acá si querés reusar el UoW para otra Begin();
            // si preferís one-shot por UoW, podés cerrar acá y en Dispose().
        }

        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                // Si quedó una transacción abierta y no se hizo Commit, la revertimos
                if (_tx != null)
                {
                    try { _tx.Rollback(); } catch { /* best effort */ }
                    _tx.Dispose();
                    _tx = null;
                }

                _ctx?.Dispose();

                if (_conn.State != ConnectionState.Closed)
                    _conn.Close();

                _conn.Dispose();
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
