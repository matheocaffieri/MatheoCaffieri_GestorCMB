using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DAL.DAL_Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;

using BL.BL_Interfaces;

namespace BL
{
    public class ClienteBL : IClienteBL
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public ClienteBL()
        {
            _uow = DalFactory.CreateUnitOfWork();

            _repo = DalFactory.CreateClienteRepository(_uow);
        }

        // DI / tests
        public ClienteBL(IUnitOfWork uow, IClienteRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Add(DomainModel.Cliente entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            _uow.Begin();
            try
            {
                _repo.Add(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ClienteBL] Cliente agregado. Id={entity.IdCliente}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ClienteBL] Validación al agregar cliente: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Falla al agregar cliente. Id={entity.IdCliente}", ex);
                throw;
            }
        }

        public void Update(DomainModel.Cliente entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            _uow.Begin();
            try
            {
                _repo.Update(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ClienteBL] Cliente actualizado. Id={entity.IdCliente}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ClienteBL] Validación al actualizar cliente: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Falla al actualizar cliente. Id={entity.IdCliente}", ex);
                throw;
            }
        }

        public void Delete(DomainModel.Cliente entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            _uow.Begin();
            try
            {
                _repo.Delete(entity);
                _uow.Commit();
                LoggerLogic.Info($"[ClienteBL] Cliente eliminado. Id={entity.IdCliente}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[ClienteBL] Validación al eliminar cliente: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[ClienteBL] Falla al eliminar cliente. Id={entity.IdCliente}", ex);
                throw;
            }
        }

        public List<DomainModel.Cliente> GetAll() => _repo.GetAll();

        public DomainModel.Cliente GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");
            return _repo.GetById(id);
        }
    }
}
