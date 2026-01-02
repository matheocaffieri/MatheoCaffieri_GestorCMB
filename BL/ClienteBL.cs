using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;

namespace BL
{
    public class ClienteBL : IClienteRepository
    {
        private readonly IClienteRepository clienteRepository;

        public ClienteBL()
        {
            var context = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(context.Database.Connection.ConnectionString);

            clienteRepository = new ClienteRepository(uow);
        }

        public void Add(DomainModel.Cliente entity)
        {
            clienteRepository.Add(entity);
        }

        public void Update(DomainModel.Cliente entity)
        {
            clienteRepository.Update(entity);
        }

        public void Delete(DomainModel.Cliente entity)
        {
            clienteRepository.Delete(entity);
        }

        public List<DomainModel.Cliente> GetAll()
        {
            return clienteRepository.GetAll();
        }

        public DomainModel.Cliente GetById(Guid id)
        {
            return clienteRepository.GetById(id);
        }
    }
}
