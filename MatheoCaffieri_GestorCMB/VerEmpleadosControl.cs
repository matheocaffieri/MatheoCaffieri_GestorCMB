using BL;
using DomainModel;
using BL.BL_Interfaces;
using Services.Login;
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
    public partial class VerEmpleadosControl : UserControl
    {
        private const string REQUIRED = "VER_EMPLEADOS";

        private readonly IEmpleadoBL _empleadoRepo = new EmpleadoBL();

        private Panel            _scrollArea;
        private Label            _lblCount;

        public VerEmpleadosControl()
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        // LOAD
        // ══════════════════════════════════════════════════════════

        private void VerEmpleadosControl_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            // Backstop: si llegó acá sin permiso (navegación que no chequeó), no construir nada.
            if (!SessionContext.Has(REQUIRED)) return;
            BuildUI();
            BeginInvoke((Action)(() => CargarListado()));
        }

        // ══════════════════════════════════════════════════════════
        // BUILD UI
        // ══════════════════════════════════════════════════════════

        private void BuildUI()
        {
            this.Controls.Clear();
            this.BackColor     = Color.White;
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
                Text      = LanguageService.Current?.T("hdr_personal") ?? "Personal",
                Font      = new Font("Microsoft YaHei UI", 15f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 22, 28),
                AutoSize  = true,
                Location  = new Point(20, 16),
                BackColor = Color.Transparent,
            };

            var newSearch = new TextBox
            {
                Font        = new Font("Microsoft YaHei UI", 9f),
                BorderStyle = BorderStyle.FixedSingle,
                Width       = 230,
            };

            var newAddBtn = new Button
            {
                Text      = LanguageService.Current?.T("btn_agregar_empleado") ?? "+ Agregar empleado",
                Height    = 30,
                Width     = 158,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
            };
            newAddBtn.FlatAppearance.BorderSize = 0;
            newAddBtn.Click += buttonAgregarEmpleado_Click;

            header.Controls.Add(lblTitle);
            header.Controls.Add(newSearch);
            header.Controls.Add(newAddBtn);

            header.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                if (p.Width < 100) return;
                newAddBtn.Location = new Point(p.Width - newAddBtn.Width - 20, (p.Height - newAddBtn.Height) / 2);
                newSearch.Location = new Point(newAddBtn.Left - newSearch.Width - 8, (p.Height - newSearch.Height) / 2 + 1);
            };

            // ── SECTION BAR ───────────────────────────────────────
            var sectionBar = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 38,
                BackColor = Color.White,
            };

            var lblSec = new Label
            {
                Text      = LanguageService.Current?.T("hdr_empleados") ?? "Empleados",
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

            sectionBar.Controls.Add(_lblCount);
            sectionBar.Controls.Add(lblSec);

            sectionBar.Resize += (s, ev) =>
                _lblCount.Location = new Point(lblSec.Right + 6, 12);

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
                int cardW = CardWidth();
                foreach (EmpleadosItemControl c in newGrid.Controls.OfType<EmpleadosItemControl>())
                    c.Width = cardW;
            };

            // Reasignar campos del Designer para que la lógica existente funcione
            textBox1              = newSearch;
            buttonAgregarEmpleado = newAddBtn;
            empleadosLayoutPanel  = newGrid;

            // ── ASSEMBLE ──────────────────────────────────────────
            this.Controls.Add(_scrollArea);
            this.Controls.Add(sepH);
            this.Controls.Add(sectionBar);
            this.Controls.Add(header);

            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyDown     += textBox1_KeyDown;
        }

        // ══════════════════════════════════════════════════════════
        // LISTADO
        // ══════════════════════════════════════════════════════════

        private void CargarListado(string filtro = "")
        {
            List<Empleado> empleados = _empleadoRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();
                empleados = empleados.Where(e =>
                    (!string.IsNullOrEmpty(e.Nombre)   && e.Nombre.ToLower().Contains(filtro))   ||
                    (!string.IsNullOrEmpty(e.Apellido) && e.Apellido.ToLower().Contains(filtro)) ||
                    e.NroDocumento.ToString().Contains(filtro) ||
                    e.Sueldo.ToString().Contains(filtro)
                ).ToList();
            }

            if (_lblCount != null)
                _lblCount.Text = $"({empleados.Count})";

            empleadosLayoutPanel.SuspendLayout();
            empleadosLayoutPanel.Controls.Clear();

            int cardW = CardWidth();
            foreach (var emp in empleados)
            {
                var item = CrearItemEmpleado(emp);
                item.Width = cardW;
                empleadosLayoutPanel.Controls.Add(item);
            }

            empleadosLayoutPanel.ResumeLayout();
        }

        private int CardWidth()
        {
            if (_scrollArea == null) return 600;
            int avail = _scrollArea.ClientSize.Width - 32;
            return Math.Max(200, avail);
        }

        private EmpleadosItemControl CrearItemEmpleado(Empleado emp)
        {
            var item = new EmpleadosItemControl();
            item.Bind(emp);

            item.ActiveChanged += (e, nuevoEstado) =>
            {
                if (e == null || e.IdEmpleado == Guid.Empty) return;
                if (!PermisosUI.Require(TipoPermiso.GESTIONAR_EMPLEADOS))
                {
                    CargarListado(textBox1.Text); // revertir el switch al estado real
                    return;
                }
                try
                {
                    _empleadoRepo.Update(e);
                }
                catch (DomainModel.Exceptions.AppException ex)
                {
                    Services.Logs.LoggerLogic.Warn($"[VerEmpleadosControl] Validación al cambiar estado de empleado: {ex.MessageKey}");
                    MessageBox.Show(
                        LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CargarListado(textBox1.Text);
                }
                catch (Exception ex)
                {
                    Services.Logs.LoggerLogic.Error($"[VerEmpleadosControl] Falla al cambiar estado de empleado. Id={e.IdEmpleado}", ex);
                    MessageBox.Show(
                        LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CargarListado(textBox1.Text);
                }
            };

            item.EditRequested += (e) =>
            {
                if (e == null || e.IdEmpleado == Guid.Empty) return;
                if (!PermisosUI.Require(TipoPermiso.GESTIONAR_EMPLEADOS))
                    return;

                using (var frm = new EditEmpleadoForm(_empleadoRepo, e))
                {
                    var owner = this.FindForm();
                    var dr    = (owner != null) ? frm.ShowDialog(owner) : frm.ShowDialog();
                    if (dr == DialogResult.OK)
                        CargarListado(textBox1.Text);
                }
            };

            return item;
        }

        // ══════════════════════════════════════════════════════════
        // EVENTOS
        // ══════════════════════════════════════════════════════════

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (empleadosLayoutPanel == null) return;
            int cardW = CardWidth();
            foreach (EmpleadosItemControl c in empleadosLayoutPanel.Controls.OfType<EmpleadosItemControl>())
                c.Width = cardW;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) => CargarListado(textBox1.Text);

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            CargarListado(textBox1.Text);
        }

        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_EMPLEADOS))
                return;

            using (var frm = new AddEmpleadosForm(_empleadoRepo))
            {
                var owner = this.FindForm();
                DialogResult dr = (owner != null) ? frm.ShowDialog(owner) : frm.ShowDialog();
                if (dr == DialogResult.OK)
                    CargarListado(textBox1.Text);
            }
        }
    }
}
