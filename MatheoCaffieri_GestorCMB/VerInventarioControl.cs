using BL;
using DomainModel;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.Logs;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class VerInventarioControl : UserControl
    {
        private readonly IGenericRepository<Inventario> _invRepo = new InventarioBL();
        private readonly InformeDeCompraBL _informesBL = new InformeDeCompraBL();
        private readonly MainForm _mainForm;

        private const string REQUIRED = "VER_INVENTARIO";

        private Panel _scrollArea;
        private Label _lblCount;

        public VerInventarioControl()
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = _mainForm ?? (this.FindForm() as MainForm);
                host?.addUserControl(new HomeControl(_mainForm));
                return;
            }
        }

        // ══════════════════════════════════════════════════════════
        // LOAD
        // ══════════════════════════════════════════════════════════

        private void VerInventarioControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            BuildUI();
            BeginInvoke((Action)(() => CargarListado()));
        }

        // ══════════════════════════════════════════════════════════
        // BUILD UI
        // ══════════════════════════════════════════════════════════

        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor      = Color.White;
            this.DoubleBuffered = true;

            // ── HEADER ────────────────────────────────────────────
            var header = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 58,
                BackColor = Color.White,
            };

            var lblTitle = new Label
            {
                Text      = LanguageService.Current?.T("hdr_inventario") ?? "Inventario",
                Font      = new Font("Microsoft YaHei UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 22, 28),
                AutoSize  = true,
                Location  = new Point(20, 16),
                BackColor = Color.Transparent,
            };

            var newInformes = MakeNavBtn(LanguageService.Current?.T("btn_informe_compra") ?? "Informe de compra");
            newInformes.Click += buttonVerInformesCompra_Click;

            var newProveedores = MakeNavBtn(LanguageService.Current?.T("btn_proveedores") ?? "Proveedores");
            newProveedores.Click += buttonGestionarProveedores_Click;

            header.Controls.Add(lblTitle);
            header.Controls.Add(newInformes);
            header.Controls.Add(newProveedores);

            header.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                if (p.Width < 100) return;
                newProveedores.Location = new Point(p.Width - newProveedores.Width - 20, (p.Height - newProveedores.Height) / 2);
                newInformes.Location    = new Point(newProveedores.Left - newInformes.Width - 8, (p.Height - newInformes.Height) / 2);
            };

            // ── SECTION BAR ───────────────────────────────────────
            var sectionBar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 40,
                BackColor = Color.White,
            };

            var lblSec = new Label
            {
                Text      = LanguageService.Current?.T("hdr_materiales") ?? "Materiales",
                Font      = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(48, 48, 58),
                AutoSize  = true,
                Location  = new Point(20, 10),
                BackColor = Color.Transparent,
            };

            _lblCount = new Label
            {
                Text      = "",
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.FromArgb(155, 155, 168),
                AutoSize  = true,
                Location  = new Point(120, 12),
                BackColor = Color.Transparent,
            };

            var newSearch = new TextBox
            {
                Font        = new Font("Microsoft YaHei UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                Width       = 220,
            };

            var newAddBtn = new Button
            {
                Text      = LanguageService.Current?.T("btn_agregar_material") ?? "+ Agregar material",
                Height    = 28,
                Width     = 148,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
            };
            newAddBtn.FlatAppearance.BorderSize = 0;
            newAddBtn.Click += buttonAgregarMaterial_Click;

            sectionBar.Controls.Add(_lblCount);
            sectionBar.Controls.Add(lblSec);
            sectionBar.Controls.Add(newSearch);
            sectionBar.Controls.Add(newAddBtn);

            sectionBar.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                if (p.Width < 100) return;
                newAddBtn.Location = new Point(p.Width - newAddBtn.Width - 20, (p.Height - newAddBtn.Height) / 2);
                newSearch.Location = new Point(newAddBtn.Left - newSearch.Width - 8, (p.Height - newSearch.Height) / 2 + 1);
                _lblCount.Location = new Point(lblSec.Right + 6, 12);
            };

            var sepH = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 1,
                BackColor = Color.FromArgb(230, 230, 236),
            };

            // ── SCROLL + GRID ─────────────────────────────────────
            _scrollArea = new Panel
            {
                Dock       = DockStyle.Fill,
                AutoScroll = true,
                BackColor  = Color.FromArgb(247, 248, 250),
                Padding    = new Padding(16, 14, 16, 14),
            };

            var newGrid = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoSize      = true,
                AutoSizeMode  = AutoSizeMode.GrowAndShrink,
                BackColor     = Color.Transparent,
            };

            _scrollArea.Controls.Add(newGrid);
            _scrollArea.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                int w = Math.Max(1, p.ClientSize.Width - newGrid.Margin.Horizontal);
                newGrid.Width = w;
                foreach (InventarioItemControl c in newGrid.Controls.OfType<InventarioItemControl>())
                    c.Width = w;
            };

            // Reasignar campos del Designer
            textBox1          = newSearch;
            MaterialesItemPanel = newGrid;
            buttonAgregarMaterial        = newAddBtn;
            buttonVerInformesCompra      = newInformes;
            buttonGestionarProveedores   = newProveedores;

            // ── ASSEMBLE ──────────────────────────────────────────
            this.Controls.Add(_scrollArea);
            this.Controls.Add(sepH);
            this.Controls.Add(sectionBar);
            this.Controls.Add(header);

            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyDown     += textBox1_KeyDown;
        }

        private static Button MakeNavBtn(string text)
        {
            var btn = new Button
            {
                Text      = text,
                Height    = 28,
                AutoSize  = false,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(70, 100, 160),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                Cursor    = Cursors.Hand,
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(190, 210, 240);
            btn.FlatAppearance.BorderSize  = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 245, 255);

            // Ancho dinámico según texto
            btn.Width = TextRenderer.MeasureText(text, btn.Font).Width + 24;

            return btn;
        }

        // ══════════════════════════════════════════════════════════
        // LISTADO
        // ══════════════════════════════════════════════════════════

        private void CargarListado(string filtro = "")
        {
            List<Inventario> materiales = _invRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();
                materiales = materiales.Where(e =>
                    (e.Material?.DescripcionArticulo?.ToLower().Contains(filtro) == true) ||
                    (e.Material?.TipoMaterial?.ToLower().Contains(filtro) == true)        ||
                    (e.Material?.TipoUnidad?.ToLower().Contains(filtro) == true)          ||
                    (e.Material?.Proveedor?.Descripcion?.ToLower().Contains(filtro) == true) ||
                    e.Cantidad.ToString().Contains(filtro)
                ).ToList();
            }

            if (_lblCount != null)
                _lblCount.Text = $"({materiales.Count})";

            HashSet<Guid> materialesConPendiente;
            try
            {
                materialesConPendiente = _informesBL.GetMaterialesConInformesPendientes();
            }
            catch (Exception ex)
            {
                LoggerLogic.Warn($"[VerInventarioControl] No se pudo cargar el set de informes pendientes: {ex.Message}");
                materialesConPendiente = new HashSet<Guid>();
            }

            MaterialesItemPanel.SuspendLayout();
            MaterialesItemPanel.Controls.Clear();

            int cardW = CardWidth();
            foreach (var mat in materiales)
            {
                var item = new InventarioItemControl { Width = cardW };
                item.Bind(mat);
                item.SetInformePendiente(mat.Material != null && materialesConPendiente.Contains(mat.Material.IdMaterial));
                item.InformePendienteClicked += buttonVerInformesCompra_Click;
                item.EditRequested += (s, inv) =>
                {
                    using (var form = new AddMaterialesForm(inv.Material))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog(this) == DialogResult.OK)
                            CargarListado(textBox1.Text);
                    }
                };
                item.Deleted += (s, ev) => CargarListado(textBox1.Text);
                MaterialesItemPanel.Controls.Add(item);
            }

            MaterialesItemPanel.ResumeLayout();
        }

        private int CardWidth()
        {
            if (_scrollArea == null) return 600;
            int avail = _scrollArea.ClientSize.Width - 32;
            return Math.Max(200, avail);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (MaterialesItemPanel == null) return;
            int w = CardWidth();
            foreach (InventarioItemControl c in MaterialesItemPanel.Controls.OfType<InventarioItemControl>())
                c.Width = w;
        }

        // ══════════════════════════════════════════════════════════
        // EVENTOS
        // ══════════════════════════════════════════════════════════

        private void textBox1_TextChanged(object sender, EventArgs e) => CargarListado(textBox1.Text);

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            CargarListado(textBox1.Text);
        }

        private void buttonAgregarMaterial_Click(object sender, EventArgs e)
        {
            using (var form = new AddMaterialesForm())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(this) == DialogResult.OK)
                    CargarListado(textBox1.Text);
            }
        }

        private void buttonGestionarProveedores_Click(object sender, EventArgs e)
        {
            if (!SessionContext.Has("GESTIONAR_PROVEEDORES"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            (_mainForm ?? this.FindForm() as MainForm)?.addUserControl(new ProveedorControl());
        }

        private void buttonVerInformesCompra_Click(object sender, EventArgs e)
        {
            if (!SessionContext.Has("VER_INFORMES_COMPRA"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            (_mainForm ?? this.FindForm() as MainForm)?.addUserControl(new InformesDeCompraControl());
        }
    }
}
