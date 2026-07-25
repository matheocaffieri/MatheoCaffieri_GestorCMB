using BL;
using DomainModel.Exceptions;
using Services.Language;
using Services.Logs;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DomainModel;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class InventarioItemControl : UserControl
    {
        private readonly InventarioBL _invBL = new InventarioBL();

        private Label  _lblNombre;
        private Panel  _chip;
        private Label  _lblChipTxt;
        private Label  _lblInfo;
        private Button _btnIncrease;
        private Button _btnDecrease;
        private Label  _lblCantidad;
        private Panel  _stockPanel;
        private Panel  _actionPanel;
        private Button _btnEdit;
        private Button _btnDelete;

        private Color _chipColor = Color.Gray;
        private Inventario _currentInv;

        private Panel _badgePendiente;
        private Label _lblBadgeTxt;
        private bool  _badgeVisible;

        public event EventHandler<Inventario> EditRequested;
        public event EventHandler Deleted;
        public event EventHandler InformePendienteClicked;

        public InventarioItemControl()
        {
            InitializeComponent();
            this.BackColor      = Color.White;
            this.Height         = 80;
            this.DoubleBuffered = true;
            BuildCard();
        }

        private void BuildCard()
        {
            // ===== Fila 1: nombre =====
            _lblNombre = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 22, 28),
                AutoSize  = false,
                Height    = 20,
                Location  = new Point(14, 14),
                BackColor = Color.Transparent,
            };

            // ===== Fila 2: chip categoria =====
            _chip = new Panel
            {
                Height    = 18,
                Location  = new Point(14, 44),
                BackColor = Color.Transparent,
            };
            _chip.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, _chip.Width - 1, _chip.Height - 1);
                int radius = r.Height / 2;
                using (var path = RoundedRect(r, radius))
                using (var br   = new SolidBrush(_chipColor))
                    g.FillPath(br, path);
            };

            _lblChipTxt = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 7.5f),
                ForeColor = Color.White,
                AutoSize  = true,
                BackColor = Color.Transparent,
                Location  = new Point(6, 1),
            };
            _chip.Controls.Add(_lblChipTxt);

            // ===== Fila 2: info (proveedor / costo / unidad) =====
            _lblInfo = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.FromArgb(115, 115, 130),
                AutoSize  = false,
                Height    = 18,
                Location  = new Point(14, 44),   // X se ajusta en OnResize
                BackColor = Color.Transparent,
            };

            // ===== Panel de stock =====
            _stockPanel = new Panel { Width = 60, BackColor = Color.Transparent };

            _btnIncrease = MakeArrowBtn("+");
            _btnIncrease.Click += BtnIncrease_Click;

            _lblCantidad = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Height    = 28,
                BackColor = Color.White,
            };

            _btnDecrease = MakeArrowBtn("−");
            _btnDecrease.Click += BtnDecrease_Click;

            _stockPanel.Controls.Add(_btnDecrease);
            _stockPanel.Controls.Add(_lblCantidad);
            _stockPanel.Controls.Add(_btnIncrease);

            // ===== Badge "informe pendiente" (oculto por defecto) =====
            _badgePendiente = new Panel
            {
                Height    = 22,
                Visible   = false,
                BackColor = Color.Transparent,
                Cursor    = Cursors.Hand,
            };
            _badgePendiente.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, _badgePendiente.Width - 1, _badgePendiente.Height - 1);
                int radius = r.Height / 2;
                using (var path = RoundedRect(r, radius))
                using (var br   = new SolidBrush(Color.FromArgb(235, 123, 56)))
                    g.FillPath(br, path);
            };
            _lblBadgeTxt = new Label
            {
                Font      = new Font("Microsoft YaHei UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                BackColor = Color.Transparent,
                Location  = new Point(8, 4),
                Cursor    = Cursors.Hand,
            };
            _badgePendiente.Controls.Add(_lblBadgeTxt);
            _badgePendiente.Click += (s, e) => InformePendienteClicked?.Invoke(this, EventArgs.Empty);
            _lblBadgeTxt.Click    += (s, e) => InformePendienteClicked?.Invoke(this, EventArgs.Empty);

            // ===== Panel de acciones (editar + eliminar) =====
            _actionPanel = new Panel { Width = 66, BackColor = Color.Transparent };

            _btnEdit = MakeIconBtn("", Color.FromArgb(60, 100, 180));
            _btnEdit.Click += BtnEdit_Click;

            _btnDelete = MakeIconBtn("", Color.FromArgb(200, 50, 50));
            _btnDelete.Click += BtnDelete_Click;

            _actionPanel.Controls.Add(_btnEdit);
            _actionPanel.Controls.Add(_btnDelete);

            this.Controls.Add(_lblNombre);
            this.Controls.Add(_chip);
            this.Controls.Add(_lblInfo);
            this.Controls.Add(_badgePendiente);
            this.Controls.Add(_stockPanel);
            this.Controls.Add(_actionPanel);
        }

        private static Button MakeArrowBtn(string text)
        {
            var btn = new Button
            {
                Text      = text,
                Height    = 22,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Microsoft YaHei UI", 7f, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 95),
                BackColor = Color.FromArgb(242, 242, 247),
                Cursor    = Cursors.Hand,
                TabStop   = false,
            };
            btn.FlatAppearance.BorderColor        = Color.FromArgb(215, 215, 222);
            btn.FlatAppearance.BorderSize         = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(225, 225, 238);
            return btn;
        }

        private static Button MakeIconBtn(string icon, Color iconColor)
        {
            var btn = new Button
            {
                Text      = icon,
                Width     = 28,
                Height    = 28,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe MDL2 Assets", 10f),
                ForeColor = iconColor,
                BackColor = Color.White,
                Cursor    = Cursors.Hand,
                TabStop   = false,
            };
            btn.FlatAppearance.BorderColor        = Color.FromArgb(218, 220, 228);
            btn.FlatAppearance.BorderSize         = 1;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(242, 245, 255);
            return btn;
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
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
            if (Width < 150 || _stockPanel == null) return;

            // Stock panel anclado al extremo derecho
            _stockPanel.Height   = Height;
            _stockPanel.Location = new Point(Width - _stockPanel.Width - 12, 0);

            // Action panel justo a la izquierda del stock panel
            _actionPanel.Height   = Height;
            _actionPanel.Location = new Point(_stockPanel.Left - _actionPanel.Width - 10, 0);
            int btnY = (Height - 28) / 2;
            _btnEdit.Location   = new Point(0, btnY);
            _btnDelete.Location = new Point(38, btnY);

            int sw = _stockPanel.Width;
            _btnIncrease.Width = _lblCantidad.Width = _btnDecrease.Width = sw;
            int mid = (Height - (_btnIncrease.Height + _lblCantidad.Height + _btnDecrease.Height + 2)) / 2;
            _btnIncrease.Location = new Point(0, mid);
            _lblCantidad.Location = new Point(0, _btnIncrease.Bottom + 1);
            _btnDecrease.Location = new Point(0, _lblCantidad.Bottom + 1);

            // Fila 1: badge "pendiente" anclado a la derecha (si está visible) + nombre hasta el badge / acciones
            int titleRightLimit = _actionPanel.Left - 10;
            if (_badgePendiente != null && _badgeVisible)
            {
                int badgeY = (Math.Max(14, _lblNombre.Top + (_lblNombre.Height - _badgePendiente.Height) / 2));
                _badgePendiente.Location = new Point(_actionPanel.Left - _badgePendiente.Width - 8, badgeY);
                titleRightLimit = _badgePendiente.Left - 8;
            }
            _lblNombre.Width = Math.Max(10, titleRightLimit - _lblNombre.Left);

            // Fila 2: chip + info hasta el panel de acciones
            int maxRight  = _actionPanel.Left - 10;
            int chipRight = _chip.Right + 8;
            _lblInfo.Location = new Point(chipRight, 37);
            _lblInfo.Width    = Math.Max(10, maxRight - chipRight);
        }

        public void SetInformePendiente(bool hasPending)
        {
            _badgeVisible = hasPending;
            if (_badgePendiente == null) return;

            _badgePendiente.Visible = hasPending;
            if (!hasPending)
            {
                OnResize(EventArgs.Empty);
                return;
            }

            _lblBadgeTxt.Text       = LanguageService.Current?.T("lbl_informe_pendiente_badge") ?? "Pendiente en informe";
            _badgePendiente.Width   = _lblBadgeTxt.Width + 16;
            OnResize(EventArgs.Empty);
            _badgePendiente.Invalidate();
        }

        // ===== Cargar datos =====

        public void Bind(Inventario inv)
        {
            _currentInv = inv;
            string nombre = inv.Material?.DescripcionArticulo
                ?? (LanguageService.Current?.T("txt_sin_nombre") ?? "(sin nombre)");
            string tipo   = inv.Material?.TipoMaterial                ?? "—";
            string prov   = inv.Material?.Proveedor?.Descripcion
                ?? (LanguageService.Current?.T("txt_sin_proveedor") ?? "Sin proveedor");
            string unidad = inv.Material?.TipoUnidad                  ?? "";
            var    costo  = (decimal)(inv.Material?.CostoPorUnidad ?? 0);

            _lblNombre.Text  = nombre;

            _chipColor       = CategoryColor(tipo);
            _lblChipTxt.Text = tipo;
            _chip.Width      = _lblChipTxt.Width + 14;
            _chip.Invalidate();

            _lblInfo.Text = $"{prov}  ·  ${costo:N0}  ·  {unidad}";

            // Reposicionar info ahora que el chip tiene su ancho real
            if (_actionPanel != null && _actionPanel.Left > 0)
            {
                int chipRight = _chip.Left + _chip.Width + 8;
                int maxRight  = _actionPanel.Left - 10;
                _lblInfo.Location = new Point(chipRight, 45);
                _lblInfo.Width    = Math.Max(10, maxRight - chipRight);
            }

            _lblCantidad.Text = inv.Cantidad.ToString();

            _btnIncrease.Tag = inv.IdMaterialInventario;
            _btnDecrease.Tag = inv.IdMaterialInventario;
        }

        // Compat
        public string CantidadArticuloInventario
        {
            get => _lblCantidad?.Text ?? "0";
            set { if (_lblCantidad != null) _lblCantidad.Text = value; }
        }

        public void SetInventarioId(Guid idInventario)
        {
            if (_btnIncrease != null) _btnIncrease.Tag = idInventario;
            if (_btnDecrease != null) _btnDecrease.Tag = idInventario;
        }

        // ===== Botones =====

        private void BtnIncrease_Click(object sender, EventArgs e)
        {
            if (!((sender as Button)?.Tag is Guid id)) return;
            try
            {
                _lblCantidad.Text = _invBL.CambiarCantidad(id, +1).ToString();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioItemControl] Falla al aumentar cantidad. InvId={id}", ex);
                MessageBox.Show(LanguageService.Current?.T("err_aumentar_cantidad") ?? "No se pudo aumentar la cantidad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecrease_Click(object sender, EventArgs e)
        {
            if (!((sender as Button)?.Tag is Guid id)) return;
            try
            {
                _lblCantidad.Text = _invBL.CambiarCantidad(id, -1).ToString();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioItemControl] Falla al disminuir cantidad. InvId={id}", ex);
                MessageBox.Show(LanguageService.Current?.T("err_disminuir_cantidad") ?? "No se pudo disminuir la cantidad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_currentInv != null)
                EditRequested?.Invoke(this, _currentInv);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentInv == null) return;
            string nombre = _currentInv.Material?.DescripcionArticulo
                ?? (LanguageService.Current?.T("txt_este_material") ?? "este material");
            var r = MessageBox.Show(
                string.Format(LanguageService.Current?.T("msg_confirmar_eliminar_material_inv_fmt") ?? "¿Eliminar \"{0}\"?\nEsta acción no se puede deshacer.", nombre),
                LanguageService.Current?.T("cap_confirmar_eliminacion") ?? "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (r != DialogResult.Yes) return;
            try
            {
                new MaterialBL().Delete(_currentInv.Material);
                Deleted?.Invoke(this, EventArgs.Empty);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[InventarioItemControl] Validación al eliminar material: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    LanguageService.Current?.T("cap_error") ?? "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[InventarioItemControl] Falla al eliminar material. Id={_currentInv.Material?.IdMaterial}", ex);
                MessageBox.Show(
                    string.Format(LanguageService.Current?.T("err_eliminar_material_fmt") ?? "No se pudo eliminar el material.\n{0}", ex.Message),
                    LanguageService.Current?.T("cap_error") ?? "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Colores por categoria =====

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

        private static Color CategoryColor(string tipo)
        {
            int hash = (tipo ?? "").GetHashCode();
            return _palette[Math.Abs(hash) % _palette.Length];
        }
    }
}
