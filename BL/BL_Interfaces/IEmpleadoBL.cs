using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IEmpleadoBL
    {
        void Add(Empleado entity);
        void Update(Empleado entity);
        void Delete(Empleado entity);
        List<Empleado> GetAll();
        Empleado GetById(Guid id);
    }
}
