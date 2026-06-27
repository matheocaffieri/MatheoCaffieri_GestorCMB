using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IProveedorBL
    {
        void Add(Proveedor entity);
        void Update(Proveedor entity);
        void Delete(Proveedor entity);
        Proveedor GetById(Guid id);
        List<Proveedor> GetAll();
    }
}
