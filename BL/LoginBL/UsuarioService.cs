using DAL.LoginDAL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Collections.Generic;


namespace BL.LoginBL
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;

        public UsuarioService(IUsuarioRepository usuarioRepo, IPasswordHasher hasher)
        {
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
        }

        // <- OVERLOAD para UI
        public UsuarioService(string connectionString)
    : this(new UsuarioRepository(), new PasswordHasher())
        { }


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
