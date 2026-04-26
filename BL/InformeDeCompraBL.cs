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

namespace BL
{
    public class InformeDeCompraBL
    {
        public Guid GenerarDesdeFaltantes(Guid idProyecto, bool unicoPorDia = true)
        {
            if (idProyecto == Guid.Empty)
                throw new AppException("err_proyecto_id_required");

            LoggerLogic.Info($"[InformeDeCompraBL] GenerarDesdeFaltantes START. idProyecto={idProyecto}, unicoPorDia={unicoPorDia}");

            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);

            uow.Begin();

            var informeRepo = new InformeDeCompraRepository(uow);
            var detalleRepo = new DetalleInformeMaterialFaltanteRepository(uow);

            try
            {
                var db = uow.Context;

                var faltantesIds = db.Material_faltante
                    .Where(m => m.idProyecto == idProyecto)
                    .Select(m => m.idMaterialFaltante)
                    .ToList();

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

                LoggerLogic.Info($"[InformeDeCompraBL] GenerarDesdeFaltantes OK. idInforme={informe.IdInformeCompra}");
                return informe.IdInformeCompra;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] GenerarDesdeFaltantes ERROR. idProyecto={idProyecto}. {ex.Message}");
                throw;
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }

        public List<InformeDeCompra> GetAll()
        {
            try
            {
                var ctx = new GestorCMBEntities();
                var uow = new SqlUnitOfWork(ctx);
                var repo = new InformeDeCompraRepository(uow);

                var list = repo.GetAll();

                uow.Dispose();
                ctx.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InformeDeCompraBL] GetAll ERROR. {ex.Message}");
                throw;
            }
        }

        public List<InformeDeCompra> GetHistorial()
        {
            try
            {
                var ctx = new GestorCMBEntities();
                var uow = new SqlUnitOfWork(ctx);
                var repo = new InformeDeCompraRepository(uow);

                var list = repo.GetHistorial();

                uow.Dispose();
                ctx.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InformeDeCompraBL] GetHistorial ERROR. {ex.Message}");
                throw;
            }
        }

        public void EliminarInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

            LoggerLogic.Info($"[InformeDeCompraBL] EliminarInforme START. idInforme={idInformeCompra}");

            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);
            uow.Begin();

            var infRepo = new InformeDeCompraRepository(uow);

            try
            {
                var inf = infRepo.GetById(idInformeCompra);
                if (inf != null)
                {
                    inf.Estado = "cancelado";
                    infRepo.Update(inf);
                }

                uow.Commit();
                LoggerLogic.Info($"[InformeDeCompraBL] EliminarInforme OK. idInforme={idInformeCompra}");
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] EliminarInforme ERROR. idInforme={idInformeCompra}. {ex.Message}");
                throw;
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }

        public void ConfirmarCompraYAplicar(Guid idProyecto, Guid idInformeCompra)
        {
            if (idProyecto == Guid.Empty)
                throw new AppException("err_proyecto_id_required");
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

            LoggerLogic.Info($"[InformeDeCompraBL] ConfirmarCompraYAplicar START. idProyecto={idProyecto}, idInforme={idInformeCompra}");

            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);

            uow.Begin();

            try
            {
                var db = uow.Context;

                var detMatRepo = new DetalleMaterialesRepository(uow);

                // 1) ids de Material_faltante incluidos en el informe
                var idsFaltantes = db.Detalle_informe_material_faltante
                    .Where(d => d.idInformeCompra == idInformeCompra)
                    .Select(d => d.idMaterialFaltante)
                    .ToList();

                if (idsFaltantes.Count == 0)
                    throw new AppException("err_informe_sin_materiales");

                // 2) traer faltantes (solo del proyecto) que están en el informe
                var faltantes = db.Material_faltante
                    .Where(m => idsFaltantes.Contains(m.idMaterialFaltante) && m.idProyecto == idProyecto)
                    .ToList();

                if (faltantes.Count == 0)
                    throw new AppException("err_informe_sin_faltantes_proyecto");

                // 3) aplicar compra: sumar al detalle del proyecto
                foreach (var f in faltantes)
                {
                    var idMaterial = db.Material
                        .Where(m =>
                            m.descripcionArticulo == f.descripcionArticuloFaltante &&
                            m.tipoMaterial == f.tipoMaterialFaltante &&
                            m.tipoUnidad == f.tipoUnidadMaterialFaltante
                        )
                        .Select(m => m.idMaterial)
                        .FirstOrDefault();

                    if (idMaterial == Guid.Empty)
                    {
                        throw new AppException("err_informe_material_no_existe");
                    }

                    detMatRepo.AddOrUpdate(
                        idProyecto: idProyecto,
                        idMaterial: idMaterial,
                        cantidad: (int)f.cantidadFaltante,
                        valorGanancia: 0,
                        fechaIngreso: DateTime.Today
                    );
                }

                // 4) guardar snapshot y borrar detalle del informe
                var snapshot = faltantes.Select(f => new MaterialFaltante
                {
                    CantidadFaltante               = (int)f.cantidadFaltante,
                    DescripcionArticuloFaltante    = f.descripcionArticuloFaltante,
                    TipoMaterialFaltante           = f.tipoMaterialFaltante,
                    TipoUnidadMaterialFaltante     = f.tipoUnidadMaterialFaltante
                }).ToList();
                SnapshotService.Guardar(idInformeCompra, snapshot);

                var detInfRows = db.Detalle_informe_material_faltante
                    .Where(d => d.idInformeCompra == idInformeCompra)
                    .ToList();
                db.Detalle_informe_material_faltante.RemoveRange(detInfRows);

                // 5) marcar informe como finalizado (no se borra)
                var infRow = db.Informe_compra.FirstOrDefault(i => i.idInformeCompra == idInformeCompra);
                if (infRow != null)
                    infRow.estado = "finalizado";

                // 6) borrar Material_faltante (los del informe)
                var mfRows = db.Material_faltante
                    .Where(m => idsFaltantes.Contains(m.idMaterialFaltante))
                    .ToList();
                db.Material_faltante.RemoveRange(mfRows);

                uow.Commit();

                LoggerLogic.Info($"[InformeDeCompraBL] ConfirmarCompraYAplicar OK. idProyecto={idProyecto}, idInforme={idInformeCompra}");
            }
            catch (Exception ex)
            {
                uow.Rollback();
                LoggerLogic.Error($"[InformeDeCompraBL] ConfirmarCompraYAplicar ERROR. idProyecto={idProyecto}, idInforme={idInformeCompra}. {ex.Message}");
                throw;
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }
    }
}
