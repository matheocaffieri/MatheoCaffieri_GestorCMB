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
                ctx.Dispose();
            }
        }

        public List<InformeDeCompra> GetAll()
        {
            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);
            try
            {
                var repo = new InformeDeCompraRepository(uow);
                return repo.GetAll();
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }

        public List<InformeDeCompra> GetHistorial()
        {
            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);
            try
            {
                var repo = new InformeDeCompraRepository(uow);
                return repo.GetHistorial();
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }

        public HashSet<Guid> GetMaterialesConInformesPendientes()
        {
            var ctx = new GestorCMBEntities();
            var uow = new SqlUnitOfWork(ctx);
            try
            {
                var repo = new InformeDeCompraRepository(uow);
                return repo.GetMaterialesConInformesPendientes();
            }
            finally
            {
                uow.Dispose();
                ctx.Dispose();
            }
        }

        public void EliminarInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

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
                ctx.Dispose();
            }
        }

        public void ConfirmarCompraYAplicar(Guid idProyecto, Guid idInformeCompra)
        {
            if (idProyecto == Guid.Empty)
                throw new AppException("err_proyecto_id_required");
            if (idInformeCompra == Guid.Empty)
                throw new AppException("err_informe_id_required");

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
                    var descLow = (f.descripcionArticuloFaltante ?? "").Trim().ToLowerInvariant();
                    var tipoLow = (f.tipoMaterialFaltante        ?? "").Trim().ToLowerInvariant();
                    var unidLow = (f.tipoUnidadMaterialFaltante  ?? "").Trim().ToLowerInvariant();

                    // Traemos candidatos por descripción (la más discriminante) y filtramos en memoria.
                    var idMaterial = db.Material
                        .Where(m => m.descripcionArticulo == f.descripcionArticuloFaltante)
                        .AsEnumerable()
                        .Where(m =>
                            (m.descripcionArticulo ?? "").Trim().ToLowerInvariant() == descLow &&
                            (m.tipoMaterial        ?? "").Trim().ToLowerInvariant() == tipoLow &&
                            (m.tipoUnidad          ?? "").Trim().ToLowerInvariant() == unidLow)
                        .Select(m => m.idMaterial)
                        .FirstOrDefault();

                    if (idMaterial == Guid.Empty)
                    {
                        LoggerLogic.Warn($"[InformeDeCompraBL] Material no encontrado en inventario, se omite. Desc='{f.descripcionArticuloFaltante}'");
                        continue;
                    }

                    detMatRepo.AddOrUpdate(
                        idProyecto: idProyecto,
                        idMaterial: idMaterial,
                        cantidad: (int)f.cantidadFaltante,
                        valorGanancia: 0,
                        fechaIngreso: DateTime.Today
                    );
                }

                // 4) guardar snapshot
                var snapshot = faltantes.Select(f => new MaterialFaltante
                {
                    CantidadFaltante               = (int)f.cantidadFaltante,
                    DescripcionArticuloFaltante    = f.descripcionArticuloFaltante,
                    TipoMaterialFaltante           = f.tipoMaterialFaltante,
                    TipoUnidadMaterialFaltante     = f.tipoUnidadMaterialFaltante
                }).ToList();
                SnapshotService.Guardar(idInformeCompra, snapshot);

                // 5) borrar TODOS los Detalle_informe que referencien estos faltantes
                //    (puede haber informes viejos pendientes del mismo proyecto apuntando a los mismos IDs)
                var detInfTodos = db.Detalle_informe_material_faltante
                    .Where(d => idsFaltantes.Contains(d.idMaterialFaltante))
                    .ToList();
                db.Detalle_informe_material_faltante.RemoveRange(detInfTodos);

                // 6) marcar el informe actual como finalizado; cancelar los informes viejos huérfanos
                var todosInformes = db.Informe_compra
                    .Where(i => i.idProyecto == idProyecto && i.estado == "pendiente")
                    .ToList();
                foreach (var inf in todosInformes)
                    inf.estado = inf.idInformeCompra == idInformeCompra ? "finalizado" : "cancelado";

                // 7) borrar Material_faltante — ahora sin referencias pendientes
                var mfRows = db.Material_faltante
                    .Where(m => idsFaltantes.Contains(m.idMaterialFaltante))
                    .ToList();
                db.Material_faltante.RemoveRange(mfRows);

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
                ctx.Dispose();
            }
        }
    }
}
