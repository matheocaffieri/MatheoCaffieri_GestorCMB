using Services.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class DetalleEmpleadoHeaderControl : UserControl
    {
        public DetalleEmpleadoHeaderControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            AplicarTraducciones();
            ApplyLayout();
        }

        private void AplicarTraducciones()
        {
            labelNombre.Text    = LanguageService.Current?.T("lbl_nombre")    ?? labelNombre.Text;
            labelDocumento.Text = LanguageService.Current?.T("hdr_documento") ?? labelDocumento.Text;
            labelSueldo.Text    = LanguageService.Current?.T("lbl_sueldo")    ?? labelSueldo.Text;
            labelGanancia.Text  = LanguageService.Current?.T("hdr_ganancia")  ?? labelGanancia.Text;
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
                    new MultiColumnRowLayout.ColumnSpec { Label = labelNombre,    MinWidth = 100, Weight = 35, Align = ContentAlignment.MiddleLeft  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelDocumento, MinWidth = 75,  Weight = 18, Align = ContentAlignment.MiddleRight },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelSueldo,    MinWidth = 80,  Weight = 22, Align = ContentAlignment.MiddleRight },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelGanancia,  MinWidth = 80,  Weight = 25, Align = ContentAlignment.MiddleRight },
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
