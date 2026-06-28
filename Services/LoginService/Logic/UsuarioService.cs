using System;
using System.Configuration;
using Services.LoginService.DataAccess;
using DomainModel.Exceptions;
using Services.Login;
using System.Collections.Generic;
using Services.LoginService;
using Services.RoleService.Logic;
using Services.Services_Interfaces;

namespace Services.LoginService.Logic
{
    public class UsuarioService : IDisposable
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;
        private readonly ILoginUnitOfWork _uow;

        // DI: el UoW debe ser ILoginUnitOfWork (ADO.NET directo), NO el IUnitOfWork de EF.
        public UsuarioService(ILoginUnitOfWork uow, IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        // Recibe el nombre de la connection string definida en App.config o una cadena literal.
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

        // El usuario administrador es intocable: ninguna operación de gestión puede afectarlo.
        private static void RechazarSiEsAdmin(Guid idUsuario)
        {
            if (idUsuario == RolesService.AdminUserId)
                throw new AppException("err_admin_protegido");
        }

        public void SetActivo(Guid idUsuario, bool activo)
        {
            RechazarSiEsAdmin(idUsuario);
            _usuarioRepo.SetActivo(idUsuario, activo);
        }

        public Usuario ObtenerPorId(Guid id) => _usuarioRepo.GetById(id);

        public Usuario ObtenerPorMail(string mail) => _usuarioRepo.FindByEmail(mail);

        // El mail identifica al usuario en el login: no puede repetirse.
        // Al actualizar, se excluye al propio usuario para permitir guardar sin cambiar el mail.
        private void RechazarSiMailDuplicado(Usuario u)
        {
            var existente = _usuarioRepo.FindByEmail(u.Mail);
            if (existente != null && existente.IdUsuario != u.IdUsuario)
                throw new AppException("err_mail_duplicado");
        }

        public void CrearUsuario(Usuario u, string contraseñaPlano)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            RechazarSiMailDuplicado(u);
            u.Contraseña = _hasher.Hash(contraseñaPlano);
            _usuarioRepo.Add(u);
        }

        // nuevaContraseñaPlano: null o vacía = mantener la contraseña actual;
        // con valor = se hashea acá y reemplaza a la anterior. u.Contraseña nunca
        // debe traer texto plano desde la UI.
        public void ActualizarUsuario(Usuario u, string nuevaContraseñaPlano = null)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            RechazarSiEsAdmin(u.IdUsuario);
            RechazarSiMailDuplicado(u);

            if (!string.IsNullOrWhiteSpace(nuevaContraseñaPlano))
                u.Contraseña = _hasher.Hash(nuevaContraseñaPlano);

            _usuarioRepo.Update(u);
        }

        public void EliminarUsuario(Guid id)
        {
            RechazarSiEsAdmin(id);
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
