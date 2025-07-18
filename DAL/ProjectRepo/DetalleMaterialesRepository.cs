using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProjectRepo
{
    public class DetalleMaterialesRepository : IDetalleMaterialesRepository
    {

        private readonly GestorCMBEntities _context;

        public DetalleMaterialesRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<DetalleProyectoMaterial> GetAll(Guid idProyecto)
        {
            return _context.Detalle_proyecto_material
                           .AsNoTracking()
                           .Where(d => d.idProyecto == idProyecto) // Filtra por el ID del proyecto
                           .Include(d => d.Material) // Incluye los datos del empleado
                           .Select(d => new DomainModel.DetalleProyectoMaterial
                           {
                               IdDetalleMaterial = d.idDetalleMaterial,
                               IdProyecto = d.idProyecto,
                               IdMaterial = d.idMaterial,
                               Cantidad = d.cantidad,
                               ValorGanancia = (float)d.valorGanancia,
                               FechaIngresoMaterial = d.fechaIngresoMaterial,

                               Material = new DomainModel.Material
                               {
                                   IdMaterial = d.Material.idMaterial,
                                   DescripcionArticulo = d.Material.descripcionArticulo,
                                   TipoMaterial = d.Material.tipoMaterial,
                                   TipoUnidad = d.Material.tipoUnidad,
                                   CostoPorUnidad = (float)d.Material.costoPorUnidad,
                                   IdProveedor = d.Material.idProveedor
                               }
                           })
                           .ToList();
        }
    }
}
