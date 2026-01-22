using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public class LoginUnitOfWork : ILoginUnitOfWork
    {
        private readonly string _connectionString;
        private SqlConnection _cn;
        private SqlTransaction _tx;

        public LoginUnitOfWork(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DbConnection Connection => _cn;
        public DbTransaction Transaction => _tx;

        public void Begin()
        {
            if (_cn == null)
                _cn = new SqlConnection(_connectionString);

            if (_cn.State != System.Data.ConnectionState.Open)
                _cn.Open();

            if (_tx == null)
                _tx = _cn.BeginTransaction();
        }

        public void Commit()
        {
            _tx?.Commit();
            _tx?.Dispose();
            _tx = null;
        }

        public void Rollback()
        {
            try { _tx?.Rollback(); } catch { }
            _tx?.Dispose();
            _tx = null;
        }

        public void Dispose()
        {
            try { _tx?.Dispose(); } catch { }
            _tx = null;

            try { _cn?.Dispose(); } catch { }
            _cn = null;
        }
    }
}
