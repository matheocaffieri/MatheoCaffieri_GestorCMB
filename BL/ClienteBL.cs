using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;

namespace BL
{
    public class ClienteBL : IClienteRepository
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        // Opción 1: constructor por defecto (como venías usando)
        public ClienteBL()
        {
            var ctx = new GestorCMBEntities();   // usa config (name=GestorCMBEntities)
            _uow = new SqlUnitOfWork(ctx);       // ahora recibe Context

            _repo = new ClienteRepository(_uow);
        }

        // Opción 2: inyección (útil para tests o si armás factories)
        public ClienteBL(IUnitOfWork uow, IClienteRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Add(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            LoggerLogic.Info($"[ClienteBL] Add START. Id={entity.IdCliente}");

            _uow.Begin();
            try
            {
                _repo.Add(entity);   // queda pendiente en el context
                _uow.Commit();       // hace SaveChanges + Commit (según tu SqlUnitOfWork)

                LoggerLogic.Info($"[ClienteBL] Add OK. Id={entity.IdCliente}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Add ERROR. Id={entity.IdCliente}. {ex.Message}");
                throw;
            }
        }

        public void Update(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            LoggerLogic.Info($"[ClienteBL] Update START. Id={entity.IdCliente}");

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();

                LoggerLogic.Info($"[ClienteBL] Update OK. Id={entity.IdCliente}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Update ERROR. Id={entity.IdCliente}. {ex.Message}");
                throw;
            }
        }

        public void Delete(DomainModel.Cliente entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            LoggerLogic.Info($"[ClienteBL] Delete START. Id={entity.IdCliente}");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();

                LoggerLogic.Info($"[ClienteBL] Delete OK. Id={entity.IdCliente}");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Delete ERROR. Id={entity.IdCliente}. {ex.Message}");
                throw;
            }
        }

        public List<DomainModel.Cliente> GetAll()
        {
            // Lectura: no hace falta Begin/Commit
            try
            {
                return _repo.GetAll();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ClienteBL] GetAll ERROR. {ex.Message}");
                throw;
            }
        }

        public DomainModel.Cliente GetById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentException("id requerido.", nameof(id));

            try
            {
                return _repo.GetById(id);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ClienteBL] GetById ERROR. Id={id}. {ex.Message}");
                throw;
            }
        }
    }
}
