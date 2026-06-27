using BL;
using DomainModel;
using DomainModel.Interfaces;
using DomainModel.Login;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class ProveedorControl : UserControl
    {
        public ProveedorControl() : this(new ProveedorBL())
        {
        }

        private const string REQUIRED = "GESTIONAR_PROVEEDORES";

        private readonly MainForm _mainForm;

        public ProveedorControl(IGenericRepository<Proveedor> proveedorRepo)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = _mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                        LanguageService.Current?.T("cap_atencion") ?? "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                host.addUserControl(new HomeControl(_mainForm));
                return;
            }

            _proveedorRepo = proveedorRepo ?? throw new ArgumentNullException(nameof(proveedorRepo));
        }

        private readonly IGenericRepository<Proveedor> _proveedorRepo;
        private Label _lblCount;
        private Panel _scrollArea;

        // ══════════════════════════════════════════════════════════
        // LOAD
        // ══════════════════════════════════════════════════════════

        private void ProveedorControl_Load(object sender, EventArgs e)
        {
            if (DesignMode || _proveedorRepo == null) return;
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

            var btnBack = MakeNavBtn(LanguageService.Current?.T("btn_volver") ?? "← Volver");
            btnBack.Location  = new Point(16, 15);
            btnBack.Click    += buttonBack_Click;

            var lblTitle = new Label
            {
                Text      = LanguageService.Current?.T("hdr_proveedores") ?? "Proveedores",
                Font      = new Font("Microsoft YaHei UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 22, 28),
                AutoSize  = true,
                Location  = new Point(btnBack.Right + 16, 16),
                BackColor = Color.Transparent,
            };

            header.Controls.Add(btnBack);
            header.Controls.Add(lblTitle);

            // ── CONTENT ───────────────────────────────────────────
            var contentPanel = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.White,
            };

            // ── SIDEBAR (formulario de alta) ───────────────────────
            var sidebar = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 244,
                BackColor = Color.White,
            };

            var lblDesc = new Label
            {
                Text      = LanguageService.Current?.T("lbl_descripcion") ?? "Descripción",
                Font      = new Font("Microsoft YaHei UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 90),
                AutoSize  = true,
                Location  = new Point(16, 20),
                BackColor = Color.Transparent,
            };

            var newTxDesc = new TextBox
            {
                Font        = new Font("Microsoft YaHei UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                Location    = new Point(16, 42),
                Width       = 210,
            };

            var lblTel = new Label
            {
                Text      = LanguageService.Current?.T("lbl_telefono") ?? "Teléfono",
                Font      = new Font("Microsoft YaHei UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 90),
                AutoSize  = true,
                Location  = new Point(16, 80),
                BackColor = Color.Transparent,
            };

            var newTxTel = new TextBox
            {
                Font        = new Font("Microsoft YaHei UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                Location    = new Point(16, 102),
                Width       = 210,
            };

            var newAddBtn = new Button
            {
                Text      = LanguageService.Current?.T("btn_agregar") ?? "+ Agregar",
                Location  = new Point(16, 146),
                Height    = 30,
                Width     = 210,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
            };
            newAddBtn.FlatAppearance.BorderSize             = 0;
            newAddBtn.FlatAppearance.MouseOverBackColor     = Color.FromArgb(56, 155, 60);

            sidebar.Controls.Add(lblDesc);
            sidebar.Controls.Add(newTxDesc);
            sidebar.Controls.Add(lblTel);
            sidebar.Controls.Add(newTxTel);
            sidebar.Controls.Add(newAddBtn);

            // ── DIVIDER ───────────────────────────────────────────
            var divider = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 1,
                BackColor = Color.FromArgb(230, 230, 236),
            };

            // ── RIGHT ─────────────────────────────────────────────
            var rightPanel = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.White,
            };

            var sectionBar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 40,
                BackColor = Color.White,
            };

            var lblSec = new Label
            {
                Text      = LanguageService.Current?.T("hdr_lista") ?? "Lista",
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
                Location  = new Point(60, 12),
                BackColor = Color.Transparent,
            };

            var newSearch = new TextBox
            {
                Font        = new Font("Microsoft YaHei UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                Width       = 200,
            };

            var newSearchBtn = new Button
            {
                Text      = LanguageService.Current?.T("btn_buscar") ?? "Buscar",
                Height    = 28,
                Width     = 70,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(70, 100, 160),
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                Cursor    = Cursors.Hand,
            };
            newSearchBtn.FlatAppearance.BorderColor = Color.FromArgb(190, 210, 240);
            newSearchBtn.FlatAppearance.BorderSize  = 1;

            sectionBar.Controls.Add(_lblCount);
            sectionBar.Controls.Add(lblSec);
            sectionBar.Controls.Add(newSearch);
            sectionBar.Controls.Add(newSearchBtn);

            sectionBar.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                if (p.Width < 100) return;
                newSearchBtn.Location = new Point(p.Width - newSearchBtn.Width - 20, (p.Height - newSearchBtn.Height) / 2);
                newSearch.Location    = new Point(newSearchBtn.Left - newSearch.Width - 6, (p.Height - newSearch.Height) / 2 + 1);
                _lblCount.Location    = new Point(lblSec.Right + 6, 12);
            };

            var sepH = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 1,
                BackColor = Color.FromArgb(230, 230, 236),
            };

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
                foreach (ProveedorItemControl c in newGrid.Controls.OfType<ProveedorItemControl>())
                    c.Width = w;
            };

            rightPanel.Controls.Add(_scrollArea);
            rightPanel.Controls.Add(sepH);
            rightPanel.Controls.Add(sectionBar);

            contentPanel.Controls.Add(rightPanel);
            contentPanel.Controls.Add(divider);
            contentPanel.Controls.Add(sidebar);

            // Reasignar campos del Designer
            textBoxDescripcion   = newTxDesc;
            textBoxTelefono      = newTxTel;
            buttonAddProveedor   = newAddBtn;
            textBox1             = newSearch;
            buttonSearchClientes = newSearchBtn;
            proveedorLayoutPanel = newGrid;

            // ── ASSEMBLE ──────────────────────────────────────────
            this.Controls.Add(contentPanel);
            this.Controls.Add(header);

            newAddBtn.Click       += buttonAddProveedor_Click;
            newSearchBtn.Click    += buttonSearchClientes_Click;
            newSearch.TextChanged += textBox1_TextChanged;
            newSearch.KeyDown     += textBox1_KeyDown;
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
            btn.FlatAppearance.BorderColor            = Color.FromArgb(190, 210, 240);
            btn.FlatAppearance.BorderSize             = 1;
            btn.FlatAppearance.MouseOverBackColor     = Color.FromArgb(240, 245, 255);
            btn.Width = TextRenderer.MeasureText(text, btn.Font).Width + 24;
            return btn;
        }

        // ══════════════════════════════════════════════════════════
        // LISTADO
        // ══════════════════════════════════════════════════════════

        private void CargarListado(string filtro = "")
        {
            List<Proveedor> proveedores = _proveedorRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();
                proveedores = proveedores
                    .Where(p =>
                        (!string.IsNullOrEmpty(p.Descripcion) && p.Descripcion.ToLower().Contains(filtro)) ||
                        p.Telefono.ToString().Contains(filtro)
                    )
                    .ToList();
            }

            if (_lblCount != null)
                _lblCount.Text = $"({proveedores.Count})";

            proveedorLayoutPanel.SuspendLayout();
            proveedorLayoutPanel.Controls.Clear();

            int cardW = CardWidth();
            foreach (var proveedor in proveedores)
            {
                var item = new ProveedorItemControl { Width = cardW };
                item.Bind(proveedor);
                item.EditRequested += EditarProveedor;
                item.ActiveChanged += ToggleActivoProveedor;
                proveedorLayoutPanel.Controls.Add(item);
            }

            proveedorLayoutPanel.ResumeLayout();
        }

        private int CardWidth()
        {
            if (_scrollArea == null) return 500;
            int avail = _scrollArea.ClientSize.Width - 32;
            return Math.Max(200, avail);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (proveedorLayoutPanel == null) return;
            int w = CardWidth();
            foreach (ProveedorItemControl c in proveedorLayoutPanel.Controls.OfType<ProveedorItemControl>())
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

        private void buttonSearchClientes_Click(object sender, EventArgs e) => CargarListado(textBox1.Text);

        private void buttonAddProveedor_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_PROVEEDORES))
                return;

            string descripcion = textBoxDescripcion.Text.Trim();
            string telefonoStr = textBoxTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_requerida") ?? "La descripción es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // El teléfono es opcional: en la DB la columna es int NOT NULL, así que "sin teléfono" se guarda como 0.
            int telefono = 0;
            if (!string.IsNullOrWhiteSpace(telefonoStr) && !int.TryParse(telefonoStr, out telefono))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoProveedor = new Proveedor
            {
                Descripcion = descripcion,
                Telefono    = telefono,
                IsActive    = true,
            };

            try
            {
                _proveedorRepo.Add(nuevoProveedor);

                textBoxDescripcion.Clear();
                textBoxTelefono.Clear();
                textBoxDescripcion.Focus();

                CargarListado(textBox1.Text);
            }
            catch (DomainModel.Exceptions.AppException ex)
            {
                Services.Logs.LoggerLogic.Warn($"[ProveedorControl] Validación al crear proveedor: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Services.Logs.LoggerLogic.Error("[ProveedorControl] Falla al crear proveedor.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleActivoProveedor(Proveedor proveedor, bool nuevoEstado)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_PROVEEDORES))
            {
                CargarListado(textBox1.Text); // revertir el switch al estado real
                return;
            }

            try
            {
                proveedor.IsActive = nuevoEstado;
                _proveedorRepo.Update(proveedor);
            }
            catch (DomainModel.Exceptions.AppException ex)
            {
                Services.Logs.LoggerLogic.Warn($"[ProveedorControl] Validación al cambiar estado de proveedor: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarListado(textBox1.Text);
            }
            catch (Exception ex)
            {
                Services.Logs.LoggerLogic.Error($"[ProveedorControl] Falla al cambiar estado de proveedor. Id={proveedor.IdProveedor}", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarListado(textBox1.Text);
            }
        }

        private void EditarProveedor(DomainModel.Proveedor proveedor)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_PROVEEDORES))
                return;

            using (var frm = new EditProveedorForm(_proveedorRepo, proveedor))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                    CargarListado(textBox1.Text);
            }
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

            host.addUserControl(new VerInventarioControl());
        }
    }
}
