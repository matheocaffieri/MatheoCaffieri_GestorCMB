using Services.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
       



        public Point mouseLocation;
        public VerLogsForm()
        {
            InitializeComponent();
        }

        private void SetupListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("Fecha", 160);
                listView1.Columns.Add("Nivel", 90);
                listView1.Columns.Add("Mensaje", 500);
                listView1.Columns.Add("Excepción", 300);
            }
        }

        private void CargarLogsDesdeSql(string cs, string filtroTexto = null, TraceLevel? nivel = null, int top = 500)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            var sql = new StringBuilder();
            sql.Append($@"
        SELECT TOP ({top}) Fecha, Nivel, Mensaje, Excepcion
        FROM Log
        WHERE 1=1");

            var cmdParams = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(filtroTexto))
            {
                sql.Append(" AND (Mensaje LIKE @q OR Excepcion LIKE @q)");
                cmdParams.Add(new SqlParameter("@q", "%" + filtroTexto + "%"));
            }
            if (nivel.HasValue)
            {
                sql.Append(" AND Nivel = @nivel");
                cmdParams.Add(new SqlParameter("@nivel", nivel.Value.ToString()));
            }

            sql.Append(" ORDER BY Fecha DESC;");

            using (var cn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(sql.ToString(), cn))
            {
                if (cmdParams.Count > 0) cmd.Parameters.AddRange(cmdParams.ToArray());
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var fecha = rd.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        var nivelStr = rd.GetString(1);
                        var msg = rd.IsDBNull(2) ? "" : rd.GetString(2);
                        var ex = rd.IsDBNull(3) ? "" : rd.GetString(3);

                        var it = new ListViewItem(new[] { fecha, nivelStr, msg, ex });
                        // Colorear por nivel
                        switch (nivelStr.ToLowerInvariant())
                        {
                            case "error": it.ForeColor = Color.Firebrick; break;
                            case "warning": it.ForeColor = Color.DarkOrange; break;
                            case "verbose":
                            case "debug": it.ForeColor = Color.DimGray; break;
                            default: it.ForeColor = Color.Black; break;
                        }
                        listView1.Items.Add(it);
                    }
                }
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
                // connectionString desde App.config, ej: ConnLogs
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings["LogsConnection"]?.ConnectionString;
                CargarLogsDesdeSql(cs);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("Descripción breve y clara del contexto", ex);
            }
        }
    }
}
