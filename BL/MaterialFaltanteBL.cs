using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using DAL.DAL_Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class MaterialFaltanteBL : IMaterialFaltanteBL
    {
        private readonly IUnitOfWork _uow;
        private readonly IMaterialesFaltantesRepository _repo;

        public MaterialFaltanteBL()
        {
            _uow = DalFactory.CreateUnitOfWork();
            _repo = DalFactory.CreateMaterialFaltanteRepository(_uow);
        }

        // DI / tests
        public MaterialFaltanteBL(IUnitOfWork uow, IMaterialesFaltantesRepository repo)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void AddOrUpdate(Guid idProyecto, string descripcion, string tipoMaterial, string tipoUnidad, int cantidad)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (string.IsNullOrWhiteSpace(descripcion)) throw new AppException("err_descripcion_required");
            if (string.IsNullOrWhiteSpace(tipoMaterial)) throw new AppException("err_tipo_material_required");
            if (string.IsNullOrWhiteSpace(tipoUnidad)) throw new AppException("err_tipo_unidad_required");
            if (cantidad <= 0) return;

            // Normalizamos espacios para que "Cable" y "Cable " no se traten como faltantes distintos.
            descripcion = descripcion.Trim();
            tipoMaterial = tipoMaterial.Trim();
            tipoUnidad = tipoUnidad.Trim();

            _uow.Begin();
            try
            {
                _repo.AddOrUpdate(idProyecto, descripcion, tipoMaterial, tipoUnidad, cantidad);
                _uow.Commit();
                LoggerLogic.Info($"[MaterialFaltanteBL] Material faltante agregado/actualizado. Proy={idProyecto} Desc='{descripcion}' Cant={cantidad}");
            }
            catch (AppException ex)
            {
                _uow.Rollback();
                LoggerLogic.Warn($"[MaterialFaltanteBL] Validación al agregar/actualizar faltante: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                LoggerLogic.Error($"[MaterialFaltanteBL] Falla al agregar/actualizar faltante. Proy={idProyecto} Desc='{descripcion}'", ex);
                throw;
            }
        }

        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            return _repo.GetAll(idProyecto);
        }
    }
}
