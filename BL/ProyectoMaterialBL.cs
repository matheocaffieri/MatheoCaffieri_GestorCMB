using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel.Entities;
using DomainModel.Exceptions;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class ProyectoMaterialBL : IProyectoMaterialBL
    {
        public AsignacionMaterialResult AgregarMaterialDetalleProyectoDesdeInventario(
            Guid idProyecto,
            Guid idMaterial,
            int cantidadSolicitada,
            double valorGanancia)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");
            if (cantidadSolicitada <= 0) throw new AppException("err_material_cantidad_invalida");

            using (var uow = DalFactory.CreateUnitOfWork())
            {
                uow.Begin();

                var invRepo = DalFactory.CreateInventarioRepository(uow);
                var detRepo = DalFactory.CreateDetalleMaterialesRepository(uow);
                var matRepo = DalFactory.CreateMaterialRepository(uow);
                var faltRepo = DalFactory.CreateMaterialFaltanteRepository(uow);

                try
                {
                    var inv = invRepo.GetByMaterialId(idMaterial);
                    var stock = inv?.Cantidad ?? 0;

                    var cantidadAsignada = Math.Min(stock, cantidadSolicitada);
                    var cantidadFaltante = cantidadSolicitada - cantidadAsignada;

                    // 1) Asignar al proyecto (detalle) lo que haya en stock
                    if (cantidadAsignada > 0)
                    {
                        detRepo.AddOrUpdate(idProyecto, idMaterial, cantidadAsignada, valorGanancia, DateTime.Now);

                        // 2) Descontar inventario (sin negativo)
                        if (inv == null)
                            throw new AppException("err_inventario_inconsistencia");

                        inv.Cantidad = stock - cantidadAsignada; // puede quedar 0
                        invRepo.Update(inv);
                    }

                    // 3) Si faltó, registrar material faltante
                    if (cantidadFaltante > 0)
                    {
                        var mat = matRepo.GetById(idMaterial);
                        if (mat == null) throw new AppException("err_material_not_found_faltante");

                        faltRepo.AddOrUpdate(
                            idProyecto,
                            mat.DescripcionArticulo,
                            mat.TipoMaterial,
                            mat.TipoUnidad,
                            cantidadFaltante
                        );
                    }

                    uow.Commit();

                    LoggerLogic.Info(
                        $"[ProyectoMaterialBL] Material asignado al proyecto. Proy={idProyecto} Mat={idMaterial} " +
                        $"Asig={cantidadAsignada} Falt={cantidadFaltante}");

                    return new AsignacionMaterialResult(stock, cantidadSolicitada, cantidadAsignada, cantidadFaltante);
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoMaterialBL] Validación al asignar material: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error(
                        $"[ProyectoMaterialBL] Falla asignando material. Proy={idProyecto} Mat={idMaterial} Sol={cantidadSolicitada}",
                        ex);
                    throw;
                }
            }
        }

        public void QuitarMaterialDelProyecto(Guid idProyecto, Guid idMaterial)
        {
            if (idProyecto == Guid.Empty) throw new AppException("err_proyecto_id_required");
            if (idMaterial == Guid.Empty) throw new AppException("err_inventario_material_required");

            using (var uow = DalFactory.CreateUnitOfWork())
            {
                uow.Begin();

                var detRepo = DalFactory.CreateDetalleMaterialesRepository(uow);
                var invRepo = DalFactory.CreateInventarioRepository(uow);

                try
                {
                    int cantidad = detRepo.Delete(idProyecto, idMaterial);

                    if (cantidad > 0)
                    {
                        var inv = invRepo.GetByMaterialId(idMaterial);
                        if (inv != null)
                        {
                            inv.Cantidad += cantidad;
                            invRepo.Update(inv);
                        }
                    }

                    uow.Commit();
                    LoggerLogic.Info($"[ProyectoMaterialBL] Material quitado del proyecto. Proy={idProyecto} Mat={idMaterial} CantDevuelta={cantidad}");
                }
                catch (AppException ex)
                {
                    uow.Rollback();
                    LoggerLogic.Warn($"[ProyectoMaterialBL] Validación al quitar material: {ex.MessageKey}");
                    throw;
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error($"[ProyectoMaterialBL] Falla al quitar material. Proy={idProyecto} Mat={idMaterial}", ex);
                    throw;
                }
            }
        }
    }
}
