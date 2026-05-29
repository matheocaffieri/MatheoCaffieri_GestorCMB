using DomainModel;
using Services.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class HistorialInformeItemControl : UserControl
    {
        private Panel _panelDetalle;

        public HistorialInformeItemControl()
        {
            InitializeComponent();
            EstilarCard();
        }

        public void Bind(string fecha, string nombreProyecto, string estado, List<MaterialFaltante> faltantes)
        {
            labelFecha.Text = fecha;
            labelNombre.Text = nombreProyecto;

            var estadoNormalizado = (estado ?? string.Empty).Trim().ToLowerInvariant();
            if (estadoNormalizado == "finalizado")
            {
                labelEstado.Text = LanguageService.Current?.T("val_transaccion_completa") ?? "Transacción completa";
                labelEstado.BackColor = Color.FromArgb(76, 175, 80);
            }
            else
            {
                labelEstado.Text = LanguageService.Current?.T("val_transaccion_cancelada") ?? "Transacción cancelada";
                labelEstado.BackColor = Color.FromArgb(229, 57, 53);
            }

            if (faltantes != null && faltantes.Count > 0)
            {
                var sinDesc = LanguageService.Current?.T("txt_sin_descripcion_paren") ?? "(sin descripción)";
                var sb = new StringBuilder();
                foreach (var f in faltantes)
                {
                    var desc = string.IsNullOrWhiteSpace(f.DescripcionArticuloFaltante) ? sinDesc : f.DescripcionArticuloFaltante;
                    var tipo = string.IsNullOrWhiteSpace(f.TipoMaterialFaltante) ? "-" : f.TipoMaterialFaltante;
                    var unidad = string.IsNullOrWhiteSpace(f.TipoUnidadMaterialFaltante) ? "" : f.TipoUnidadMaterialFaltante;
                    sb.AppendLine($"• {f.CantidadFaltante} {unidad} - {desc} ({tipo})");
                }

                textBoxMateriales.Text = sb.ToString();
            }
            else
            {
                textBoxMateriales.Text = estadoNormalizado == "finalizado"
                    ? (LanguageService.Current?.T("txt_materiales_aplicados") ?? "Materiales aplicados al inventario del proyecto.")
                    : (LanguageService.Current?.T("txt_sin_detalle")          ?? "(sin detalle)");
            }
        }

        private void EstilarCard()
        {
            BackColor = Color.White;
            Margin = new Padding(6);

            labelFecha.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
            labelFecha.ForeColor = Color.FromArgb(30, 30, 30);
            labelFecha.BackColor = Color.Transparent;

            labelNombre.Font = new Font("Microsoft YaHei UI", 10f);
            labelNombre.ForeColor = Color.FromArgb(100, 100, 110);
            labelNombre.BackColor = Color.Transparent;

            labelEstado.Font = new Font("Microsoft YaHei UI", 8.5f, FontStyle.Bold);
            labelEstado.ForeColor = Color.White;
            labelEstado.TextAlign = ContentAlignment.MiddleCenter;

            textBoxMateriales.BackColor = Color.FromArgb(244, 246, 250);
            textBoxMateriales.BorderStyle = BorderStyle.None;
            textBoxMateriales.Font = new Font("Microsoft YaHei UI", 8.7f);
            textBoxMateriales.ForeColor = Color.FromArgb(75, 75, 85);
            textBoxMateriales.ReadOnly = true;
            textBoxMateriales.Multiline = true;
            textBoxMateriales.ScrollBars = ScrollBars.Vertical;

            if (_panelDetalle == null)
            {
                _panelDetalle = new Panel
                {
                    BackColor = Color.FromArgb(244, 246, 250)
                };
                Controls.Add(_panelDetalle);
                _panelDetalle.Controls.Add(textBoxMateriales);
            }

            Resize += (s, e) =>
            {
                if (Width > 10 && Height > 10)
                    using (var path = MakeCardPath(new Rectangle(1, 1, Width - 2, Height - 2), 10))
                        Region = new Region(path);

                int left = 10;
                int right = 10;
                int panelY = labelEstado.Bottom + 8;
                int panelH = Math.Max(44, Height - panelY - 10);

                _panelDetalle.SetBounds(left, panelY, Math.Max(80, Width - left - right), panelH);
                textBoxMateriales.SetBounds(8, 7, Math.Max(60, _panelDetalle.Width - 16), Math.Max(20, _panelDetalle.Height - 14));

                using (var pathPanel = MakeCardPath(new Rectangle(0, 0, _panelDetalle.Width - 1, _panelDetalle.Height - 1), 7))
                    _panelDetalle.Region = new Region(pathPanel);
            };

            Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeCardPath(new Rectangle(0, 0, Width - 1, Height - 1), 10))
                using (var pen = new Pen(Color.FromArgb(218, 218, 225)))
                    e.Graphics.DrawPath(pen, path);
            };
        }

        private static GraphicsPath MakeCardPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
