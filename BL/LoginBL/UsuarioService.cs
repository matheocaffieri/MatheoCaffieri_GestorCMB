using System;
using System.Configuration;
using DAL.FactoryDAL;
using DAL.LoginDAL;
using DomainModel.Interfaces;
using DomainModel.Login;
using System.Collections.Generic;
using Services.LoginService;
using Interfaces.LoginInterfaces;
using DomainModel.LoginDALInterfaces;

namespace BL.LoginBL
{
    public class UsuarioService : IDisposable
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;
        private readonly ILoginUnitOfWork _uow;

        // DI: te recomiendo que el factory de Login devuelva ILoginUnitOfWork (no el de EF)
        // Si todavía no tenés ese factory, usá el ctor legacy o el ctor directo.
        public UsuarioService(ILoginUnitOfWork uow, IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        // Legacy (string): nombre de CS o CS directa
        public UsuarioService(string connectionStringOrName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringOrName))
                throw new ArgumentNullException(nameof(connectionStringOrName));

            var cs = TryResolveConnectionString(connectionStringOrName);

            _uow = new SqlLoginUnitOfWork(cs);
            _usuarioRepo = new UsuarioRepository(_uow);
            _hasher = new PasswordHasher();
        }

        private static string TryResolveConnectionString(string csOrName)
        {
            try
            {
                var entry = ConfigurationManager.ConnectionStrings[csOrName];
                if (entry != null && !string.IsNullOrWhiteSpace(entry.ConnectionString))
                    return entry.ConnectionString;
            }
            catch { }
            return csOrName;
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }

        public List<Usuario> ObtenerTodos() => _usuarioRepo.GetAll();

        public void SetActivo(Guid idUsuario, bool activo) => _usuarioRepo.SetActivo(idUsuario, activo);

        public Usuario ObtenerPorId(Guid id) => _usuarioRepo.GetById(id);

        public Usuario ObtenerPorMail(string mail) => _usuarioRepo.FindByEmail(mail);

        public void CrearUsuario(Usuario u, string contraseñaPlano)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            u.Contraseña = _hasher.Hash(contraseñaPlano);
            _usuarioRepo.Add(u);
        }

        public void ActualizarUsuario(Usuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            _usuarioRepo.Update(u);
        }

        public void EliminarUsuario(Guid id)
        {
            var usuario = _usuarioRepo.GetById(id);
            if (usuario != null)
                _usuarioRepo.Delete(usuario);
        }

        public bool ValidarLogin(string mail, string contraseñaPlano)
        {
            var usuario = _usuarioRepo.FindByEmail(mail);
            if (usuario == null) return false;
            return _hasher.Verify(usuario.Contraseña, contraseñaPlano);
        }
    }
}
