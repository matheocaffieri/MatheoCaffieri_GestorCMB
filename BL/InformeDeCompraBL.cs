using DAL;
using DAL.FactoryDAL;
using DAL.ProjectRepo;
using DomainModel;
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
                throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));

            using (var context = new GestorCMBEntities())
            {
                var uow = new SqlUnitOfWork(context.Database.Connection.ConnectionString);

                uow.Begin();

                var informeRepo = new InformeDeCompraRepository(uow);
                var detalleRepo = new DetalleInformeMaterialFaltanteRepository(uow);

                try
                {
                    var faltantesIds = context.Material_faltante
                        .Where(m => m.idProyecto == idProyecto)
                        .Select(m => m.idMaterialFaltante)
                        .ToList();

                    if (faltantesIds.Count == 0)
                        throw new InvalidOperationException("Este proyecto no tiene materiales faltantes.");

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
                            FechaRealizacion = DateTime.Today
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
                    return informe.IdInformeCompra;
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        // === NUEVO: listar informes ===
        public List<InformeDeCompra> GetAll()
        {
            using (var context = new GestorCMBEntities())
            {
                var uow = new SqlUnitOfWork(context.Database.Connection.ConnectionString);
                var repo = new InformeDeCompraRepository(uow);
                return repo.GetAll();
            }
        }

        // === NUEVO: Eliminar (borra detalle + informe) ===
        public void EliminarInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty)
                throw new ArgumentException("idInformeCompra requerido.", nameof(idInformeCompra));

            using (var context = new GestorCMBEntities())
            {
                var uow = new SqlUnitOfWork(context.Database.Connection.ConnectionString);
                uow.Begin();

                var infRepo = new InformeDeCompraRepository(uow);
                var detRepo = new DetalleInformeMaterialFaltanteRepository(uow);

                try
                {
                    // borrar detalle del informe
                    var detallesIds = context.Detalle_informe_material_faltante
                        .Where(d => d.idInformeCompra == idInformeCompra)
                        .Select(d => d.idDetalleMaterialFaltante)
                        .ToList();

                    foreach (var idDet in detallesIds)
                    {
                        var dom = detRepo.GetById(idDet);
                        if (dom != null) detRepo.Delete(dom);
                    }

                    // borrar informe
                    var inf = infRepo.GetById(idInformeCompra);
                    if (inf != null) infRepo.Delete(inf);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        // === NUEVO: Agregar compra (aplicar compra y limpiar todo) ===
        // Regla pedida:
        // - agrega los materiales al detalle del proyecto
        // - borra informe + detalle
        // - borra materiales faltantes del proyecto (los que estaban en el informe)
        public void ConfirmarCompraYAplicar(Guid idProyecto, Guid idInformeCompra)
        {
            if (idProyecto == Guid.Empty)
                throw new ArgumentException("idProyecto requerido.", nameof(idProyecto));
            if (idInformeCompra == Guid.Empty)
                throw new ArgumentException("idInformeCompra requerido.", nameof(idInformeCompra));

            // Solo para obtener el connectionString
            using (var tmp = new GestorCMBEntities())
            using (var uow = new SqlUnitOfWork(tmp.Database.Connection.ConnectionString))
            {
                uow.Begin();
                var ctx = uow.Context;

                var detMatRepo = new DetalleMaterialesRepository(uow);

                try
                {
                    // 1) ids de Material_faltante incluidos en el informe
                    var idsFaltantes = ctx.Detalle_informe_material_faltante
                        .Where(d => d.idInformeCompra == idInformeCompra)
                        .Select(d => d.idMaterialFaltante)
                        .ToList();

                    if (idsFaltantes.Count == 0)
                        throw new InvalidOperationException("El informe no tiene materiales.");

                    // 2) traer faltantes (solo del proyecto) que están en el informe
                    var faltantes = ctx.Material_faltante
                        .Where(m => idsFaltantes.Contains(m.idMaterialFaltante) && m.idProyecto == idProyecto)
                        .ToList();

                    if (faltantes.Count == 0)
                        throw new InvalidOperationException("No se encontraron materiales faltantes para ese proyecto/informe.");

                    // 3) aplicar compra: sumar al detalle del proyecto
                    foreach (var f in faltantes)
                    {
                        // Resolver material EXISTENTE por match exacto (rápido y SQL-friendly)
                        var idMaterial = ctx.Material
                            .Where(m =>
                                m.descripcionArticulo == f.descripcionArticuloFaltante &&
                                m.tipoMaterial == f.tipoMaterialFaltante &&
                                m.tipoUnidad == f.tipoUnidadMaterialFaltante
                            )
                            .Select(m => m.idMaterial)
                            .FirstOrDefault();

                        if (idMaterial == Guid.Empty)
                        {
                            throw new InvalidOperationException(
                                $"No existe el material en Inventario/Material para: '{f.descripcionArticuloFaltante}' " +
                                $"({f.tipoMaterialFaltante} / {f.tipoUnidadMaterialFaltante}). " +
                                "Cargalo primero en Inventario/Material y volvé a aplicar la compra."
                            );
                        }

                        detMatRepo.AddOrUpdate(
                            idProyecto: idProyecto,
                            idMaterial: idMaterial,
                            cantidad: (int)f.cantidadFaltante,
                            valorGanancia: 0,
                            fechaIngreso: DateTime.Today
                        );
                    }

                    // 4) borrar detalle del informe
                    var detInfRows = ctx.Detalle_informe_material_faltante
                        .Where(d => d.idInformeCompra == idInformeCompra)
                        .ToList();
                    ctx.Detalle_informe_material_faltante.RemoveRange(detInfRows);

                    // 5) borrar informe
                    var infRow = ctx.Informe_compra.FirstOrDefault(i => i.idInformeCompra == idInformeCompra);
                    if (infRow != null)
                        ctx.Informe_compra.Remove(infRow);

                    // 6) borrar Material_faltante (los del informe)
                    var mfRows = ctx.Material_faltante
                        .Where(m => idsFaltantes.Contains(m.idMaterialFaltante))
                        .ToList();
                    ctx.Material_faltante.RemoveRange(mfRows);

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }


        private Guid ResolverMaterialExistente(
            List<dynamic> materiales,
            string descripcion,
            string tipo,
            string unidad)
        {
            descripcion = (descripcion ?? "").Trim();
            tipo = (tipo ?? "").Trim();
            unidad = (unidad ?? "").Trim();

            var mat = materiales.FirstOrDefault(m =>
                string.Equals((string)m.descripcionArticulo, descripcion, StringComparison.OrdinalIgnoreCase) &&
                string.Equals((string)m.tipoMaterial, tipo, StringComparison.OrdinalIgnoreCase) &&
                string.Equals((string)m.tipoUnidad, unidad, StringComparison.OrdinalIgnoreCase));

            if (mat == null)
            {
                throw new InvalidOperationException(
                    $"No existe el material en Inventario/Material para: '{descripcion}' ({tipo} / {unidad}). " +
                    "Cargalo primero en Inventario/Material y volvé a aplicar la compra."
                );
            }

            return (Guid)mat.idMaterial;
        }


        // === helper: busca material en inventario por descripcion/tipo/unidad; si no existe, lo crea ===
        // AJUSTÁ nombres si tu entidad/columnas se llaman distinto (Material / descripcionArticulo / tipoMaterial / tipoUnidadMaterial / idMaterial)
        private Guid ResolverOCrearMaterialDesdeFaltante(GestorCMBEntities context, string descripcion, string tipo, string unidad)
        {
            descripcion = (descripcion ?? "").Trim();
            tipo = (tipo ?? "").Trim();
            unidad = (unidad ?? "").Trim();

            var mat = context.Material
                .FirstOrDefault(m =>
                    m.descripcionArticulo == descripcion &&
                    m.tipoMaterial == tipo &&
                    m.tipoUnidad == unidad);

            if (mat != null) return mat.idMaterial;

            var nuevoId = Guid.NewGuid();
            var nuevo = new DAL.Material
            {
                idMaterial = nuevoId,
                descripcionArticulo = descripcion,
                tipoMaterial = tipo,
                tipoUnidad = unidad,
                // completá acá defaults si tu tabla los exige (stock, etc.)
            };

            context.Material.Add(nuevo);
            context.SaveChanges();

            return nuevoId;
        }
    }
}
