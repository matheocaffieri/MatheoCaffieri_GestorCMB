using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class DetalleMaterialItemControl : UserControl
    {
        public Guid IdMaterial { get; set; }

        public DetalleMaterialItemControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            foreach (Control c in Controls)
                c.DoubleClick += (s, e) => OnDoubleClick(e);
            ApplyLayout();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyLayout();
        }

        private void ApplyLayout()
        {
            SuspendLayout();
            MultiColumnRowLayout.Layout(
                new List<MultiColumnRowLayout.ColumnSpec>
                {
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoDescripcionArticulo, MinWidth = 70, Weight = 24, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoTipoArt,             MinWidth = 60, Weight = 18, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoTipoUnidad,          MinWidth = 40, Weight = 10, Align = ContentAlignment.MiddleCenter },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoCantidad,            MinWidth = 35, Weight = 8,  Align = ContentAlignment.MiddleRight  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoCosto,               MinWidth = 70, Weight = 20, Align = ContentAlignment.MiddleRight  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoValorGananciaMat,    MinWidth = 65, Weight = 20, Align = ContentAlignment.MiddleRight  },
                },
                new List<Label> { label1, label2, label3, label4, label5 },
                totalWidth: ClientSize.Width,
                paddingX:   4,
                sepWidth:   8,
                rowY:       10,
                rowHeight:  17);
            ResumeLayout(false);
        }

        public string InfoDescripcionArticulo
        {
            get => labelInfoDescripcionArticulo.Text;
            set => labelInfoDescripcionArticulo.Text = value ?? "Material #N/A";
        }

        public string InfoCantidad
        {
            get => labelInfoCantidad.Text;
            set => labelInfoCantidad.Text = value ?? "Cantidad #N/A";
        }

        public string InfoTipoArticulo
        {
            get => labelInfoTipoArt.Text;
            set => labelInfoTipoArt.Text = value ?? "Tipo Artículo #N/A";
        }

        public string InfoTipoUnidad
        {
            get => labelInfoTipoUnidad.Text;
            set => labelInfoTipoUnidad.Text = value ?? "Tipo Unidad #N/A";
        }

        public string InfoCosto
        {
            get => labelInfoCosto.Text;
            set => labelInfoCosto.Text = value ?? "Costo #N/A";
        }

        public string InfoValorGananciaMat
        {
            get => labelInfoValorGananciaMat.Text;
            set => labelInfoValorGananciaMat.Text = value ?? "Valor Ganancia #N/A";
        }

    }
}
