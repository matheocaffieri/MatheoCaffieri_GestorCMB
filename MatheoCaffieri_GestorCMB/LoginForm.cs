using BL.AccessBL;
using BL.LoginBL;
using DomainModel.Exceptions;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.Language;
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
using Services.RoleService.Logic;


namespace MatheoCaffieri_GestorCMB
{
    public partial class LoginForm : Form
    {
        private LoginService _loginService; // Necesitarás inicializar esto (probablemente en el constructor)
        private IPasswordHasher _passwordHasher;
        string connectionStringUsers = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"]?.ConnectionString;


        // Constructor (ejemplo de inicialización)
        private System.Drawing.Point _mouseLocation;

        public LoginForm()
        {
            InitializeComponent();

            this.MouseDown += (s, e) => { _mouseLocation = new System.Drawing.Point(-e.X, -e.Y); };
            this.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pos = MousePosition;
                    pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                    Location = pos;
                }
            };
            panel1.MouseDown += (s, e) => { _mouseLocation = new System.Drawing.Point(-e.X, -e.Y); };
            panel1.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pos = MousePosition;
                    pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                    Location = pos;
                }
            };

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    buttonLogin_Click(s, e);
            };

            try
            {
                // --- Inicialización de Servicios ---

                // 1. Obtener la cadena de conexión desde App.config
                // Asegúrate de que el nombre "UsersConnectionString" exista en tu App.config

                if (string.IsNullOrEmpty(connectionStringUsers))
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_config_connection_string") ?? "No se encontró la cadena de conexión en App.config.",
                        LanguageService.Current?.T("cap_error_config") ?? "Error de Configuración",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(
                    LanguageService.Current?.T("err_config_lectura") ?? "Error al leer la configuración.",
                    LanguageService.Current?.T("cap_error_config") ?? "Error de Configuración",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                buttonLogin.Enabled = false; // Deshabilitar login si hay error de config
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error durante la inicialización
                MessageBox.Show(
                    LanguageService.Current?.T("err_init_inesperado") ?? "Error inesperado durante la inicialización.",
                    LanguageService.Current?.T("cap_error_critico") ?? "Error Crítico",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                MessageBox.Show(
                    LanguageService.Current?.T("val_login_campos_vacios") ?? "Por favor, ingrese su mail y contraseña.",
                    LanguageService.Current?.T("cap_campos_vacios") ?? "Campos Vacíos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Nuevo: login con motivo (Ok / Inactivo / CredencialesInvalidas)
                var result = _loginService.TryLogin(mail, password, out Usuario usuarioLogueado);

                if (result == LoginResult.UsuarioInactivo)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_usuario_inactivo") ?? "Este usuario no está activo.",
                        LanguageService.Current?.T("cap_usuario_inactivo") ?? "Usuario inactivo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textPassword.Clear();
                    return;
                }

                if (result == LoginResult.CredencialesInvalidas)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_credenciales_incorrectas") ?? "Mail o contraseña incorrectos.",
                        LanguageService.Current?.T("cap_error_login") ?? "Error de Login",
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

                // Cargar parámetros globales de la empresa
                var parametrosService = AccessServicesFactory.CreateParametrosService(connectionStringUsers);
                Services.RoleService.ParametrosContext.Cargar(parametrosService.Obtener());


                var idioma = usuarioLogueado.Idioma;
                var cultureCode = idioma == "en" ? "en-US" : "es-AR";
                Properties.Settings.Default.CultureCode = cultureCode;
                Properties.Settings.Default.Save();

                var repo = new MatheoCaffieri_GestorCMB.Localization.ResxLanguageRepository();
                var lang = new Services.Language.LanguageService(repo, cultureCode);
                lang.SetCulture(cultureCode);


                MainForm mainForm = new MainForm(rolesService, usuarioService, parametrosService);
                mainForm.Show();

                this.Hide();
                mainForm.FormClosed += (s, args) => this.Close();
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_generic") ?? "Ocurrió un error inesperado.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
