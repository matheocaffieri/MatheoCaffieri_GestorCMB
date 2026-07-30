using Services.Backup;
using Services.Language;
using Services.Login;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class BackupForm : Form
    {
        private readonly BackupService _backupService = new BackupService();

        public BackupForm()
        {
            InitializeComponent();
            AplicarTraducciones();
            CargarBases();
            textBoxDestino.Text = _backupService.CarpetaDestinoPorDefecto();
        }

        private void AplicarTraducciones()
        {
            var t = LanguageService.Current;
            this.Text            = t?.T("lbl_backup_titulo")     ?? this.Text;
            labelTitulo.Text     = t?.T("lbl_backup_titulo")     ?? labelTitulo.Text;
            labelSeleccione.Text = t?.T("lbl_backup_seleccione") ?? labelSeleccione.Text;
            labelDestino.Text    = t?.T("lbl_backup_destino")    ?? labelDestino.Text;
            buttonExaminar.Text  = t?.T("btn_examinar")          ?? buttonExaminar.Text;
            buttonGenerar.Text   = t?.T("btn_backup_generar")    ?? buttonGenerar.Text;
            buttonCerrar.Text    = t?.T("btn_cerrar")            ?? buttonCerrar.Text;
        }

        private void CargarBases()
        {
            checkedListBoxBases.Items.Clear();
            foreach (var db in _backupService.DatabasesDisponibles())
                checkedListBoxBases.Items.Add(db.Catalogo, true); // tildadas por defecto
        }

        private void buttonExaminar_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(textBoxDestino.Text))
                    dlg.SelectedPath = textBoxDestino.Text;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                    textBoxDestino.Text = dlg.SelectedPath;
            }
        }

        private void buttonGenerar_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(TipoPermiso.CONFIGURAR_PARAMETROS))
                return;

            var t = LanguageService.Current;

            var seleccionadas = checkedListBoxBases.CheckedItems.Cast<string>().ToList();
            if (seleccionadas.Count == 0)
            {
                MostrarStatus(t?.T("val_backup_sin_seleccion") ?? "Seleccioná al menos una base.", System.Drawing.Color.Red);
                return;
            }

            var destino = textBoxDestino.Text?.Trim();
            if (string.IsNullOrWhiteSpace(destino))
            {
                MostrarStatus(t?.T("val_backup_sin_destino") ?? "Elegí una carpeta destino.", System.Drawing.Color.Red);
                return;
            }

            Cursor = Cursors.WaitCursor;
            buttonGenerar.Enabled = false;
            try
            {
                var results = _backupService.Generar(seleccionadas, destino);

                var sb = new StringBuilder();
                var ok = 0;
                foreach (var r in results)
                {
                    if (r.Ok)
                    {
                        ok++;
                        sb.AppendLine($"OK  {r.Base}  ->  {r.RutaArchivo}");
                    }
                    else
                    {
                        sb.AppendLine(string.Format(t?.T("err_backup_fmt") ?? "Error al copiar {0}: {1}", r.Base, r.Error));
                    }
                }
                textBoxResultado.Text = sb.ToString();

                var todoOk = ok == results.Count;
                MostrarStatus(
                    string.Format(t?.T("msg_backup_completado_fmt") ?? "Backup completado: {0} de {1} bases.", ok, results.Count),
                    todoOk ? System.Drawing.Color.DarkGreen : System.Drawing.Color.DarkOrange);
            }
            catch (Exception ex)
            {
                Services.Logs.LoggerLogic.Error("[BackupForm] Error generando backups.", ex);
                MostrarStatus(t?.T("err_backup_generico") ?? "No se pudo generar la copia.", System.Drawing.Color.Red);
            }
            finally
            {
                Cursor = Cursors.Default;
                buttonGenerar.Enabled = true;
            }
        }

        private void MostrarStatus(string texto, System.Drawing.Color color)
        {
            labelStatus.Text = texto;
            labelStatus.ForeColor = color;
        }

        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
