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
    public partial class MaterialFaltanteItemControl : UserControl
    {
        public MaterialFaltanteItemControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
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
            int rowHeight = 17;
            int rowY = Math.Max(0, (ClientSize.Height - rowHeight) / 2);
            MultiColumnRowLayout.Layout(
                new List<MultiColumnRowLayout.ColumnSpec>
                {
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoDescripcionArtFalt, MinWidth = 100, Weight = 38, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoTipoArtFalt,        MinWidth = 75,  Weight = 24, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoUnidadFalt,         MinWidth = 40,  Weight = 13, Align = ContentAlignment.MiddleCenter },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoCantidadFaltante,   MinWidth = 80,  Weight = 25, Align = ContentAlignment.MiddleRight  },
                },
                new List<Label> { label1, label2, label3 },
                totalWidth: ClientSize.Width,
                paddingX:   4,
                sepWidth:   10,
                rowY:       rowY,
                rowHeight:  rowHeight);
            ResumeLayout(false);
        }

        public string DescripcionArticuloFaltante
        {
            get => labelInfoDescripcionArtFalt.Text;
            set => labelInfoDescripcionArtFalt.Text = value ?? "Sin descripción";
        }

        public string TipoArticuloFaltante
        {
            get => labelInfoTipoArtFalt.Text;
            set => labelInfoTipoArtFalt.Text = value ?? "Sin tipo";
        }

        public string TipoUnidadArticuloFaltante
        {
            get => labelInfoUnidadFalt.Text;
            set => labelInfoUnidadFalt.Text = value ?? "Sin tipo de unidad";
        }

        public string CantidadArticuloFaltante
        {
            get => labelInfoCantidadFaltante.Text;
            set => labelInfoCantidadFaltante.Text = value ?? "Sin cantidad";
        }
    }
}
