using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProjectRepo
{
    public class MaterialFaltanteRepository : IMaterialesFaltantesRepository
    {

        private readonly GestorCMBEntities _context;

        public MaterialFaltanteRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public List<MaterialFaltante> GetAll(Guid idProyecto)
        {
            return _context.Material_faltante
                                       .AsNoTracking()
                                       .Where(d => d.idProyecto == idProyecto) // Filtra por el ID del proyecto
                                       .Select(d => new DomainModel.MaterialFaltante
                                       {
                                           IdMaterialFaltante = d.idMaterialFaltante,
                                           DescripcionArticuloFaltante = d.descripcionArticuloFaltante,
                                           TipoMaterialFaltante = d.tipoMaterialFaltante,
                                           TipoUnidadMaterialFaltante = d.tipoUnidadMaterialFaltante,
                                           IdProyecto = idProyecto,
                                           CantidadFaltante = d.cantidadFaltante
                                       })
                                       .ToList();
        }
    }
}
