using DomainModel.Login;
using System;
using System.Collections.Generic;

namespace Services.Services_Interfaces
{
    // Contrato propio de Services para el acceso a datos de usuarios (ADO.NET).
    // No reutiliza el IGenericRepository del dominio: la capa de usuarios es independiente.
    public interface IUsuarioRepository
    {
        void Add(Usuario entity);
        void Update(Usuario entity);
        void Delete(Usuario entity);
        Usuario GetById(Guid id);
        List<Usuario> GetAll();

        Usuario FindByEmail(string mail);
        void SetActivo(Guid idUsuario, bool activo);
    }
}
