using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    public interface IDetalleGeneric<T> where T : class
    {
        List<T> GetAll(Guid idProyecto);
    }
}
