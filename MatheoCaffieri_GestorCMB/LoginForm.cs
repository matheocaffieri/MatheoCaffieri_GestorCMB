using BL.AccessBL;
using BL.LoginBL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Services.RoleService;                 // SessionContext
using System.Threading.Tasks;
using System.Windows.Forms;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;


namespace MatheoCaffieri_GestorCMB
{
    public partial class LoginForm : Form
    {
        private LoginService _loginService; // Necesitarás inicializar esto (probablemente en el constructor)
        private IPasswordHasher _passwordHasher;
        string connectionStringUsers = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"]?.ConnectionString;


        // Constructor (ejemplo de inicialización)
        public LoginForm()
        {
            InitializeComponent();

            try
            {
                // --- Inicialización de Servicios ---

                // 1. Obtener la cadena de conexión desde App.config
                // Asegúrate de que el nombre "UsersConnectionString" exista en tu App.config

                if (string.IsNullOrEmpty(connectionStringUsers))
                {
                    MessageBox.Show("No se encontró la cadena de conexión 'UsersConnectionString' en App.config.", "Error de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Podrías deshabilitar el botón de login o cerrar la aplicación aquí
                    buttonLogin.Enabled = false;
                    return;
                }

                // 2. Crear la instancia del Hasher
                _passwordHasher = new PasswordHasher();

                // 3. Crear la instancia del LoginService, pasando la conexión y el hasher
                _loginService = new LoginService(connectionStringUsers);
            }
            catch (ConfigurationErrorsException configEx)
            {
                MessageBox.Show($"Error al leer la configuración: {configEx.Message}\nAsegúrate de que App.config está bien formado y contiene la cadena de conexión.", "Error de Configuración", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                buttonLogin.Enabled = false; // Deshabilitar login si hay error de config
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error durante la inicialización
                MessageBox.Show($"Error inesperado durante la inicialización: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                buttonLogin.Enabled = false; // Deshabilitar login
            }
        }

        private static void CargarPermisosUsuario(Usuario usuario, string cs)
        {
            if (usuario == null) return;

            var rolesService = AccessServicesFactory.CreateRolesService(cs);
            var usuarioPermisosService = AccessServicesFactory.CreateUsuarioPermisosService(cs);

            var set = new HashSet<TipoPermiso>();

            // Permisos directos
            foreach (var acc in usuarioPermisosService.ObtenerDirectos(usuario.IdUsuario))
                set.Add(acc.DataKey);

            // Permisos por roles
            foreach (var rol in rolesService.RolesDeUsuario(usuario.IdUsuario))
                foreach (var p in rolesService.ObtenerPermisosDeRol(rol.IdRol))
                    set.Add(p);

            // Hidratar en el objeto Usuario (SessionManager.TienePermiso usa esto)
            foreach (var p in set)
                usuario.AgregarPermiso(new Acceso(p.ToString().Replace('_', ' '), p));
        }


        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string mail = textMail.Text.Trim(); // Asume que tus TextBox se llaman así
            string password = textPassword.Text;

            // Validación básica de entrada
            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, ingrese su mail y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Nuevo: login con motivo (Ok / Inactivo / CredencialesInvalidas)
                var result = _loginService.TryLogin(mail, password, out Usuario usuarioLogueado);

                if (result == LoginResult.UsuarioInactivo)
                {
                    MessageBox.Show("Este usuario no está activo.", "Usuario inactivo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textPassword.Clear();
                    return;
                }

                if (result == LoginResult.CredencialesInvalidas)
                {
                    MessageBox.Show("Mail o contraseña incorrectos.", "Error de Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textPassword.Clear();
                    return;
                }

                // ===== Login OK (todo esto queda igual) =====

                SessionManager.Instance.Login(usuarioLogueado);
                UserSession.UserDisplayName = usuarioLogueado.Mail;

                CargarPermisosUsuario(usuarioLogueado, connectionStringUsers);

                var rolesService = AccessServicesFactory.CreateRolesService(connectionStringUsers);
                var usuarioService = new UsuarioService(connectionStringUsers);
                var userPermsService = AccessServicesFactory.CreateUsuarioPermisosService(connectionStringUsers);

                var permisos = new HashSet<TipoPermiso>();

                var rolesDelUsuario = rolesService.RolesDeUsuario(usuarioLogueado.IdUsuario);
                foreach (var rol in rolesDelUsuario)
                    permisos.UnionWith(rolesService.ObtenerPermisosDeRol(rol.IdRol));

                var directos = userPermsService.ObtenerDirectos(usuarioLogueado.IdUsuario)
                                              .Select(a => a.DataKey);
                permisos.UnionWith(directos);

                SessionContext.SetUsuario(usuarioLogueado.IdUsuario, permisos.Select(p => p.ToString()));

                MainForm mainForm = new MainForm(rolesService, usuarioService);
                mainForm.Show();

                this.Hide();
                mainForm.FormClosed += (s, args) => this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
    }
}
