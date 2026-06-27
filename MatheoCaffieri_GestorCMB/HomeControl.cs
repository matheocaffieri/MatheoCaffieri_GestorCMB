using BL;
using Services.LoginService.Logic;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using Services.Language;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class HomeControl : UserControl
    {
        private MainForm mainForm;
        private Panel    _contentPanel;
        private Dictionary<Control, (Point Loc, Size Size, Font Font)> _originals;

        private const int   DesignWidth    = 800;
        private const int   DesignHeight   = 358;
        private const float DesignMetricsH = 120f;

        // Metrics panel labels
        private Panel _metricsWrapper;
        private Label _lblEmpleadosVal,  _lblEmpleadosTitulo;
        private Label _lblInformesVal,   _lblInformesTitulo;
        private Label _lblProyectoNombre, _lblProyectoTitulo, _lblProyectoFecha;
        private Proyecto _proximoProyecto;

        // Rounded nav buttons (replace designer defaults)
        private RoundedButton _btnProyectos, _btnInventario, _btnPersonal, _btnClientes;

        // ── Constructor ────────────────────────────────────────────────────────
        public HomeControl(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            linkLabelUser.Text = string.IsNullOrWhiteSpace(UserSession.UserDisplayName)
                ? "User"
                : UserSession.UserDisplayName;
            linkLabelUser.DoubleClick -= linkLabelUser_DoubleClick;
            linkLabelUser.DoubleClick += linkLabelUser_DoubleClick;

            ReemplazarBotones();
            AplicarAccesosHome();

            WrapContentInPanel();
            CrearPanelMetricas();
            CenterContentPanel();

            this.Load -= HomeControl_Load;
            this.Load += HomeControl_Load;
        }

        // ── Replace designer square buttons with RoundedButtons ────────────────
        private void ReemplazarBotones()
        {
            // Capture positions from designer-created buttons (set from .resx)
            Point locProy = butMainProyectos.Location, locInv  = button2.Location,
                  locPers = button3.Location,           locCli  = button4.Location;
            Size  szProy  = butMainProyectos.Size,      szInv   = button2.Size,
                  szPers  = button3.Size,                szCli   = button4.Size;

            Controls.Remove(butMainProyectos);
            Controls.Remove(button2);
            Controls.Remove(button3);
            Controls.Remove(button4);

            _btnProyectos  = new RoundedButton(Color.FromArgb(76,  175, 80),  Properties.Resources.proyecto)
                             { Location = locProy, Size = szProy };
            _btnInventario = new RoundedButton(Color.FromArgb(255, 167, 38),  Properties.Resources.inventario)
                             { Location = locInv,  Size = szInv  };
            _btnPersonal   = new RoundedButton(Color.FromArgb(92,  155, 214), Properties.Resources.personal)
                             { Location = locPers, Size = szPers };
            _btnClientes   = new RoundedButton(Color.FromArgb(239, 83,  80),  Properties.Resources.cliente)
                             { Location = locCli,  Size = szCli  };

            _btnProyectos.Click  += butMainProyectos_Click;
            _btnInventario.Click += button2_Click;
            _btnPersonal.Click   += button3_Click;
            _btnClientes.Click   += button4_Click;

            Controls.Add(_btnProyectos);
            Controls.Add(_btnInventario);
            Controls.Add(_btnPersonal);
            Controls.Add(_btnClientes);
        }

        // ── Layout / scaling ───────────────────────────────────────────────────
        private void WrapContentInPanel()
        {
            _contentPanel = new Panel { Size = new Size(DesignWidth, DesignHeight) };

            foreach (var c in Controls.Cast<Control>().ToList())
                _contentPanel.Controls.Add(c);

            Controls.Add(_contentPanel);
            CenterContentPanel();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterContentPanel();
        }

        private void SnapOriginals()
        {
            if (_originals != null) return;
            _originals = new Dictionary<Control, (Point, Size, Font)>();
            foreach (Control c in _contentPanel.Controls)
                _originals[c] = (c.Location, c.Size, c.Font);
        }

        private void CenterContentPanel()
        {
            if (_contentPanel == null) return;
            SnapOriginals();

            const int topPadding = 8;
            const int bottomPadding = 10;

            bool compact = Height < 620 || Width < 980;

            int metricsH = compact ? 96 : (int)DesignMetricsH;
            int metricsSideMargin = compact ? 10 : 0;

            float scaleW = (float)Width / DesignWidth;
            float scaleH = (float)Math.Max(1, Height - metricsH - topPadding - bottomPadding) / DesignHeight;
            float scaleBase = Math.Min(scaleW, scaleH);

            float scale = scaleBase * (compact ? 0.97f : 0.92f);
            scale = Math.Max(scale, 0.45f);
            scale = Math.Min(scale, 2.4f);

            int panelW = (int)(DesignWidth * scale);
            int panelH = (int)(DesignHeight * scale);
            int leftX = Math.Max(0, (Width - panelW) / 2);

            int metricsTop = Height - bottomPadding - metricsH;

            int availableH = metricsTop - topPadding;
            int topY = topPadding + Math.Max(0, (availableH - panelH) / 2);

            _contentPanel.Size = new Size(panelW, panelH);
            _contentPanel.Location = new Point(leftX, topY);

            foreach (var kv in _originals)
            {
                var c = kv.Key;
                var orig = kv.Value;
                c.Location = new Point((int)(orig.Loc.X * scale), (int)(orig.Loc.Y * scale));
                c.Size = new Size((int)(orig.Size.Width * scale), (int)(orig.Size.Height * scale));
                c.Font = new Font(orig.Font.FontFamily, orig.Font.Size * scale, orig.Font.Style);
            }

            if (_metricsWrapper != null)
            {
                int metricsW = compact ? Math.Max(panelW, Width - (metricsSideMargin * 2)) : panelW;
                int metricsX = compact ? Math.Max(0, (Width - metricsW) / 2) : leftX;

                _metricsWrapper.Width = metricsW;
                _metricsWrapper.Height = metricsH;
                _metricsWrapper.Location = new Point(metricsX, metricsTop);
                _metricsWrapper.Padding = compact ? new Padding(10, 2, 10, 2) : new Padding(16, 8, 16, 8);

                ScaleMetricsLabels((float)metricsH / DesignMetricsH);
            }
        }

        private void ScaleMetricsLabels(float scale)
        {
            if (_lblEmpleadosVal == null) return;

            float valSize = Math.Max(11f, 22f * scale);
            float nameSize = Math.Max(8f, 11f * scale);
            float titleSize = Math.Max(7f, 8.5f * scale);
            int tituloH = Math.Max(15, (int)(22f * scale));

            int fechaH = Math.Max(12, (int)(18f * scale));

            _lblEmpleadosVal.Font = new Font("Microsoft YaHei UI", valSize, FontStyle.Bold);
            _lblInformesVal.Font = new Font("Microsoft YaHei UI", valSize, FontStyle.Bold);
            _lblProyectoNombre.Font = new Font("Microsoft YaHei UI", nameSize, FontStyle.Bold);

            if (_lblEmpleadosTitulo != null)
            {
                var tf = new Font("Microsoft YaHei UI", titleSize);
                _lblEmpleadosTitulo.Font = tf; _lblEmpleadosTitulo.Height = tituloH;
                _lblInformesTitulo.Font = tf; _lblInformesTitulo.Height = tituloH;
                _lblProyectoTitulo.Font = tf; _lblProyectoTitulo.Height = tituloH;
                _lblProyectoFecha.Font = new Font("Microsoft YaHei UI", titleSize);
                _lblProyectoFecha.Height = fechaH;
            }
        }

        // ── Navigation handlers ────────────────────────────────────────────────
        private void butMainProyectos_Click(object sender, EventArgs e) =>
            mainForm.addUserControl(new VerProyectosControl(mainForm));

        private void button2_Click(object sender, EventArgs e) =>
            mainForm.addUserControl(new VerInventarioControl());

        private void button3_Click(object sender, EventArgs e) =>
            mainForm.addUserControl(new VerEmpleadosControl());

        private void button4_Click(object sender, EventArgs e) =>
            mainForm.addUserControl(new ClientesControl(mainForm));

        private void HomeControl_Load(object sender, EventArgs e)
        {
            AplicarAccesosHome();
            CargarMetricas();
        }

        private void linkLabelUser_DoubleClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                LanguageService.Current?.T("msg_confirmar_cerrar_sesion") ?? "¿Querés cerrar sesión?",
                LanguageService.Current?.T("cap_cerrar_sesion") ?? "Cerrar sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                LoggerLogic.Info($"[HomeControl] Cierre de sesión solicitado. Usuario='{UserSession.UserDisplayName}'");

                var t = new System.Threading.Thread(() => Application.Run(new LoginForm()));
                t.SetApartmentState(System.Threading.ApartmentState.STA);
                t.Start();

                Form host = mainForm ?? this.FindForm();
                host?.Invoke(new Action(() => host.Close()));
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[HomeControl] Validación al cerrar sesión: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[HomeControl] Falla al cerrar sesión.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_cerrar_sesion") ?? "No se pudo cerrar sesión.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Access control ─────────────────────────────────────────────────────
        private void AplicarAccesosHome()
        {
            if (_btnProyectos == null) return;
            BloquearSiNoTienePermiso(_btnProyectos,  TipoPermiso.VER_PROYECTOS);
            BloquearSiNoTienePermiso(_btnInventario, TipoPermiso.VER_INVENTARIO);
            BloquearSiNoTienePermiso(_btnPersonal,   TipoPermiso.VER_EMPLEADOS);
            BloquearSiNoTienePermiso(_btnClientes,   TipoPermiso.VER_CLIENTES);
        }

        private struct ButtonStyleSnapshot
        {
            public Color     Back;
            public Color     Fore;
            public FlatStyle Flat;
            public bool      UseVisual;
        }

        private void GuardarEstiloOriginal(Button btn)
        {
            if (btn.Tag is ButtonStyleSnapshot) return;
            btn.Tag = new ButtonStyleSnapshot
            {
                Back      = btn.BackColor,
                Fore      = btn.ForeColor,
                Flat      = btn.FlatStyle,
                UseVisual = btn.UseVisualStyleBackColor
            };
        }

        private void RestaurarEstiloOriginal(Button btn)
        {
            if (btn.Tag is ButtonStyleSnapshot s)
            {
                btn.BackColor               = s.Back;
                btn.ForeColor               = s.Fore;
                btn.FlatStyle               = s.Flat;
                btn.UseVisualStyleBackColor = s.UseVisual;
                btn.Invalidate();
            }
        }

        private static Color Oscurecer(Color c, float factor = 0.88f) =>
            Color.FromArgb(c.A,
                (int)Math.Max(0, Math.Min(255, c.R * factor)),
                (int)Math.Max(0, Math.Min(255, c.G * factor)),
                (int)Math.Max(0, Math.Min(255, c.B * factor)));

        private void BloquearSiNoTienePermiso(Button btn, TipoPermiso permiso)
        {
            GuardarEstiloOriginal(btn);

            if (SessionManager.Instance.TienePermiso(permiso))
            {
                btn.Enabled = true;
                btn.Cursor  = Cursors.Hand;
                RestaurarEstiloOriginal(btn);
                return;
            }

            btn.Enabled               = false;
            btn.Cursor                = Cursors.No;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle             = FlatStyle.Flat;

            if (btn.Tag is ButtonStyleSnapshot s)
            {
                btn.BackColor                  = Oscurecer(s.Back, 0.88f);
                btn.ForeColor                  = SystemColors.GrayText;
                btn.FlatAppearance.BorderColor = Oscurecer(s.Back, 0.75f);
                btn.FlatAppearance.BorderSize  = 1;
                btn.Invalidate();
            }
        }

        // ── Metrics panel ──────────────────────────────────────────────────────
        private void CrearPanelMetricas()
        {
            _metricsWrapper = new Panel
            {
                Dock      = DockStyle.None,
                Height    = (int)DesignMetricsH,
                BackColor = SystemColors.Control,
                Padding   = new Padding(16, 8, 16, 8)
            };

            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // Rounded border drawn over the card
            card.Paint += (s, ev) =>
            {
                var p = (Panel)s;
                ev.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeRoundRectPath(new Rectangle(0, 0, p.Width - 1, p.Height - 1), 10))
                using (var pen  = new Pen(Color.FromArgb(215, 215, 215)))
                    ev.Graphics.DrawPath(pen, path);
            };

            // Clip children to the rounded rect
            card.Resize += (s, ev) =>
            {
                var p = (Panel)s;
                if (p.Width > 4 && p.Height > 4)
                {
                    using (var path = MakeRoundRectPath(new Rectangle(0, 0, p.Width, p.Height), 10))
                        p.Region = new Region(path);
                }
            };

            var table = new TableLayoutPanel
            {
                Dock            = DockStyle.Fill,
                ColumnCount     = 5,
                RowCount        = 1,
                BackColor       = Color.Transparent,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  33f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,  1f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  34f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,  1f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,  33f));

            var cellEmpleados = CreateMetricCell(LanguageService.Current?.T("hdr_empleados_activos")   ?? "Empleados activos",  out _lblEmpleadosTitulo, out _lblEmpleadosVal);
            var cellInformes  = CreateMetricCell(LanguageService.Current?.T("hdr_informes_de_compra") ?? "Informes de compra", out _lblInformesTitulo,  out _lblInformesVal);
            var cellProyecto  = CreateProyectoCell(out _lblProyectoTitulo, out _lblProyectoNombre, out _lblProyectoFecha);

            table.Controls.Add(cellEmpleados, 0, 0);
            table.Controls.Add(CreateSeparator(), 1, 0);
            table.Controls.Add(cellInformes,  2, 0);
            table.Controls.Add(CreateSeparator(), 3, 0);
            table.Controls.Add(cellProyecto,  4, 0);

            ApplyInteraction(cellEmpleados, () => {
                if (PermisosUI.Require(TipoPermiso.VER_EMPLEADOS))
                    mainForm.addUserControl(new VerEmpleadosControl());
            });
            ApplyInteraction(cellInformes,  () => {
                if (PermisosUI.Require(TipoPermiso.VER_INFORMES_COMPRA))
                    mainForm.addUserControl(new InformesDeCompraControl(mainForm));
            });
            ApplyInteraction(cellProyecto,  () => {
                if (_proximoProyecto != null && PermisosUI.Require(TipoPermiso.VER_PROYECTOS))
                    mainForm.addUserControl(new DetalleProyectoControl(mainForm, _proximoProyecto));
            });

            card.Controls.Add(table);
            _metricsWrapper.Controls.Add(card);
            Controls.Add(_metricsWrapper);
        }

        private static readonly Color HoverColor = Color.FromArgb(244, 246, 252);

        private static void ApplyInteraction(Panel cell, Action onDoubleClick)
        {
            void Enter(object s, EventArgs e) => cell.BackColor = HoverColor;
            void Leave(object s, EventArgs e)
            {
                if (!cell.ClientRectangle.Contains(cell.PointToClient(Cursor.Position)))
                    cell.BackColor = Color.White;
            }
            void DblClick(object s, EventArgs e) => onDoubleClick?.Invoke();
            AttachEventsRecursive(cell, Enter, Leave, DblClick);
        }

        private static void AttachEventsRecursive(Control root, EventHandler enter, EventHandler leave, EventHandler dblClick)
        {
            root.Cursor      = Cursors.Hand;
            root.MouseEnter  += enter;
            root.MouseLeave  += leave;
            root.DoubleClick += dblClick;
            foreach (Control c in root.Controls)
                AttachEventsRecursive(c, enter, leave, dblClick);
        }

        private static Panel CreateSeparator() =>
            new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(220, 220, 220) };

        private static Panel CreateMetricCell(string titulo, out Label lblTitulo, out Label lblValor)
        {
            var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            lblTitulo = new Label
            {
                Text      = titulo,
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.FromArgb(110, 110, 110),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock      = DockStyle.Top,
                Height    = 22,
                AutoSize  = false,
                BackColor = Color.Transparent
            };

            lblValor = new Label
            {
                Text      = "—",
                Font      = new Font("Microsoft YaHei UI", 22f, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock      = DockStyle.Fill,
                AutoSize  = false,
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblValor);
            panel.Controls.Add(lblTitulo);
            return panel;
        }

        private static Panel CreateProyectoCell(out Label lblTitulo, out Label lblNombre, out Label lblFecha)
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));  // Título
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // Nombre
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));  // Fecha (un poco más arriba/visible)

            lblTitulo = new Label
            {
                Text = LanguageService.Current?.T("hdr_proximo_cierre") ?? "Próximo cierre",
                Font = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.FromArgb(110, 110, 110),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Margin = Padding.Empty
            };

            lblNombre = new Label
            {
                Text = "—",
                Font = new Font("Microsoft YaHei UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Margin = Padding.Empty
            };

            lblFecha = new Label
            {
                Text = "",
                Font = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.FromArgb(130, 130, 130),
                TextAlign = ContentAlignment.TopCenter,
                Dock = DockStyle.Top,
                Height = 14,
                AutoSize = false,
                Margin = new Padding(0, -3, 0, 0) // sube un toque más la fecha
            };

            layout.Controls.Add(lblTitulo, 0, 0);
            layout.Controls.Add(lblNombre, 0, 1);
            layout.Controls.Add(lblFecha, 0, 2);

            panel.Controls.Add(layout);
            return panel;
        }

        private void CargarMetricas()
        {
            try
            {
                var bl = (IEmpleadoRepository)new EmpleadoBL();
                _lblEmpleadosVal.Text = bl.GetAll().Count(e => e.IsActive).ToString();
            }
            catch { _lblEmpleadosVal.Text = "—"; }

            try
            {
                _lblInformesVal.Text = new InformeDeCompraBL().GetAll().Count.ToString();
            }
            catch { _lblInformesVal.Text = "—"; }

            try
            {
                var proximo = new ProyectoBL().GetAll()
                    .Where(p => p.Estado != EnumEstado.Suspendido && p.Estado != EnumEstado.Finalizado)
                    .OrderBy(p => p.FechaFin)
                    .FirstOrDefault();

                _proximoProyecto        = proximo;
                _lblProyectoNombre.Text = proximo?.Descripcion
                    ?? (LanguageService.Current?.T("txt_sin_proyectos_activos") ?? "Sin proyectos activos");
                _lblProyectoFecha.Text  = proximo != null ? proximo.FechaFin.ToString("dd/MM/yyyy") : "";
            }
            catch
            {
                _lblProyectoNombre.Text = "—";
                _lblProyectoFecha.Text  = "";
            }
        }

        // ── Shared drawing helper ──────────────────────────────────────────────
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

        // ── RoundedButton ──────────────────────────────────────────────────────
        private sealed class RoundedButton : Button
        {
            public Image Icon { get; set; }

            public RoundedButton(Color accent, Image icon)
            {
                Icon = icon;
                BackColor = accent;
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                UseVisualStyleBackColor = false;
                Cursor = Cursors.Hand;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw, true);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);

                int radius = Math.Max(6, Math.Min(Width, Height) / 6);
                if (Width > 2 && Height > 2)
                {
                    using (var path = MakeRoundRectPath(new Rectangle(0, 0, Width - 1, Height - 1), radius))
                        Region = new Region(path);
                }
            }

            protected override void OnPaintBackground(PaintEventArgs pevent)
            {
                // Evita los "bordes negros" en esquinas fuera del radio.
                pevent.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                bool hovered = Enabled && ClientRectangle.Contains(PointToClient(Cursor.Position));
                var fillColor = Enabled
                    ? (hovered ? DimColor(BackColor, 0.88f) : BackColor)
                    : DimColor(BackColor, 0.55f);

                int radius = Math.Max(6, Math.Min(Width, Height) / 6);
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);

                using (var path = MakeRoundRectPath(rect, radius))
                using (var brush = new SolidBrush(fillColor))
                using (var pen = new Pen(fillColor))
                {
                    g.FillPath(brush, path);
                    g.DrawPath(pen, path); // tapa el halo oscuro en el borde redondeado
                }

                if (Icon != null)
                {
                    int iconSize = (int)(Math.Min(Width, Height) * 0.55f);
                    int ix = (Width - iconSize) / 2;
                    int iy = (Height - iconSize) / 2;
                    DrawIconWhite(g, Icon, new Rectangle(ix, iy, iconSize, iconSize), Enabled ? 1f : 0.35f);
                }
            }

            // Draws the icon tinted to white (preserves alpha channel)
            private static void DrawIconWhite(Graphics g, Image img, Rectangle dest, float alpha)
            {
                float[][] m = {
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 0, 0, alpha, 0 },
                    new float[] { 1, 1, 1, 0, 1 }
                };
                using (var attrs = new ImageAttributes())
                {
                    attrs.SetColorMatrix(new ColorMatrix(m));
                    g.DrawImage(img, dest, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attrs);
                }
            }

            protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); Invalidate(); }
            protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); Invalidate(); }

            private static Color DimColor(Color c, float f) =>
                Color.FromArgb(c.A,
                    (int)Math.Max(0, Math.Min(255, c.R * f)),
                    (int)Math.Max(0, Math.Min(255, c.G * f)),
                    (int)Math.Max(0, Math.Min(255, c.B * f)));
        }
    }
}
