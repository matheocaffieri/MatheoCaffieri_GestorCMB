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

namespace BL
{
    public class ProyectoMaterialBL
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

            using (var ctx = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(ctx))
            {
                uow.Begin();

                var invRepo = new InventarioRepository(uow);
                var detRepo = new DetalleMaterialesRepository(uow);
                var matRepo = new MaterialRepository(uow);
                var faltRepo = new MaterialFaltanteRepository(uow);

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
                        $"[ProyectoMaterialBL] Asignación material ok. Proy={idProyecto} Mat={idMaterial} " +
                        $"Stock={stock} Sol={cantidadSolicitada} Asig={cantidadAsignada} Falt={cantidadFaltante}");

                    return new AsignacionMaterialResult(stock, cantidadSolicitada, cantidadAsignada, cantidadFaltante);
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    LoggerLogic.Error(
                        $"[ProyectoMaterialBL] Error asignando material. Proy={idProyecto} Mat={idMaterial} Sol={cantidadSolicitada}",
                        ex);
                    throw;
                }
            }
        }
    }
}
