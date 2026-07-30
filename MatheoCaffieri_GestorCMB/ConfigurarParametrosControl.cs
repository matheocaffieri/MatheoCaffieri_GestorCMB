using BL;
using Services.Login;
using Services.Language;
using Services.Logs;
using Services.RoleService;
using Services.RoleService.Logic;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class ConfigurarParametrosControl : UserControl
    {
        private readonly ParametrosService _parametrosService;
        private Parametros _parametros;

        public ConfigurarParametrosControl(ParametrosService parametrosService)
        {
            InitializeComponent();
            _parametrosService = parametrosService ?? throw new ArgumentNullException(nameof(parametrosService));
            this.Load += ConfigurarParametrosControl_Load;
        }

        private void ConfigurarParametrosControl_Load(object sender, EventArgs e)
        {
            buttonBackup.Text = LanguageService.Current?.T("btn_backup") ?? buttonBackup.Text;
            buttonVerificarIntegridad.Text = LanguageService.Current?.T("btn_verificar_integridad") ?? buttonVerificarIntegridad.Text;
            buttonRecalcularDV.Text = LanguageService.Current?.T("btn_recalcular_dv") ?? buttonRecalcularDV.Text;

            // Backstop: si llegó acá sin permiso (navegación que no chequeó), no cargar nada.
            if (!SessionContext.Has(Services.Login.TipoPermiso.CONFIGURAR_PARAMETROS.ToString()))
                return;

            try
            {
                _parametros = _parametrosService.Obtener();
                if (_parametros == null) return;

                // Convertir de decimal (0.20) a porcentaje (20.00) para mostrar
                numMargenEmpleados.Value  = _parametros.MargenEmpleados  * 100;
                numMargenMateriales.Value = _parametros.MargenMateriales * 100;
                numUtilidadEmpresa.Value  = _parametros.UtilidadEmpresa  * 100;
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.CONFIGURAR_PARAMETROS))
                return;

            if (_parametros == null) return;

            try
            {
                // Convertir de porcentaje (20.00) a decimal (0.20) para guardar
                _parametros.MargenEmpleados  = numMargenEmpleados.Value  / 100;
                _parametros.MargenMateriales = numMargenMateriales.Value / 100;
                _parametros.UtilidadEmpresa  = numUtilidadEmpresa.Value  / 100;

                _parametrosService.Guardar(_parametros);

                labelStatus.Text      = LanguageService.Current?.T("msg_parametros_guardados") ?? "Parámetros guardados correctamente.";
                labelStatus.ForeColor = System.Drawing.Color.DarkGreen;
            }
            catch (Exception)
            {
                labelStatus.Text      = LanguageService.Current?.T("err_db_generic") ?? "Error al guardar.";
                labelStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.CONFIGURAR_PARAMETROS))
                return;

            using (var form = new BackupForm())
                form.ShowDialog(this);
        }

        private void buttonVerificarIntegridad_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.CONFIGURAR_PARAMETROS))
                return;

            var cap = LanguageService.Current?.T("cap_integridad") ?? "Integridad de datos";
            try
            {
                var anomalias = new IntegridadBL().Verificar();
                if (anomalias.Count == 0)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("msg_integridad_ok") ?? "Integridad OK: no se detectaron manipulaciones.",
                        cap, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var resumen = string.Join(Environment.NewLine,
                    anomalias.GroupBy(a => a.Tabla).Select(g => $"- {g.Key}: {g.Count()}"));

                LoggerLogic.Error($"[Integridad] Verificación manual: {anomalias.Count} inconsistencias. {resumen.Replace(Environment.NewLine, " ")}");

                MessageBox.Show(
                    string.Format(
                        LanguageService.Current?.T("err_integridad_detectada_fmt") ?? "Se detectaron {0} inconsistencias de integridad:",
                        anomalias.Count) + Environment.NewLine + Environment.NewLine + resumen,
                    cap, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[Integridad] Error en verificación manual.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_integridad_generico") ?? "No se pudo verificar la integridad.",
                    cap, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRecalcularDV_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.CONFIGURAR_PARAMETROS))
                return;

            var cap = LanguageService.Current?.T("cap_integridad") ?? "Integridad de datos";

            var confirm = MessageBox.Show(
                LanguageService.Current?.T("msg_recalcular_dv_confirm")
                    ?? "Esto va a recalcular la línea base de dígitos con los datos actuales. Hacelo solo si los cambios externos son legítimos. ¿Continuar?",
                cap, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                new IntegridadBL().RecalcularLineaBase();
                LoggerLogic.Info("[Integridad] Línea base de dígitos verificadores recalculada manualmente.");
                MessageBox.Show(
                    LanguageService.Current?.T("msg_recalcular_dv_ok") ?? "Línea base recalculada.",
                    cap, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[Integridad] Error recalculando línea base.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_integridad_generico") ?? "No se pudo recalcular la línea base.",
                    cap, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
