using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public class SqlLoginUnitOfWork : ILoginUnitOfWork
    {
        private readonly SqlConnection _cn;
        private SqlTransaction _tx;

        // ✅ Match exacto
        public DbConnection Connection => _cn;
        public DbTransaction Transaction => _tx;

        public SqlLoginUnitOfWork(string connectionStringOrName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringOrName))
                throw new ArgumentNullException(nameof(connectionStringOrName));

            var cs = TryResolveConnectionString(connectionStringOrName);
            _cn = new SqlConnection(cs);
        }

        private static string TryResolveConnectionString(string csOrName)
        {
            try
            {
                var entry = ConfigurationManager.ConnectionStrings[csOrName];
                if (entry != null && !string.IsNullOrWhiteSpace(entry.ConnectionString))
                    return entry.ConnectionString;
            }
            catch { }

            return csOrName; // si no es nombre, lo tratamos como CS directa
        }

        public void Begin()
        {
            if (_cn.State != ConnectionState.Open)
                _cn.Open();

            if (_tx == null)
                _tx = _cn.BeginTransaction();
        }

        public void Commit()
        {
            if (_tx == null) return;

            _tx.Commit();
            _tx.Dispose();
            _tx = null;
        }

        public void Rollback()
        {
            if (_tx == null) return;

            _tx.Rollback();
            _tx.Dispose();
            _tx = null;
        }

        public void Dispose()
        {
            try
            {
                if (_tx != null)
                {
                    try { _tx.Rollback(); } catch { /* swallow */ }
                    _tx.Dispose();
                    _tx = null;
                }
            }
            finally
            {
                if (_cn.State != ConnectionState.Closed)
                    _cn.Close();

                _cn.Dispose();
            }
        }
    }
}
