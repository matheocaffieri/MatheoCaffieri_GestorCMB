using System;
using System.Data.Common;

namespace Services.Services_Interfaces
{
    public interface ILoginUnitOfWork : IDisposable
    {
        DbConnection Connection { get; }
        DbTransaction Transaction { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
