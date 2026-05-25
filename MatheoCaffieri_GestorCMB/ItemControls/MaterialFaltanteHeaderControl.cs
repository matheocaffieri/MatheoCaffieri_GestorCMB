using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class MaterialFaltanteHeaderControl : UserControl
    {
        public MaterialFaltanteHeaderControl()
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
            MultiColumnRowLayout.Layout(
                new List<MultiColumnRowLayout.ColumnSpec>
                {
                    new MultiColumnRowLayout.ColumnSpec { Label = labelDescripcion, MinWidth = 100, Weight = 38, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelCategoria,   MinWidth = 75,  Weight = 24, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelUnidad,      MinWidth = 40,  Weight = 13, Align = ContentAlignment.MiddleCenter },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelCantidad,    MinWidth = 80,  Weight = 25, Align = ContentAlignment.MiddleRight  },
                },
                new List<Label> { labelSep1, labelSep2, labelSep3 },
                totalWidth: ClientSize.Width,
                paddingX:   4,
                sepWidth:   10,
                rowY:       0,
                rowHeight:  ClientSize.Height);
            ResumeLayout(false);
        }
    }
}
