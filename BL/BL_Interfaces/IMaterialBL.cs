using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IMaterialBL
    {
        void Add(Material entity);
        void Update(Material entity);
        void Delete(Material entity);
        Material GetById(Guid id);
        List<Material> GetAll();
    }
}
