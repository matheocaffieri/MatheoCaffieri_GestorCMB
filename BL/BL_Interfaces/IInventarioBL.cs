using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IInventarioBL
    {
        void Add(Inventario entity);
        void Update(Inventario entity);
        void Delete(Inventario entity);
        Inventario GetById(Guid id);
        List<Inventario> GetAll();
        Inventario GetByMaterialId(Guid idMaterial);
        decimal GetCantidad(Guid idMaterial);
        int CambiarCantidad(Guid idInventario, int delta);
    }
}
