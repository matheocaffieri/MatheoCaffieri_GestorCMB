using DomainModel;
using Services.Language;
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

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class InformeCompraItemControl : UserControl
    {
        private string _numeroProyecto;
        private string _nombreProyecto;

        [Browsable(false)]
        public Guid IdProyecto { get; private set; }

        // Guarda el último informe generado o el informe “activo”
        [Browsable(false)]
        public Guid? IdInformeCompra { get; private set; }

        public event EventHandler<Guid> AgregarCompraClicked;
        public event EventHandler<Guid> EliminarClicked;

        public InformeCompraItemControl()
        {
            InitializeComponent();

            // Recomendado para que se vea bien el listado
            textBoxItemsFaltantes.Multiline = true;
            textBoxItemsFaltantes.ScrollBars = ScrollBars.Vertical;
            textBoxItemsFaltantes.ReadOnly = true;

            buttonAgregarCompra.Click += (s, e) => AgregarCompraClicked?.Invoke(this, IdProyecto);
            buttonEliminar.Click += (s, e) => EliminarClicked?.Invoke(this, IdProyecto);

            EstilarCard();
        }

        private void EstilarCard()
        {
            BackColor = Color.White;
            Margin    = new Padding(4, 4, 4, 4);

            labelNumeroProyecto.BackColor = Color.Transparent;
            labelNumeroProyecto.Font      = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
            labelNumeroProyecto.ForeColor = Color.FromArgb(30, 30, 30);

            labelNombreProyecto.BackColor = Color.Transparent;
            labelNombreProyecto.Font      = new Font("Microsoft YaHei UI", 10f);
            labelNombreProyecto.ForeColor = Color.FromArgb(100, 100, 110);

            textBoxItemsFaltantes.BackColor   = Color.White;
            textBoxItemsFaltantes.BorderStyle = BorderStyle.None;
            textBoxItemsFaltantes.Font        = new Font("Microsoft YaHei UI", 8.5f);
            textBoxItemsFaltantes.ForeColor   = Color.FromArgb(75, 75, 85);

            EstilarBoton(buttonAgregarCompra, Color.FromArgb(76, 175, 80),  Color.FromArgb(56, 142, 60));
            EstilarBoton(buttonEliminar,      Color.FromArgb(229, 57,  53), Color.FromArgb(198, 40,  40));

            Resize += (s, e) =>
            {
                if (Width > 10 && Height > 10)
                    using (var path = MakeCardPath(new Rectangle(1, 1, Width - 2, Height - 2), 10))
                        Region = new Region(path);

                int txLeft = textBoxItemsFaltantes.Left;
                if (Width > txLeft + 20)
                    textBoxItemsFaltantes.Width = Width - txLeft - 10;
            };

            Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeCardPath(new Rectangle(0, 0, Width - 1, Height - 1), 10))
                using (var pen  = new Pen(Color.FromArgb(218, 218, 225)))
                    e.Graphics.DrawPath(pen, path);
            };
        }

        private static void EstilarBoton(Button btn, Color color, Color hover)
        {
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize             = 0;
            btn.FlatAppearance.MouseOverBackColor     = hover;
            btn.Font      = new Font("Microsoft YaHei UI", 8.5f, FontStyle.Bold);
            btn.Cursor    = Cursors.Hand;

            Action aplicar = () =>
            {
                if (btn.Width > 0 && btn.Height > 0)
                    using (var path = MakeCardPath(new Rectangle(0, 0, btn.Width, btn.Height), 8))
                        btn.Region = new Region(path);
            };
            btn.Resize += (s, e) => aplicar();
            aplicar();
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

        [Category("Custom Props")]
        public string NumeroProyecto
        {
            get => _numeroProyecto;
            set
            {
                _numeroProyecto = value;
                labelNumeroProyecto.Text = value;
            }
        }

        [Category("Custom Props")]
        public string NombreProyecto
        {
            get => _nombreProyecto;
            set
            {
                _nombreProyecto = value;
                labelNombreProyecto.Text = value;
            }
        }

        public void BindProyecto(Guid idProyecto, string numero, string nombre)
        {
            IdProyecto = idProyecto;
            NumeroProyecto = numero;
            NombreProyecto = nombre;
        }

        public void SetInforme(Guid? idInformeCompra)
        {
            IdInformeCompra = idInformeCompra;
        }

        public void SetFaltantes(List<MaterialFaltante> faltantes)
        {
            if (faltantes == null || faltantes.Count == 0)
            {
                textBoxItemsFaltantes.Text = LanguageService.Current?.T("txt_sin_materiales_faltantes") ?? "Sin materiales faltantes.";
                buttonAgregarCompra.Enabled = false;
                return;
            }

            buttonAgregarCompra.Enabled = true;

            // Orden opcional para que quede prolijo
            var ordered = faltantes
                .OrderBy(x => x.TipoMaterialFaltante)
                .ThenBy(x => x.DescripcionArticuloFaltante)
                .ToList();

            var sb = new StringBuilder();
            foreach (var f in ordered)
            {
                var desc = string.IsNullOrWhiteSpace(f.DescripcionArticuloFaltante)
                    ? (LanguageService.Current?.T("txt_sin_descripcion_paren") ?? "(sin descripción)")
                    : f.DescripcionArticuloFaltante;
                var tipo = string.IsNullOrWhiteSpace(f.TipoMaterialFaltante) ? "-" : f.TipoMaterialFaltante;
                var unidad = string.IsNullOrWhiteSpace(f.TipoUnidadMaterialFaltante) ? "" : f.TipoUnidadMaterialFaltante;

                sb.AppendLine($"• {f.CantidadFaltante} {unidad} - {desc} ({tipo})");
            }

            textBoxItemsFaltantes.Text = sb.ToString();
        }
    }
}
