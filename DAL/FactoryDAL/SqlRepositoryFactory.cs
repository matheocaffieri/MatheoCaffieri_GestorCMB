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
            var cs = ConfigurationManager.ConnectionStrings[csName].ConnectionString;
            return new SqlUnitOfWork(cs);
        }

        public IRepoBundle CreateRepositories(IUnitOfWork uow)
            => new SqlRepoBundle(uow); // tu “bundle” que expone los repos
    }
}
