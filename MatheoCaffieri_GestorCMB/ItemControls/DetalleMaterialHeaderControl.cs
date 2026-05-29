using Services.Language;
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
            AplicarTraducciones();
            ApplyLayout();
        }

        private void AplicarTraducciones()
        {
            labelDescripcion.Text = LanguageService.Current?.T("hdr_descripcion")      ?? labelDescripcion.Text;
            labelCategoria.Text   = LanguageService.Current?.T("hdr_categoria")        ?? labelCategoria.Text;
            labelUnidad.Text      = LanguageService.Current?.T("hdr_unidad")           ?? labelUnidad.Text;
            labelCantidad.Text    = LanguageService.Current?.T("hdr_cantidad_abr")     ?? labelCantidad.Text;
            labelPrecio.Text      = LanguageService.Current?.T("hdr_precio_unidad")    ?? labelPrecio.Text;
            labelGanancia.Text    = LanguageService.Current?.T("hdr_ganancia_unidad")  ?? labelGanancia.Text;
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
