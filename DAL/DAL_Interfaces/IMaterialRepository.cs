using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IMaterialRepository : IGenericRepository<DomainModel.Material>
    {
        // Busca el id de un material por descripción/tipo/unidad (match case-insensitive y trim).
        Guid FindIdByDescripcionTipoUnidad(string descripcion, string tipoMaterial, string tipoUnidad);
    }
}
