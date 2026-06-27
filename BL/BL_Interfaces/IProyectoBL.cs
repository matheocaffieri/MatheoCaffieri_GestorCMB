using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IProyectoBL
    {
        void Add(Proyecto entity);
        void Update(Proyecto entity);
        void Delete(Proyecto entity);
        List<Proyecto> GetAll();
        Proyecto GetById(Guid id);
    }
}
