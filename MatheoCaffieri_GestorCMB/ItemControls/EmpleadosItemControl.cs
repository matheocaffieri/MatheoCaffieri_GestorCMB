using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DomainModel;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class EmpleadosItemControl : UserControl
    {
        public Empleado Empleado { get; private set; }

        public event Action<Empleado, bool> ActiveChanged;
        public event Action<Empleado>       EditRequested;

        private Panel       _avatar;
        private Label       _lblNombre;
        private Label       _lblInfo;
        private Button      _btnEdit;
        private ToggleSwitch _toggle;
        private string      _initials    = "?";
        private Color       _avatarColor = Color.Gray;

        public EmpleadosItemControl()
        {
            InitializeComponent();
            this.BackColor     = Color.White;
            this.Height        = 68;
            this.DoubleBuffered = true;
            BuildCard();
        }

        private void BuildCard()
        {
            _avatar = new Panel
            {
                Size      = new Size(40, 40),
                Location  = new Point(12, 14),
                BackColor = Color.Transparent,
            };
            _avatar.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new RectangleF(0, 0, _avatar.Width - 1, _avatar.Height - 1);
                using (var br = new SolidBrush(_avatarColor))
                    g.FillEllipse(br, rect);
                using (var f  = new Font("Microsoft YaHei UI", 11f, FontStyle.Bold))
                using (var br = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat
                    {
                        Alignment     = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                    };
                    g.DrawString(_initials, f, br, rect, sf);
                }
            };

            _lblNombre = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 25, 30),
                AutoSize  = false,
                Height    = 20,
                Location  = new Point(62, 12),
                BackColor = Color.Transparent,
            };

            _lblInfo = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 8f),
                ForeColor = Color.FromArgb(125, 125, 138),
                AutoSize  = false,
                Height    = 18,
                Location  = new Point(62, 34),
                BackColor = Color.Transparent,
            };

            _btnEdit = new Button
            {
                Size      = new Size(26, 26),
                BackColor = Color.Transparent,
                BackgroundImage       = Properties.Resources.edit_logo,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                TabStop   = false,
            };
            _btnEdit.FlatAppearance.BorderSize           = 0;
            _btnEdit.FlatAppearance.MouseOverBackColor   = Color.FromArgb(238, 238, 244);
            _btnEdit.FlatAppearance.MouseDownBackColor   = Color.FromArgb(220, 220, 228);
            _btnEdit.Click += (s, e) => { if (Empleado != null) EditRequested?.Invoke(Empleado); };

            _toggle = new ToggleSwitch
            {
                Width    = 44,
                Height   = 22,
                IsOn     = false,
                OnColor  = Color.FromArgb(76, 175, 80),
                OffColor = Color.FromArgb(190, 190, 195),
            };
            _toggle.ToggleChanged += (s, e) =>
            {
                if (Empleado == null) return;
                bool state       = _toggle.IsOn;
                Empleado.IsActive = state;
                _avatarColor     = state ? AvatarColor(Empleado) : Color.FromArgb(190, 190, 200);
                _avatar.Invalidate();
                ActiveChanged?.Invoke(Empleado, state);
            };

            this.Controls.Add(_avatar);
            this.Controls.Add(_lblNombre);
            this.Controls.Add(_lblInfo);
            this.Controls.Add(_btnEdit);
            this.Controls.Add(_toggle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(Color.FromArgb(232, 232, 238), 1))
                e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Width < 120 || _toggle == null) return;

            int rightEdge    = Width - 10;
            _toggle.Location = new Point(rightEdge - _toggle.Width, (Height - _toggle.Height) / 2);
            _btnEdit.Location = new Point(_toggle.Left - _btnEdit.Width - 6, (Height - _btnEdit.Height) / 2);

            int textRight    = _btnEdit.Left - 8;
            _lblNombre.Width = Math.Max(10, textRight - _lblNombre.Left);
            _lblInfo.Width   = Math.Max(10, textRight - _lblInfo.Left);
        }

        public void Bind(Empleado e)
        {
            Empleado     = e;
            _initials    = GetInitials(e.Nombre, e.Apellido);
            _avatarColor = e.IsActive ? AvatarColor(e) : Color.FromArgb(190, 190, 200);
            _avatar?.Invalidate();

            _lblNombre.Text = $"{e.Nombre} {e.Apellido}";

            int p = e.CantidadProyectosActivos;
            string dni = Services.Language.LanguageService.Current?.T("txt_dni") ?? "DNI";
            string proyectos = string.Format(
                Services.Language.LanguageService.Current?.T(p == 1 ? "txt_proyecto_activo_fmt" : "txt_proyectos_activos_fmt")
                    ?? (p == 1 ? "{0} proyecto activo" : "{0} proyectos activos"),
                p);
            _lblInfo.Text = $"{dni}: {e.NroDocumento}  ·  ${e.Sueldo:N0}  ·  {proyectos}";

            _toggle.IsOn = e.IsActive;
        }

        private static string GetInitials(string nombre, string apellido)
        {
            var n  = (nombre   ?? "").Trim();
            var a  = (apellido ?? "").Trim();
            string i1 = n.Length > 0 ? n[0].ToString().ToUpper() : "";
            string i2 = a.Length > 0 ? a[0].ToString().ToUpper() : "";
            return $"{i1}{i2}";
        }

        private static readonly Color[] _palette =
        {
            Color.FromArgb( 79, 129, 189),
            Color.FromArgb(155,  99, 177),
            Color.FromArgb( 75, 172, 198),
            Color.FromArgb(235, 123,  56),
            Color.FromArgb( 76, 175,  80),
            Color.FromArgb(210,  70,  70),
            Color.FromArgb(100, 130, 180),
        };

        private static Color AvatarColor(Empleado e)
        {
            int hash = ((e.Nombre ?? "") + (e.Apellido ?? "")).GetHashCode();
            return _palette[Math.Abs(hash) % _palette.Length];
        }
    }
}
