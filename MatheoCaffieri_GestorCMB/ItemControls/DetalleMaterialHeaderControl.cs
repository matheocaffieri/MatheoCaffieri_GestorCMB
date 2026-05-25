using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class DetalleMaterialHeaderControl : UserControl
    {
        public DetalleMaterialHeaderControl()
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
                    new MultiColumnRowLayout.ColumnSpec { Label = labelDescripcion, MinWidth = 70, Weight = 24, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelCategoria,   MinWidth = 60, Weight = 18, Align = ContentAlignment.MiddleLeft   },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelUnidad,      MinWidth = 40, Weight = 10, Align = ContentAlignment.MiddleCenter },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelCantidad,    MinWidth = 35, Weight = 8,  Align = ContentAlignment.MiddleRight  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelPrecio,      MinWidth = 70, Weight = 20, Align = ContentAlignment.MiddleRight  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelGanancia,    MinWidth = 65, Weight = 20, Align = ContentAlignment.MiddleRight  },
                },
                new List<Label> { labelSep1, labelSep2, labelSep3, labelSep4, labelSep5 },
                totalWidth: ClientSize.Width,
                paddingX:   4,
                sepWidth:   8,
                rowY:       0,
                rowHeight:  ClientSize.Height);
            ResumeLayout(false);
        }
    }
}
