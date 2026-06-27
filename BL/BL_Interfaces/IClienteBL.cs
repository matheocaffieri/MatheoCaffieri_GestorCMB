using System;
using System.Collections.Generic;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IClienteBL
    {
        void Add(Cliente entity);
        void Update(Cliente entity);
        void Delete(Cliente entity);
        List<Cliente> GetAll();
        Cliente GetById(Guid id);
    }
}
