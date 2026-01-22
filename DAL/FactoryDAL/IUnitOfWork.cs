using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DAL.ProjectRepo;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public interface IUnitOfWork : IDisposable
    {

        GestorCMBEntities Context { get; }

        void Begin();         // abre conn + comienza trans
        void Commit();        // commit trans
        void Rollback();      // rollback trans
    }
}
