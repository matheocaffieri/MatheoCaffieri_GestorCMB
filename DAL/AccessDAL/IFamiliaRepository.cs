using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.AccessDAL
{
    public interface IFamiliaRepository
    {
        List<(Guid Id, string Nombre)> GetAll();
        Guid Create(string nombre);

        List<Acceso> GetAccesos(Guid idFamilia);
        void AddAcceso(Guid idFamilia, Guid idAcceso);
        void RemoveAcceso(Guid idFamilia, Guid idAcceso);

        List<Usuario> GetUsuarios(Guid idFamilia);
        void AddUsuario(Guid idFamilia, Guid idUsuario);
        void RemoveUsuario(Guid idFamilia, Guid idUsuario);
    }
}
