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
        private readonly IUnitOfWork _uow; 

        // --- Constructor “correcto” con DI (usa Factory/UoW compartido) ---
        public UsuarioService(IRepositoryFactory factory, IPasswordHasher hasher, string csName = "MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios")
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (hasher == null) throw new ArgumentNullException(nameof(hasher));

            _uow = factory.CreateUnitOfWork(csName);         
            var bundle = factory.CreateRepositories(_uow);    
            _usuarioRepo = bundle.Usuarios;
            _hasher = hasher;
        }



        public UsuarioService(IUsuarioRepository usuarioRepo, IPasswordHasher hasher, IUnitOfWork uow = null)
        {
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _uow = uow; // puede ser null si el repo maneja su propio ciclo de vida
        }

        // --- Overload LEGACY (acepta string: nombre de CS o CS completa) ---
        public UsuarioService(string connectionStringOrName)
        {
            if (string.IsNullOrWhiteSpace(connectionStringOrName))
                throw new ArgumentNullException(nameof(connectionStringOrName));

            // Si es nombre de CS en App.config, lo resolvemos; si no, lo tomamos como CS directa
            var cs = TryResolveConnectionString(connectionStringOrName);

            // Creamos UoW y repo "a mano"
            _uow = new SqlUnitOfWork(cs);
            _usuarioRepo = new UsuarioRepository(_uow);
            _hasher = new Services.LoginService.PasswordHasher();
        }


        private static string TryResolveConnectionString(string csOrName)
        {
            try
            {
                var entry = ConfigurationManager.ConnectionStrings[csOrName];
                if (entry != null && !string.IsNullOrWhiteSpace(entry.ConnectionString))
                    return entry.ConnectionString;
            }
            catch
            {
                // Si no está el paquete/config, simplemente caemos a usar csOrName como CS directa
            }
            return csOrName;
        }


        // Helper: crea el repositorio a partir de un string o nombre de conexión
        private static IUsuarioRepository CreateRepoFromString(string csOrName)
        {
            if (string.IsNullOrWhiteSpace(csOrName))
                throw new ArgumentNullException(nameof(csOrName));

            // Si te pasan el nombre de una connection string, la busca en el app.config
            string cs = csOrName;
            var csEntry = System.Configuration.ConfigurationManager.ConnectionStrings[csOrName];
            if (csEntry != null)
                cs = csEntry.ConnectionString;

            // Crea un UnitOfWork con esa cadena de conexión
            var uow = new DAL.FactoryDAL.SqlUnitOfWork(cs);

            // Devuelve el repo ya inicializado con ese UnitOfWork
            return new DAL.LoginDAL.UsuarioRepository(uow);
        }



        public void Dispose()
        {
            // Si viniste por el ctor DI con factory, _uow no es null y lo cerramos acá
            if (_uow != null) _uow.Dispose();
        }


        public List<Usuario> ObtenerTodos()
        {
            return _usuarioRepo.GetAll();
        }

        public void SetActivo(Guid idUsuario, bool activo)
        {
            _usuarioRepo.SetActivo(idUsuario, activo);
        }

        public Usuario ObtenerPorId(Guid id)
        {
            return _usuarioRepo.GetById(id);
        }

        public Usuario ObtenerPorMail(string mail)
        {
            return _usuarioRepo.FindByEmail(mail);
        }

        public void CrearUsuario(Usuario u, string contraseñaPlano)
        {
            u.Contraseña = _hasher.Hash(contraseñaPlano);
            _usuarioRepo.Add(u);
        }

        public void ActualizarUsuario(Usuario u)
        {
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
