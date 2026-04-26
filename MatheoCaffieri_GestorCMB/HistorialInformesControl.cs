using BL;
using DomainModel;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Historial;
using Services.Language;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class HistorialInformesControl : UserControl
    {
        private MainForm _mainForm;
        private Panel _panelLista;

        public HistorialInformesControl()
        {
            InitializeComponent();
        }

        public HistorialInformesControl(MainForm mainForm) : this()
        {
            _mainForm = mainForm;
        }

        private void HistorialInformesControl_Load(object sender, EventArgs e)
        {
            AplicarEstilo();
            CrearPanelLista();

            buttonBuscar.Click += (s, ev) => CargarHistorial(textBoxBuscar.Text);
            textBoxBuscar.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    ev.SuppressKeyPress = true;
                    CargarHistorial(textBoxBuscar.Text);
                }
            };

            historialLayoutPanel.Resize += (_, __) => AjustarAnchoItems();
            Resize += (_, __) =>
            {
                AjustarLayoutResponsive();
                AjustarAnchoItems();
            };

            AjustarLayoutResponsive();
            CargarHistorial();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private void AplicarEstilo()
        {
            BackColor = Color.FromArgb(245, 246, 250);
            historialLayoutPanel.BackColor = Color.Transparent;
            historialLayoutPanel.Padding = new Padding(14, 10, 14, 10);

            labelTitulo.Font = new Font("Microsoft YaHei UI", 16f, FontStyle.Bold);
            labelTitulo.ForeColor = Color.FromArgb(30, 30, 30);

            buttonBack.Text = "«";
            buttonBack.Font = new Font("Microsoft YaHei UI", 12f, FontStyle.Bold);
            buttonBack.FlatStyle = FlatStyle.Flat;
            buttonBack.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 210);
            buttonBack.BackColor = Color.White;
            buttonBack.Cursor = Cursors.Hand;

            textBoxBuscar.Font = new Font("Microsoft YaHei UI", 9.5f);
            textBoxBuscar.BorderStyle = BorderStyle.FixedSingle;
            SendMessage(textBoxBuscar.Handle, 0x1501, 1, "Buscar...");

            buttonBuscar.Text = "›";
            buttonBuscar.Font = new Font("Microsoft YaHei UI", 16f, FontStyle.Bold);
            buttonBuscar.BackColor = Color.FromArgb(76, 175, 80);
            buttonBuscar.ForeColor = Color.White;
            buttonBuscar.FlatStyle = FlatStyle.Flat;
            buttonBuscar.FlatAppearance.BorderSize = 0;
            buttonBuscar.FlatAppearance.MouseOverBackColor = Color.FromArgb(56, 142, 60);
            buttonBuscar.Cursor = Cursors.Hand;
        }

        private void CrearPanelLista()
        {
            if (_panelLista != null) return;

            _panelLista = new Panel
            {
                BackColor = Color.FromArgb(236, 238, 243),
                Padding = new Padding(10),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            Controls.Remove(historialLayoutPanel);
            _panelLista.Controls.Add(historialLayoutPanel);
            Controls.Add(_panelLista);
            _panelLista.BringToFront();

            _panelLista.Resize += (_, __) =>
            {
                if (_panelLista.Width > 4 && _panelLista.Height > 4)
                {
                    using (var path = MakeRoundRect(new Rectangle(0, 0, _panelLista.Width - 1, _panelLista.Height - 1), 10))
                        _panelLista.Region = new Region(path);
                }
            };

            historialLayoutPanel.Dock = DockStyle.Fill;
            historialLayoutPanel.WrapContents = false;
            historialLayoutPanel.FlowDirection = FlowDirection.TopDown;
            historialLayoutPanel.AutoScroll = true;
        }

        private void AjustarLayoutResponsive()
        {
            int left = 18;
            int right = 18;
            int top = 16;
            int gap = 10;
            bool compact = Width < 920;

            buttonBack.SetBounds(left, top, 42, 34);

            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Microsoft YaHei UI", compact ? 14f : 16f, FontStyle.Bold);
            labelTitulo.Location = new Point(buttonBack.Right + 8, top + (compact ? 2 : 0));

            int buscarW = compact ? 58 : 64;
            int buscarH = 34;
            int buscarX = Width - right - buscarW;
            buttonBuscar.SetBounds(buscarX, top, buscarW, buscarH);

            int textX = compact ? left : Math.Max(labelTitulo.Right + 18, 360);
            int textY = compact ? (buttonBack.Bottom + 8) : top;
            int textW = Math.Max(220, buscarX - gap - textX);
            textBoxBuscar.SetBounds(textX, textY, textW, 34);

            int listaTop = compact ? (textBoxBuscar.Bottom + 12) : (buttonBack.Bottom + 14);
            _panelLista.SetBounds(left, listaTop, Width - left - right, Height - listaTop - 12);
        }

        private static GraphicsPath MakeRoundRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AjustarAnchoItems()
        {
            int w = historialLayoutPanel.ClientSize.Width;
            if (w <= 0) return;

            int cols = w >= 1150 ? 2 : 1;
            int gap = 12;
            int itemW = (w - ((cols - 1) * gap)) / cols;

            foreach (Control c in historialLayoutPanel.Controls)
                c.Width = Math.Max(280, itemW);
        }

        private void CargarHistorial(string filtro = null)
        {
            historialLayoutPanel.Controls.Clear();

            List<InformeDeCompra> historial;
            try
            {
                historial = new InformeDeCompraBL().GetHistorial() ?? new List<InformeDeCompra>();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("HistorialInformesControl.CargarHistorial_FAIL", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (historial.Count == 0) return;

            var proyectoBL = new ProyectoBL();
            var detInfBL = new DetalleInformeCompraBL();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLowerInvariant();
                var proyectosMatch = historial
                    .Select(x => x.IdProyecto)
                    .Distinct()
                    .Select(id => proyectoBL.GetById(id))
                    .Where(p => p != null && (
                        (!string.IsNullOrWhiteSpace(p.Descripcion) && p.Descripcion.ToLowerInvariant().Contains(f)) ||
                        (p.Cliente != null && !string.IsNullOrWhiteSpace(p.Cliente.RazonSocial) && p.Cliente.RazonSocial.ToLowerInvariant().Contains(f))
                    ))
                    .Select(p => p.IdProyecto)
                    .ToHashSet();

                historial = historial.Where(x => proyectosMatch.Contains(x.IdProyecto)).ToList();
            }

            foreach (var inf in historial)
            {
                var proyecto = proyectoBL.GetById(inf.IdProyecto);
                if (proyecto == null) continue;

                List<MaterialFaltante> faltantes = null;
                if (inf.Estado == "cancelado")
                    faltantes = detInfBL.GetMaterialesFaltantesDelInforme(inf.IdInformeCompra);
                else if (inf.Estado == "finalizado")
                    faltantes = SnapshotService.Leer(inf.IdInformeCompra);

                var item = new HistorialInformeItemControl();
                item.Bind(
                    fecha: inf.FechaRealizacion.ToString("dd/MM/yyyy"),
                    nombreProyecto: proyecto.Descripcion ?? "Proyecto sin nombre",
                    estado: inf.Estado,
                    faltantes: faltantes
                );

                historialLayoutPanel.Controls.Add(item);
            }

            AjustarAnchoItems();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                    LanguageService.Current?.T("cap_atencion") ?? "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new InformesDeCompraControl(_mainForm));
        }
    }
}
