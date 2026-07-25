using BL;
using DomainModel;
using BL.BL_Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class VerProyectosControl : UserControl
    {
        private MainForm mainForm;
        private const string REQUIRED = "VER_PROYECTOS";

        private enum Orden { MasRecientes, MasAntiguos, DescripcionAZ, ClienteAZ, MasFaltantes }
        private Orden _ordenActual = Orden.MasRecientes;
        private EnumEstado? _estadoFiltro = null;

        private RadioButton _rbMasRecientes;
        private RadioButton _rbMasAntiguos;
        private RadioButton _rbDescripcion;
        private RadioButton _rbCliente;
        private RadioButton _rbMasFaltantes;

        public VerProyectosControl(MainForm mainForm)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                        LanguageService.Current?.T("cap_atencion") ?? "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                host.addUserControl(new HomeControl(mainForm));
                return;
            }

            this.mainForm = mainForm;

            InicializarBuscador();
            InicializarFiltros();

            button1.Click += (_, __) => BuscarConFiltro();
            textBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BuscarConFiltro();
                }
            };

            proyectoItemPanel.Resize += (_, __) => AjustarAnchoItems();
        }

        private void BuscarConFiltro()
        {
            var texto = textBox1.Text.Trim();
            _linkLimpiar.Visible = !string.IsNullOrEmpty(texto);
            CargarListado(texto);
        }

        private LinkLabel _linkLimpiar;

        private void AjustarAnchoItems()
        {
            int panelWidth = proyectoItemPanel.ClientSize.Width;
            if (panelWidth <= 0) return;
            int columnas  = panelWidth >= 1400 ? 3 : panelWidth >= 900 ? 2 : 1;
            // Reservamos el ancho de la barra de scroll vertical para que los ítems nunca
            // queden más anchos que el área visible. Si no, al maximizar el panel dispara una
            // barra horizontal que tapa/anula la vertical y no se puede bajar a ver los ítems.
            int disponible = panelWidth - SystemInformation.VerticalScrollBarWidth - proyectoItemPanel.Padding.Horizontal;
            int itemWidth  = (disponible / columnas) - 10;
            if (itemWidth < 50) itemWidth = 50;
            foreach (Control c in proyectoItemPanel.Controls)
                c.Width = itemWidth;
        }

        private void InicializarBuscador()
        {
            _linkLimpiar = new LinkLabel
            {
                Text = LanguageService.Current?.T("lnk_limpiar") ?? "✕ Limpiar",
                AutoSize = true,
                Location = new Point(155, 16),
                Font = new Font("Microsoft YaHei UI", 8f),
                Visible = false
            };
            _linkLimpiar.LinkClicked += (_, __) =>
            {
                textBox1.Clear();
                _linkLimpiar.Visible = false;
                CargarListado();
                textBox1.Focus();
            };
            panel4.Controls.Add(_linkLimpiar);
        }

        private void InicializarFiltros()
        {
            _rbMasRecientes  = CrearRadio(LanguageService.Current?.T("val_orden_mas_recientes")   ?? "Más recientes",    40,  true);
            _rbMasAntiguos   = CrearRadio(LanguageService.Current?.T("val_orden_mas_antiguos")    ?? "Más antiguos",     70,  false);
            _rbDescripcion   = CrearRadio(LanguageService.Current?.T("val_orden_descripcion_az")  ?? "Descripción A-Z",  100, false);
            _rbCliente       = CrearRadio(LanguageService.Current?.T("val_orden_cliente_az")      ?? "Cliente A-Z",      130, false);
            _rbMasFaltantes  = CrearRadio(LanguageService.Current?.T("val_orden_mas_faltantes")   ?? "Más faltantes",    160, false);

            _rbMasRecientes .CheckedChanged += (_, __) => { if (_rbMasRecientes.Checked)  { _ordenActual = Orden.MasRecientes;  CargarListado(textBox1.Text); } };
            _rbMasAntiguos  .CheckedChanged += (_, __) => { if (_rbMasAntiguos.Checked)   { _ordenActual = Orden.MasAntiguos;   CargarListado(textBox1.Text); } };
            _rbDescripcion  .CheckedChanged += (_, __) => { if (_rbDescripcion.Checked)   { _ordenActual = Orden.DescripcionAZ; CargarListado(textBox1.Text); } };
            _rbCliente      .CheckedChanged += (_, __) => { if (_rbCliente.Checked)       { _ordenActual = Orden.ClienteAZ;     CargarListado(textBox1.Text); } };
            _rbMasFaltantes .CheckedChanged += (_, __) => { if (_rbMasFaltantes.Checked)  { _ordenActual = Orden.MasFaltantes;  CargarListado(textBox1.Text); } };

            panel5.Controls.Add(_rbMasRecientes);
            panel5.Controls.Add(_rbMasAntiguos);
            panel5.Controls.Add(_rbDescripcion);
            panel5.Controls.Add(_rbCliente);
            panel5.Controls.Add(_rbMasFaltantes);

            var separador = new Label
            {
                Text = LanguageService.Current?.T("hdr_estado") ?? "Estado",
                Location = new Point(12, 195),
                AutoSize = true,
                Font = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold),
                ForeColor = Color.DimGray
            };
            panel5.Controls.Add(separador);

            var comboEstado = new ComboBox
            {
                Location = new Point(12, 218),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft YaHei UI", 9f)
            };
            comboEstado.Items.Add(LanguageService.Current?.T("val_estado_todos")       ?? "Todos");
            comboEstado.Items.Add(LanguageService.Current?.T("val_estado_en_proceso")  ?? "En proceso");
            comboEstado.Items.Add(LanguageService.Current?.T("val_estado_suspendido")  ?? "Suspendido");
            comboEstado.Items.Add(LanguageService.Current?.T("val_estado_finalizado")  ?? "Finalizado");
            comboEstado.SelectedIndex = 0;

            comboEstado.SelectedIndexChanged += (_, __) =>
            {
                if (comboEstado.SelectedIndex == 1)      _estadoFiltro = EnumEstado.EnProceso;
                else if (comboEstado.SelectedIndex == 2) _estadoFiltro = EnumEstado.Suspendido;
                else if (comboEstado.SelectedIndex == 3) _estadoFiltro = EnumEstado.Finalizado;
                else                                     _estadoFiltro = null;
                CargarListado(textBox1.Text);
            };

            panel5.Controls.Add(comboEstado);
        }

        private RadioButton CrearRadio(string texto, int top, bool checado)
        {
            return new RadioButton
            {
                Text = texto,
                Location = new Point(12, top),
                AutoSize = true,
                Checked = checado,
                Font = new Font("Microsoft YaHei UI", 9f)
            };
        }

        private void CargarListado(string filtro = "")
        {
            List<Proyecto> todos = ((IProyectoBL)new ProyectoBL()).GetAll();

            var numeros = todos
                .OrderBy(p => p.FechaInicio)
                .Select((p, i) => new { p.IdProyecto, Numero = i + 1 })
                .ToDictionary(x => x.IdProyecto, x => x.Numero);

            var faltantes = new Dictionary<Guid, int>();
            foreach (var p in todos)
            {
                try { faltantes[p.IdProyecto] = new MaterialFaltanteBL().GetAll(p.IdProyecto).Count; }
                catch { faltantes[p.IdProyecto] = 0; }
            }

            List<Proyecto> proyectos = todos;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLower();
                proyectos = proyectos
                    .Where(p => !string.IsNullOrEmpty(p.Descripcion) && p.Descripcion.ToLower().Contains(f))
                    .ToList();
            }

            if (_estadoFiltro.HasValue)
                proyectos = proyectos.Where(p => p.Estado == _estadoFiltro.Value).ToList();

            if (_ordenActual == Orden.MasAntiguos)
                proyectos = proyectos.OrderBy(p => p.FechaInicio).ToList();
            else if (_ordenActual == Orden.DescripcionAZ)
                proyectos = proyectos.OrderBy(p => p.Descripcion).ToList();
            else if (_ordenActual == Orden.ClienteAZ)
                proyectos = proyectos.OrderBy(p => p.Cliente != null ? p.Cliente.RazonSocial : "").ToList();
            else if (_ordenActual == Orden.MasFaltantes)
                proyectos = proyectos.OrderByDescending(p => faltantes.TryGetValue(p.IdProyecto, out var c) ? c : 0).ToList();
            else
                proyectos = proyectos.OrderByDescending(p => p.FechaInicio).ToList();

            proyectoItemPanel.SuspendLayout();
            proyectoItemPanel.Controls.Clear();

            foreach (var p in proyectos)
            {
                var item = new ProyectoItemControl(mainForm, p);
                item.NumProyecto = string.Format(
                    LanguageService.Current?.T("txt_proyecto_num_fmt") ?? "#{0}",
                    numeros[p.IdProyecto]);
                if (faltantes.TryGetValue(p.IdProyecto, out var cnt))
                    item.SetFaltantesCount(cnt);
                proyectoItemPanel.Controls.Add(item);
            }

            proyectoItemPanel.ResumeLayout();
            AjustarAnchoItems();
        }

        private void ObtenerProyectosItems() => CargarListado();

        public void Refrescar() => CargarListado(textBox1.Text);

        private void VerProyectosControl_Load(object sender, EventArgs e)
        {
            AplicarEstilo();
            CargarListado();
        }

        // ===== Estilo visual =====

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private void AplicarEstilo()
        {
            BackColor = Color.FromArgb(245, 246, 250);
            proyectoItemPanel.BackColor = Color.Transparent;
            proyectoItemPanel.Padding   = new Padding(4, 8, 4, 8);

            label1.Font      = new Font("Microsoft YaHei UI", 22f);
            label1.ForeColor = Color.FromArgb(30, 30, 30);

            panel4.BackColor = Color.White;
            AgregarBordeRedondeado(panel4);
            label2.Font      = new Font("Microsoft YaHei UI", 9.5f, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(55, 55, 65);

            panel5.BackColor = Color.White;
            AgregarBordeRedondeado(panel5);
            label3.Font      = new Font("Microsoft YaHei UI", 9.5f, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(55, 55, 65);

            textBox1.Font        = new Font("Microsoft YaHei UI", 9.5f);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            SendMessage(textBox1.Handle, 0x1501, 1, LanguageService.Current?.T("txt_buscar_placeholder") ?? "Buscar...");

            button1.Text     = "›";
            button1.Font     = new Font("Microsoft YaHei UI", 16f, FontStyle.Bold);
            button1.BackColor = Color.FromArgb(76, 175, 80);
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize             = 0;
            button1.FlatAppearance.MouseOverBackColor     = Color.FromArgb(56, 142, 60);
            button1.Cursor   = Cursors.Hand;

            foreach (var rb in new[] { _rbMasRecientes, _rbMasAntiguos, _rbDescripcion, _rbCliente, _rbMasFaltantes })
                if (rb != null) rb.Font = new Font("Microsoft YaHei UI", 8.5f);
        }

        private static void AgregarBordeRedondeado(Panel panel)
        {
            panel.Paint += (s, ev) =>
            {
                var p = (Panel)s;
                ev.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeRoundRectPath(new Rectangle(0, 0, p.Width - 1, p.Height - 1), 8))
                using (var pen  = new Pen(Color.FromArgb(218, 218, 225)))
                    ev.Graphics.DrawPath(pen, path);
            };
        }

        private static GraphicsPath MakeRoundRectPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X,                  r.Y,                   radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y,                   radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2,   0, 90);
            path.AddArc(r.X,                  r.Bottom - radius * 2, radius * 2, radius * 2,  90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
