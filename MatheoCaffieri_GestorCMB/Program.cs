using BL;
using BL.AccessBL;
using BL.LoginBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;


namespace MatheoCaffieri_GestorCMB
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1) Connection string desde App.config
            var cs = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
            {
                MessageBox.Show(
                    "Falta el connection string 'Default' en App.config.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) Inicialización de DB (si aplica)
            try
            {
                var dbManager = new DatabaseManager();
                dbManager.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}",
                                "Inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Podés abortar si querés: return;
            }

            // 3) Construir servicios con CS (ajustá según tus constructores reales)
            // Si tus servicios aceptan (string cs):
            var rolesService = new RolesService(cs);
            var usuarioService = new UsuarioService(cs);

            // --- Alternativa si tus servicios aceptan repositorios ---
            // var usuarioRepo = new UsuarioAccesoRepository(cs);
            // var rolesRepo   = new RolesRepository(cs);
            // var rolesService   = new RolesService(rolesRepo);
            // var usuarioService = new UsuarioService(usuarioRepo);

            // 4) Correr la app con el MainForm que recibe servicios
            Application.Run(new LoginForm());
        }
    }
}
