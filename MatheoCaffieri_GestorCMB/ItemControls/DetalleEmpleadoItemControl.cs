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
    public partial class DetalleEmpleadoItemControl : UserControl
    {
        public Guid IdEmpleado { get; set; }

        public DetalleEmpleadoItemControl()
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
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoNombreApellido,    MinWidth = 100, Weight = 35, Align = ContentAlignment.MiddleLeft  },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoNroDocumento,      MinWidth = 75,  Weight = 18, Align = ContentAlignment.MiddleRight },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoSueldo,            MinWidth = 80,  Weight = 22, Align = ContentAlignment.MiddleRight },
                    new MultiColumnRowLayout.ColumnSpec { Label = labelInfoValorGananciaEmp,  MinWidth = 80,  Weight = 25, Align = ContentAlignment.MiddleRight },
                },
                new List<Label> { label1, label2, label3 },
                totalWidth: ClientSize.Width,
                paddingX:   4,
                sepWidth:   10,
                rowY:       10,
                rowHeight:  17);
            ResumeLayout(false);
        }

        public string InfoNombreApellido
        {
            get => labelInfoNombreApellido.Text;
            set => labelInfoNombreApellido.Text = value ?? "Empleado #N/A";
        }

        public string InfoNroDocumento
        {
            get => labelInfoNroDocumento.Text;
            set => labelInfoNroDocumento.Text = value ?? "Nro. Documento #N/A";
        }

        public string InfoSueldo
        {
            get => labelInfoSueldo.Text;
            set => labelInfoSueldo.Text = value ?? "Sueldo #N/A";
        }

        public string InfoValorGananciaEmp
        {
            get => labelInfoValorGananciaEmp.Text;
            set => labelInfoValorGananciaEmp.Text = value ?? "Valor Ganancia #N/A";
        }
    }
}
