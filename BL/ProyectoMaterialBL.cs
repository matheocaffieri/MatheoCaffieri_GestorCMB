using DAL.FactoryDAL;
using DAL.ProjectRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Entities;
using System.Threading.Tasks;

namespace BL
{
    public class ProyectoMaterialBL
    {
        private readonly string _cs;

        public ProyectoMaterialBL()
        {
            var ctx = new DAL.GestorCMBEntities();
            _cs = ctx.Database.Connection.ConnectionString;
        }

        public AsignacionMaterialResult AgregarMaterialDetalleProyectoDesdeInventario(
            Guid idProyecto,
            Guid idMaterial,
            int cantidadSolicitada,
            double valorGanancia)
        {
            if (idProyecto == Guid.Empty) throw new ArgumentException("idProyecto inválido.");
            if (idMaterial == Guid.Empty) throw new ArgumentException("idMaterial inválido.");
            if (cantidadSolicitada <= 0) throw new ArgumentException("cantidadSolicitada debe ser > 0.");

            using (var uow = new SqlUnitOfWork(_cs))
            {
                uow.Begin();

                try
                {
                    var invRepo = new InventarioRepository(uow);
                    var detRepo = new DetalleMaterialesRepository(uow);
                    var matRepo = new MaterialRepository(uow);
                    var faltRepo = new MaterialFaltanteRepository(uow);

                    var inv = invRepo.GetByMaterialId(idMaterial);
                    var stock = inv?.Cantidad ?? 0;

                    var cantidadAsignada = Math.Min(stock, cantidadSolicitada);
                    var cantidadFaltante = cantidadSolicitada - cantidadAsignada;

                    // 1) Asignar al proyecto (detalle) lo que haya en stock
                    if (cantidadAsignada > 0)
                    {
                        detRepo.AddOrUpdate(idProyecto, idMaterial, cantidadAsignada, valorGanancia, DateTime.Now);

                        // 2) Descontar inventario (sin negativo)
                        inv.Cantidad = stock - cantidadAsignada; // si stock==cantidadAsignada => 0
                        invRepo.Update(inv);
                    }

                    // 3) Si faltó, registrar material faltante
                    if (cantidadFaltante > 0)
                    {
                        var mat = matRepo.GetById(idMaterial);
                        if (mat == null) throw new InvalidOperationException("Material no encontrado para generar faltante.");

                        faltRepo.AddOrUpdate(
                            idProyecto,
                            mat.DescripcionArticulo,
                            mat.TipoMaterial,
                            mat.TipoUnidad,
                            cantidadFaltante
                        );
                    }

                    uow.Commit();

                    return new AsignacionMaterialResult(stock, cantidadSolicitada, cantidadAsignada, cantidadFaltante);
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }
    }
}
