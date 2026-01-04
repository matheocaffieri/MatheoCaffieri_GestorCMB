using DomainModel.Login;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.LoginDALInterfaces
{
    public interface IAccesoRepository
    {
        List<Acceso> GetAll();
        Acceso Create(string nombre, TipoPermiso dataKey);
    }
}
