using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MaterialFaltanteBL : IMaterialesFaltantesRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMaterialesFaltantesRepository _repo;

        public MaterialFaltanteBL()
        {
            var ctx = new GestorCMBEntities();
            _uow = new SqlUnitOfWork(ctx);
            _repo = new MaterialFaltanteRepository(_uow);
        }

        // opcional DI/tests
        public MaterialFaltanteBL(IUnitOfWork uow, IMaterialesFaltantesRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("descripcion requerida.", nameof(descripcion));
            if (string.IsNullOrWhiteSpace(tipoMaterial)) throw new ArgumentException("tipoMaterial requerido.", nameof(tipoMaterial));
            if (string.IsNullOrWhiteSpace(tipoUnidad)) throw new ArgumentException("tipoUnidad requerido.", nameof(tipoUnidad));
            if (cantidad <= 0) return;

            // normalización "mínimo cambio" (evita duplicados por espacios/caso)
            descripcion = descripcion.Trim();
            tipoMaterial = tipoMaterial.Trim();
            tipoUnidad = tipoUnidad.Trim();

            LoggerLogic.Info($"[MaterialFaltanteBL] AddOrUpdate START. Proy={idProyecto} Desc='{descripcion}' Tipo='{tipoMaterial}' Unidad='{tipoUnidad}' Cant={cantidad}");

            _uow.Begin();
            try
            {
                _repo.AddOrUpdate(idProyecto, descripcion, tipoMaterial, tipoUnidad, cantidad);

                _uow.Commit();
                LoggerLogic.Info($"[MaterialFaltanteBL] AddOrUpdate OK. Proy={idProyecto} Desc='{descripcion}'");
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialFaltanteBL] AddOrUpdate ERROR. Proy={idProyecto} Desc='{descripcion}'", ex);
                throw;
            }
        }

        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));

            try
            {
                var list = _repo.GetAll(idProyecto);
                LoggerLogic.Info($"[MaterialFaltanteBL] GetAll OK. Proy={idProyecto} Count={list.Count}");
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[MaterialFaltanteBL] GetAll ERROR. Proy={idProyecto}", ex);
                throw;
            }
        }
    }
}
