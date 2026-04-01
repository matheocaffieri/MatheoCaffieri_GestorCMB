using BL;
using BL.AccessBL;
using BL.LoginBL;
using DomainModel.Exceptions;
using Services.Language;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;


namespace MatheoCaffieri_GestorCMB
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += (s, e) => HandleFatalException(e.Exception);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                HandleFatalException(e.ExceptionObject as Exception ?? new Exception("UnhandledException sin Exception"));

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                HandleFatalException(e.Exception);
                e.SetObserved();
            };

            try
            {
                var cultureCode = string.IsNullOrWhiteSpace(Properties.Settings.Default.CultureCode) ? "es-AR" : Properties.Settings.Default.CultureCode;
                var repo = new MatheoCaffieri_GestorCMB.Localization.ResxLanguageRepository();
                Services.Language.LanguageService.Initialize(repo, cultureCode);
            }
            catch
            {
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cs = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(cs))
            {
                MessageBox.Show(
                    "Falta el connection string 'Default' en App.config.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var dbManager = new DatabaseManager();
                dbManager.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}",
                                "Inicio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var rolesService = AccessServicesFactory.CreateRolesService(cs);
            var usuarioService = new UsuarioService(cs);

            Application.Run(new LoginForm());
        }

        private static void HandleFatalException(Exception ex)
        {
            try
            {
                LogHelper.Error("Excepción no controlada", ex);

                string msg;
                if (ex is AppException appEx)
                    msg = LanguageService.Current?.T(appEx.MessageKey) ?? appEx.Message;
                else
                    msg = LanguageService.Current?.T("err_generic") ?? "Ocurrió un error inesperado.";

                MessageBox.Show(
                    msg + "\n\n" +
                    "Se registró en el log. Abrí el visor de logs para ver el detalle.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch
            {
            }
        }
    }
}
