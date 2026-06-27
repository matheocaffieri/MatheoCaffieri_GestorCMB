using DAL.ProjectRepo;
using DAL;
using DomainModel;
using DomainModel.Exceptions;
using DAL.DAL_Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Entities;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class ProyectoBL : IProyectoBL
    {
        public void Add(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Add(entity);
                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoBL] Proyecto agregado. Id={entity.IdProyecto} Desc='{entity.Descripcion}'");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoBL] Validación al agregar proyecto: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoBL] Falla al agregar proyecto. Id={entity?.IdProyecto}", ex);
                    throw;
                }
            }
        }

        public void Update(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Update(entity);
                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoBL] Proyecto actualizado. Id={entity.IdProyecto}");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoBL] Validación al actualizar proyecto: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoBL] Falla al actualizar proyecto. Id={entity?.IdProyecto}", ex);
                    throw;
                }
            }
        }

        public void Delete(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new AppException("err_entity_null");

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                uow.Begin();
                try
                {
                    var repo = new ProyectoRepository(uow);
                    repo.Delete(entity);
                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoBL] Proyecto eliminado. Id={entity.IdProyecto}");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoBL] Validación al eliminar proyecto: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoBL] Falla al eliminar proyecto. Id={entity?.IdProyecto}", ex);
                    throw;
                }
            }
        }

        public List<DomainModel.Proyecto> GetAll()
        {
            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                var repo = new ProyectoRepository(uow);
                return repo.GetAll();
            }
        }

        public DomainModel.Proyecto GetById(Guid id)
        {
            if (id == Guid.Empty) throw new AppException("err_id_required");

            using (var ctx = new DAL.GestorCMBEntities())
            using (var uow = new DAL.FactoryDAL.SqlUnitOfWork(ctx))
            {
                var repo = new ProyectoRepository(uow);
                return repo.GetById(id);
            }
        }
    }
}
