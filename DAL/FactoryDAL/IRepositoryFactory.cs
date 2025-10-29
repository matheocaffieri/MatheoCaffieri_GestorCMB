using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.FactoryDAL
{
    public interface IRepositoryFactory
    {
        IUnitOfWork CreateUnitOfWork(string csName);
        IRepoBundle CreateRepositories(IUnitOfWork uow);
    }
}
