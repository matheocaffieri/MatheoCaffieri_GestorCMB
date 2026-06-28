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
using DomainModel;
using Services.Language;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class ProyectoItemControl : UserControl
    {
        private MainForm mainForm;
        private Proyecto _proyecto;
        private Panel _badgeFaltantes;
        private int _faltantesCount;

        public string NumProyecto
        {
            get => labelNumProyecto.Text;
            set
            {
                labelNumProyecto.Text = value ?? (LanguageService.Current?.T("txt_proyecto_na") ?? "Proyecto #N/A");
                RepositionarDescripcion();
            }
        }

        // El número (AutoSize) cambia de ancho según el texto, así que pegamos la
        // descripción justo a su derecha para que no quede un hueco fijo en el medio.
        private void RepositionarDescripcion()
        {
            labelDescripcionProyecto.Left = labelNumProyecto.Right + 10;
        }

        public string DescripcionProyecto
        {
            get => labelDescripcionProyecto.Text;
            set => labelDescripcionProyecto.Text = value ?? (LanguageService.Current?.T("txt_sin_descripcion") ?? "Sin descripción");
        }

        public string NombreCliente
        {
            get => labelNombreCliente.Text;
            set => labelNombreCliente.Text = value ?? (LanguageService.Current?.T("txt_desconocido") ?? "Desconocido");
        }

        public string FechaInicio
        {
            get => labelFechaInicio.Text;
            set => labelFechaInicio.Text = value ?? (LanguageService.Current?.T("txt_fecha_no_disponible") ?? "Fecha no disponible");
        }

        public string EstadoProyecto
        {
            get => labelEstadoProyecto.Text;
            set => labelEstadoProyecto.Text = value ?? (LanguageService.Current?.T("txt_estado_desconocido") ?? "Estado desconocido");
        }

        public string UbicacionProyecto
        {
            get => labelUbicacionProyecto.Text;
            set => labelUbicacionProyecto.Text = value ?? (LanguageService.Current?.T("txt_ubicacion_no_especificada") ?? "Ubicación no especificada");
        }

        public Proyecto ProyectoData
        {
            get => _proyecto;
            set => _proyecto = value;
        }

        public ProyectoItemControl(MainForm mainForm, Proyecto proyecto)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this._proyecto = proyecto;

            NumProyecto = LanguageService.Current?.T("txt_proyecto_num_default") ?? "Proyecto #";
            DescripcionProyecto = proyecto.Descripcion;
            NombreCliente = proyecto.Cliente?.NombreContacto
                ?? (LanguageService.Current?.T("txt_cliente_desconocido") ?? "Cliente desconocido");
            FechaInicio = proyecto.FechaInicio.ToString("dd/MM/yyyy")
                ?? (LanguageService.Current?.T("txt_sin_fecha") ?? "Sin fecha");
            EstadoProyecto = FormatEstado(proyecto.Estado);
            UbicacionProyecto = proyecto.Ubicacion
                ?? (LanguageService.Current?.T("txt_ubicacion_desconocida") ?? "Ubicación desconocida");

            switch (proyecto.Estado)
            {
                case EnumEstado.EnProceso:
                    labelEstadoProyecto.ForeColor = Color.CornflowerBlue;
                    break;
                case EnumEstado.Suspendido:
                    labelEstadoProyecto.ForeColor = Color.DarkOrange;
                    break;
                case EnumEstado.Finalizado:
                    labelEstadoProyecto.ForeColor = Color.MediumSeaGreen;
                    break;
            }

            InicializarEllipsisYTooltips();
            InicializarBadge();
            EstilarCard();
        }

        private void InicializarEllipsisYTooltips()
        {
            var tip = new ToolTip { AutoPopDelay = 5000, InitialDelay = 400 };

            var labelsConEllipsis = new[]
            {
                labelNombreCliente,
                labelUbicacionProyecto,
                labelDescripcionProyecto,
                labelEstadoProyecto,
            };

            foreach (var lbl in labelsConEllipsis)
            {
                lbl.AutoSize     = false;
                lbl.AutoEllipsis = true;
                tip.SetToolTip(lbl, lbl.Text);
            }
        }

        private void InicializarBadge()
        {
            _badgeFaltantes = new Panel
            {
                Size = new Size(22, 22),
                Visible = false,
                BackColor = Color.Transparent
            };
            _badgeFaltantes.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var bounds = new RectangleF(0, 0, _badgeFaltantes.Width - 1, _badgeFaltantes.Height - 1);
                using (var brush = new SolidBrush(Color.Crimson))
                    g.FillEllipse(brush, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                var text = _faltantesCount > 99 ? "99+" : $"{_faltantesCount}";
                using (var font = new Font("Microsoft YaHei UI", 7f, FontStyle.Bold))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(text, font, textBrush, bounds, sf);
                }
            };
            _tipBadge = new ToolTip { AutoPopDelay = 5000, InitialDelay = 400 };
            this.Controls.Add(_badgeFaltantes);
            _badgeFaltantes.BringToFront();
        }

        private ToolTip _tipBadge;

        private void EstilarCard()
        {
            BackColor = Color.White;
            Margin    = new Padding(4, 4, 4, 4);
            Cursor    = Cursors.Hand;

            foreach (Control c in Controls)
                if (c is Label) c.BackColor = Color.Transparent;

            Resize += (s, e) =>
            {
                if (Width > 10 && Height > 10)
                    using (var path = MakeCardPath(new Rectangle(1, 1, Width - 2, Height - 2), 10))
                        Region = new Region(path);
            };

            Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeCardPath(new Rectangle(0, 0, Width - 1, Height - 1), 10))
                using (var pen  = new Pen(Color.FromArgb(218, 218, 225)))
                    e.Graphics.DrawPath(pen, path);
            };
        }

        private static GraphicsPath MakeCardPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X,                  r.Y,                   radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y,                   radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2,   0, 90);
            path.AddArc(r.X,                  r.Bottom - radius * 2, radius * 2, radius * 2,  90, 90);
            path.CloseFigure();
            return path;
        }

        public void SetFaltantesCount(int count)
        {
            _faltantesCount = count;
            if (count > 0)
            {
                // 1 dígito → círculo perfecto; 2 dígitos → píldora ancha; 99+ → más ancha
                _badgeFaltantes.Width  = count > 99 ? 34 : count > 9 ? 28 : 22;
                _badgeFaltantes.Height = 22;
                _badgeFaltantes.Visible = true;
                _badgeFaltantes.BringToFront();
                var keyFmt = count == 1 ? "txt_faltantes_count_singular_fmt" : "txt_faltantes_count_plural_fmt";
                var fallback = count == 1 ? "{0} material faltante" : "{0} materiales faltantes";
                _tipBadge.SetToolTip(_badgeFaltantes,
                    string.Format(LanguageService.Current?.T(keyFmt) ?? fallback, count));
                PositionBadge();
            }
            else
            {
                _badgeFaltantes.Visible = false;
            }
            _badgeFaltantes.Invalidate();
        }

        private void PositionBadge()
        {
            if (_badgeFaltantes == null) return;
            _badgeFaltantes.Location = new Point(
                this.Width - _badgeFaltantes.Width - 14,
                8
            );
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width < 300) return;

            int rightX = this.Width - (646 - 337);
            int valorX = rightX + (426 - 337);
            int linkX  = this.Width - (646 - 538);

            label2.Left                 = rightX;
            label4.Left                 = rightX;
            labelEstadoProyecto.Left    = valorX;
            labelUbicacionProyecto.Left = valorX;
            linkLabelVerDetalles.Left   = linkX;

            // columna izquierda: cliente y descripción llegan hasta el separador derecho
            int leftMax = Math.Max(40, rightX - labelNombreCliente.Left - 8);
            labelNombreCliente.Width       = leftMax;
            labelDescripcionProyecto.Width = Math.Max(40, rightX - labelDescripcionProyecto.Left - 8);

            // columna derecha: estado y ubicación llegan hasta "Ver detalles"
            int rightValW = Math.Max(40, linkX - valorX - 8);
            labelEstadoProyecto.Width    = rightValW;
            labelUbicacionProyecto.Width = rightValW;

            if (_badgeFaltantes != null && _badgeFaltantes.Visible)
                PositionBadge();
        }

        private static string FormatEstado(EnumEstado estado)
        {
            switch (estado)
            {
                case EnumEstado.EnProceso:  return LanguageService.Current?.T("val_estado_en_proceso") ?? "En proceso";
                case EnumEstado.Suspendido: return LanguageService.Current?.T("val_estado_suspendido") ?? "Suspendido";
                case EnumEstado.Finalizado: return LanguageService.Current?.T("val_estado_finalizado") ?? "Finalizado";
                default:                    return estado.ToString();
            }
        }

        private void linkLabelVerDetalles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_proyecto == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_proyecto_sin_info") ?? "No se ha asignado información del proyecto.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!PermisosUI.Require(Services.Login.TipoPermiso.VER_PROYECTOS))
                return;

            // Cambiar la vista en el MainForm
            mainForm.addUserControl(new DetalleProyectoControl(mainForm, _proyecto));
        }
    }

}

