using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        private bool _disposed;

        public SqlUnitOfWork(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Begin()
        {
            if (Transaction == null)
                Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            Transaction?.Dispose();
            Connection?.Dispose();
            _disposed = true;
        }
    }
}
