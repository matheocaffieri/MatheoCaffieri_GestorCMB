using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IInventarioRepository : IGenericRepository<DomainModel.Inventario>
    {
        DomainModel.Inventario GetByMaterialId(Guid idMaterial);
        decimal GetCantidad(Guid idMaterial);
    }
}
