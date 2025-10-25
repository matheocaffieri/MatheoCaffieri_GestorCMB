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
using System.Threading.Tasks;
using System.Windows.Forms;


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
                // Intenta hacer login usando el servicio de la capa BL
                Usuario usuarioLogueado = _loginService.Login(mail, password);

                if (usuarioLogueado != null)
                {
                    // Login exitoso: Guarda la sesión
                    SessionManager.Instance.Login(usuarioLogueado);
                    // después de autenticar:
                    UserSession.UserDisplayName = usuarioLogueado.Mail; // o el campo que tengas

                    // Abre el formulario principal

                    var rolesService = new RolesService(connectionStringUsers);
                    var usuarioService = new UsuarioService(connectionStringUsers);

                    MainForm mainForm = new MainForm(rolesService, usuarioService);
                    mainForm.Show();

                    // Cierra este formulario de login
                    this.Hide(); // O this.Close(); dependiendo si quieres poder volver o no
                    mainForm.FormClosed += (s, args) => this.Close(); // Cierra el login si se cierra el main

                }
                else
                {
                    // Login fallido (credenciales incorrectas o usuario inactivo)
                    MessageBox.Show("Mail o contraseña incorrectos, o el usuario está inactivo.", "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Podrías querer limpiar el campo de contraseña
                    textPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores más generales (ej. problema de conexión a DB)
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // Podrías loguear el error 'ex' para diagnóstico
            }
        }
    }
}
