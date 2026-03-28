using DomainModel.Login;
using Services.Language;
using Services.RoleService;
using Services.RoleService.Logic;
using System;
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
    }
}
