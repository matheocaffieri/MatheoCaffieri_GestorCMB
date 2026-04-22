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
            set => labelNumProyecto.Text = value ?? "Proyecto #N/A";
        }

        public string DescripcionProyecto
        {
            get => labelDescripcionProyecto.Text;
            set => labelDescripcionProyecto.Text = value ?? "Sin descripción";
        }

        public string NombreCliente
        {
            get => labelNombreCliente.Text;
            set => labelNombreCliente.Text = value ?? "Desconocido";
        }

        public string FechaInicio
        {
            get => labelFechaInicio.Text;
            set => labelFechaInicio.Text = value ?? "Fecha no disponible";
        }

        public string EstadoProyecto
        {
            get => labelEstadoProyecto.Text;
            set => labelEstadoProyecto.Text = value ?? "Estado desconocido";
        }

        public string UbicacionProyecto
        {
            get => labelUbicacionProyecto.Text;
            set => labelUbicacionProyecto.Text = value ?? "Ubicación no especificada";
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

            NumProyecto = "Proyecto #";
            DescripcionProyecto = proyecto.Descripcion;
            NombreCliente = proyecto.Cliente?.NombreContacto ?? "Cliente desconocido";
            FechaInicio = proyecto.FechaInicio.ToString("dd/MM/yyyy") ?? "Sin fecha";
            EstadoProyecto = FormatEstado(proyecto.Estado);
            UbicacionProyecto = proyecto.Ubicacion ?? "Ubicación desconocida";

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
                using (var font = new Font("Microsoft YaHei UI", 7.5f, FontStyle.Bold))
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

        public void SetFaltantesCount(int count)
        {
            _faltantesCount = count;
            if (count > 0)
            {
                // ancho: "!1"→30, "!12"→36, "!99+"→44
                _badgeFaltantes.Width  = count > 99 ? 44 : count > 9 ? 36 : 30;
                _badgeFaltantes.Height = 22;
                _badgeFaltantes.Visible = true;
                _badgeFaltantes.BringToFront();
                _tipBadge.SetToolTip(_badgeFaltantes, $"{count} material{(count == 1 ? "" : "es")} faltante{(count == 1 ? "" : "s")}");
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
                case EnumEstado.EnProceso:  return "En proceso";
                case EnumEstado.Suspendido: return "Suspendido";
                case EnumEstado.Finalizado: return "Finalizado";
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

            // Cambiar la vista en el MainForm
            mainForm.addUserControl(new DetalleProyectoControl(mainForm, _proyecto));
        }
    }

}

