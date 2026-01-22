using DAL.LoginDAL;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public class SqlRepositoryFactory : IRepositoryFactory
    {
        public IUnitOfWork CreateUnitOfWork(string csName)
        {
            // si tu contexto usa "name=GestorCMBEntities", no necesitás csName
            var ctx = new GestorCMBEntities();
            return new SqlUnitOfWork(ctx);
        }


        public IRepoBundle CreateRepositories(IUnitOfWork uow)
        {
            // Tomamos el mismo connectionString que usa EF (siempre desde config)
            var cs = ConfigurationManager
                .ConnectionStrings["GestorCMBEntities"]
                .ConnectionString;

            // UoW del login (ADO.NET)
            ILoginUnitOfWork uowLogin = new LoginUnitOfWork(cs);

            return new SqlRepoBundle(uow, uowLogin);
        }

    }
}
