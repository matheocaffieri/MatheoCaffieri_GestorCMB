using Services.Language;
using Services.Logs;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class VerLogsForm : Form
    {

        private const string REQUIRED = "VER_LOGS";

        // Se pone en true al terminar el Load. Evita que los eventos de los
        // filtros disparen consultas antes de que el ListView esté armado.
        private bool _listo = false;

        public Point mouseLocation;
        public VerLogsForm()
        {
            InitializeComponent();


            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            AplicarTraducciones();
        }

        private void AplicarTraducciones()
        {
            var t = LanguageService.Current;

            buttonArchivo.Text = t?.T("btn_ver_archivo") ?? buttonArchivo.Text;
            labelBuscar.Text = t?.T("lbl_buscar") ?? labelBuscar.Text;
            labelNivel.Text = t?.T("lbl_nivel") ?? labelNivel.Text;
            buttonBuscar.Text = t?.T("btn_buscar") ?? buttonBuscar.Text;

            // Poblar el combo de niveles. El orden es fijo: el índice se mapea a
            // TraceLevel en NivelSeleccionado(), así la traducción no afecta el filtro.
            comboNivel.Items.Clear();
            comboNivel.Items.Add(t?.T("cbo_nivel_todos") ?? "Todos"); // 0 -> null (todos)
            comboNivel.Items.Add("Info");                             // 1 -> Info
            comboNivel.Items.Add("Warning");                          // 2 -> Warning
            comboNivel.Items.Add("Error");                            // 3 -> Error
            comboNivel.SelectedIndex = 0;
        }

        private void SetupListView()
        {
            var t = LanguageService.Current;

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add(t?.T("hdr_log_fecha") ?? "Fecha", 160);
                listView1.Columns.Add(t?.T("hdr_log_nivel") ?? "Nivel", 90);
                listView1.Columns.Add(t?.T("hdr_log_mensaje") ?? "Mensaje", 500);
                listView1.Columns.Add(t?.T("hdr_log_excepcion") ?? "Excepción", 300);
            }
        }

        // Traduce el índice seleccionado del combo a un TraceLevel? (null = todos).
        private TraceLevel? NivelSeleccionado()
        {
            switch (comboNivel.SelectedIndex)
            {
                case 1: return TraceLevel.Info;
                case 2: return TraceLevel.Warning;
                case 3: return TraceLevel.Error;
                default: return null; // 0 o sin selección => todos
            }
        }

        // Relee los logs aplicando el texto buscado y el nivel elegido.
        private void AplicarFiltros()
        {
            if (!_listo) return;

            try
            {
                var texto = string.IsNullOrWhiteSpace(textBoxBuscar.Text) ? null : textBoxBuscar.Text.Trim();
                CargarLogs(texto, NivelSeleccionado());
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[VerLogsForm] Error aplicando filtros de logs.", ex);
            }
        }

        private void CargarLogs(string filtroTexto = null, TraceLevel? nivel = null, int top = 500)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            foreach (var log in LoggerLogic.Leer(filtroTexto, nivel, top))
            {
                var it = new ListViewItem(new[]
                {
                    log.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.Nivel,
                    log.Mensaje ?? "",
                    log.Excepcion ?? ""
                });
                // Colorear por nivel
                switch ((log.Nivel ?? "").ToLowerInvariant())
                {
                    case "error": it.ForeColor = Color.Firebrick; break;
                    case "warning": it.ForeColor = Color.DarkOrange; break;
                    case "verbose":
                    case "debug": it.ForeColor = Color.DimGray; break;
                    default: it.ForeColor = Color.Black; break;
                }
                listView1.Items.Add(it);
            }

            listView1.EndUpdate();
        }


        private void buttonExitAM_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void FormPanel_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }

        private void FormPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonArchivo_Click(object sender, EventArgs e)
        {
            // 1) Tomar carpeta de logs desde App.config
            var dirSetting = ConfigurationManager.AppSettings["LogDirectory"];
            var dir = string.IsNullOrWhiteSpace(dirSetting) ? "logs" : Environment.ExpandEnvironmentVariables(dirSetting);

            // Si es relativa, la hacemos relativa al EXE
            if (!Path.IsPathRooted(dir))
                dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);

            // 2) Si no existe, avisar
            if (!Directory.Exists(dir))
            {
                MessageBox.Show(
                    $"No existe el directorio de logs:\n{dir}\n\nConfiguralo en App.config (LogDirectory).",
                    "Logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // 3) Buscar el archivo .log más reciente
            var last = Directory.GetFiles(dir, "*.log")
                .Select(f => new FileInfo(f))
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            // 4) Si no hay logs, abrir igual la carpeta
            if (last == null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"\"{dir}\"",
                    UseShellExecute = true
                });

                MessageBox.Show(
                    $"No se encontraron archivos .log en:\n{dir}\n\nSe abrió la carpeta igualmente.",
                    "Logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // 5) Abrir el explorador seleccionando el archivo
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{last.FullName}\"",
                UseShellExecute = true
            });
        }

        private void VerLogsForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetupListView();
                CargarLogs();
                _listo = true;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[VerLogsForm] Error cargando logs desde SQL.", ex);
            }
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void textBoxBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita el "ding" del sistema
                AplicarFiltros();
            }
        }

        private void comboNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }
    }
}
