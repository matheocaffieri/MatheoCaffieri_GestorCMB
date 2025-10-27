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
        public IUnitOfWork CreateUnitOfWork(string csOrName)
        {
            if (string.IsNullOrWhiteSpace(csOrName))
                throw new ArgumentNullException(nameof(csOrName), "Debe indicar nombre o cadena de conexión.");

            // Si existe como nombre en config, lo resuelvo; si no, lo tomo como cadena directa
            var entry = ConfigurationManager.ConnectionStrings[csOrName];
            var cs = entry != null && !string.IsNullOrWhiteSpace(entry.ConnectionString)
                        ? entry.ConnectionString
                        : csOrName;

            return new SqlUnitOfWork(cs);
        }

        public IRepoBundle CreateRepositories(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            return new SqlRepoBundle(uow);
        }
    }
}
