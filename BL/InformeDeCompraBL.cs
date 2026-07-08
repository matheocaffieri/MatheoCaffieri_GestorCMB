using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
using DomainModel.Exceptions;
using Services.Historial;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class InformeDeCompraBL : IInformeDeCompraBL
    {
        public Guid GenerarDesdeFaltantes(Guid idProyecto, bool unicoPorDia = true)
        {
            if (idProyecto == Guid.Empty)
                throw new AppException("err_proyecto_id_required");

            var uow = DalFactory.CreateUnitOfWork();

            uow.Begin();

            var informeRepo = DalFactory.CreateInformeDeCompraRepository(uow);
            var detalleRepo = DalFactory.CreateDetalleInformeMaterialFaltanteRepository(uow);

            var materialFaltanteRepo = DalFactory.CreateMaterialFaltanteRepository(uow);

            try
            {
                var faltantesIds = materialFaltanteRepo.GetIdsByProyecto(idProyecto);

                if (faltantesIds.Count == 0)
                    throw new AppException("err_informe_sin_faltantes");

                InformeDeCompra informe = null;

                if (unicoPorDia && informeRepo.ExistsForProyectoOnDate(idProyecto, DateTime.Today))
                {
                    informe = informeRepo.GetByProyecto(idProyecto)
                        .FirstOrDefault(x => x.FechaRealizacion.Date == DateTime.Today.Date);
                }

                if (informe == null)
                {
                    informe = new InformeDeCompra
                    {
                        IdInformeCompra = Guid.NewGuid(),
                        IdProyecto = idProyecto,
                        FechaRealizacion = DateTime.Today,
                        Estado = "pendiente"
                    };
                    informeRepo.Add(informe);
                }

                foreach (var idMatFal in faltantesIds)
                {
                    if (detalleRepo.Exists(informe.IdInformeCompra, idMatFal))
                        continue;

                    detalleRepo.Add(new DetalleInformeMaterialFaltante
                    {
                        IdDetalleMaterialFaltante = Guid.NewGuid(),
                        IdInformeCompra = informe.IdInformeCompra,
                        IdMaterialFaltante = idMatFal
                    });
                }

                uow.Commit();

                LoggerLogic.Info($"[InformeDeCompraBL] Informe de compra generado. Id={informe.IdInformeCompra} Proy={idProyecto}");
                return informe.IdInformeCompra;
            }
            catch (AppException ex)
            {
                uow.Rollback();
                LoggerLogic.Warn($"[InformeDeCompraBL] Validación al generar informe: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] Falla al generar informe. Proy={idProyecto}", ex);
                throw;
            }
            finally
            {
                uow.Dispose();
            }
        }

        public List<InformeDeCompra> GetAll()
        {
            var uow = DalFactory.CreateUnitOfWork();
            try
            {
                var repo = DalFactory.CreateInformeDeCompraRepository(uow);
                return repo.GetAll();
            }
            finally
            {
                uow.Dispose();
            }
        }

        public List<InformeDeCompra> GetHistorial()
        {
            var uow = DalFactory.CreateUnitOfWork();
            try
            {
                var repo = DalFactory.CreateInformeDeCompraRepository(uow);
                return repo.GetHistorial();
            }
            finally
            {
                uow.Dispose();
            }
        }

        public HashSet<Guid> GetMaterialesConInformesPendientes()
        {
            var uow = DalFactory.CreateUnitOfWork();
            try
            {
                var repo = DalFactory.CreateInformeDeCompraRepository(uow);
                return repo.GetMaterialesConInformesPendientes();
            }
            finally
            {
                uow.Dispose();
            }
        }

        public void EliminarInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

            var uow = DalFactory.CreateUnitOfWork();
            uow.Begin();

            var infRepo = DalFactory.CreateInformeDeCompraRepository(uow);

            try
            {
                var inf = infRepo.GetById(idInformeCompra);
                if (inf != null)
                {
                    inf.Estado = "cancelado";
                    infRepo.Update(inf);
                }

                uow.Commit();
                LoggerLogic.Info($"[InformeDeCompraBL] Informe cancelado. Id={idInformeCompra}");
            }
            catch (AppException ex)
            {
                uow.Rollback();
                LoggerLogic.Warn($"[InformeDeCompraBL] Validación al cancelar informe: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] Falla al cancelar informe. Id={idInformeCompra}", ex);
                throw;
            }
            finally
            {
                uow.Dispose();
            }
        }

        public void ConfirmarCompraYAplicar(Guid idProyecto, Guid idInformeCompra)
        {
            if (idProyecto == Guid.Empty)
                throw new AppException("err_proyecto_id_required");
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

            var uow = DalFactory.CreateUnitOfWork();

            uow.Begin();

            var detMatRepo = DalFactory.CreateDetalleMaterialesRepository(uow);
            var detInfRepo = DalFactory.CreateDetalleInformeMaterialFaltanteRepository(uow);
            var materialFaltanteRepo = DalFactory.CreateMaterialFaltanteRepository(uow);
            var materialRepo = DalFactory.CreateMaterialRepository(uow);
            var informeRepo = DalFactory.CreateInformeDeCompraRepository(uow);

            try
            {
                // 1) ids de Material_faltante incluidos en el informe
                var idsFaltantes = detInfRepo.GetByInforme(idInformeCompra)
                    .Select(d => d.IdMaterialFaltante)
                    .ToList();

                if (idsFaltantes.Count == 0)
                    throw new AppException("err_informe_sin_materiales");

                // 2) traer faltantes (solo del proyecto) que están en el informe
                var faltantes = materialFaltanteRepo.GetByIdsAndProyecto(idsFaltantes, idProyecto);

                if (faltantes.Count == 0)
                    throw new AppException("err_informe_sin_faltantes_proyecto");

                // 3) aplicar compra: sumar al detalle del proyecto
                foreach (var f in faltantes)
                {
                    // Match del material por descripción/tipo/unidad (case-insensitive + trim) lo resuelve el repo.
                    var idMaterial = materialRepo.FindIdByDescripcionTipoUnidad(
                        f.DescripcionArticuloFaltante, f.TipoMaterialFaltante, f.TipoUnidadMaterialFaltante);

                    if (idMaterial == Guid.Empty)
                    {
                        LoggerLogic.Warn($"[InformeDeCompraBL] Material no encontrado en inventario, se omite. Desc='{f.DescripcionArticuloFaltante}'");
                        continue;
                    }

                    detMatRepo.AddOrUpdate(
                        idProyecto: idProyecto,
                        idMaterial: idMaterial,
                        cantidad: f.CantidadFaltante,
                        valorGanancia: 0,
                        fechaIngreso: DateTime.Today
                    );
                }

                // 4) guardar snapshot (los faltantes ya son entidades de dominio)
                SnapshotService.Guardar(idInformeCompra, faltantes);

                // 5) borrar TODOS los Detalle_informe que referencien estos faltantes
                //    (puede haber informes viejos pendientes del mismo proyecto apuntando a los mismos IDs)
                detInfRepo.DeleteByMaterialFaltanteIds(idsFaltantes);

                // 6) marcar el informe actual como finalizado; cancelar los informes viejos huérfanos
                var pendientes = informeRepo.GetByProyecto(idProyecto);
                foreach (var inf in pendientes)
                {
                    inf.Estado = inf.IdInformeCompra == idInformeCompra ? "finalizado" : "cancelado";
                    informeRepo.Update(inf);
                }

                // 7) borrar Material_faltante — ahora sin referencias pendientes
                materialFaltanteRepo.DeleteByIds(idsFaltantes);

                uow.Commit();

                LoggerLogic.Info($"[InformeDeCompraBL] Compra confirmada y aplicada al proyecto. Proy={idProyecto} Informe={idInformeCompra}");
            }
            catch (AppException ex)
            {
                uow.Rollback();
                LoggerLogic.Warn($"[InformeDeCompraBL] Validación al confirmar compra: {ex.MessageKey}");
                throw;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] Falla al confirmar compra. Proy={idProyecto} Informe={idInformeCompra}", ex);
                throw;
            }
            finally
            {
                uow.Dispose();
            }
        }
    }
}
