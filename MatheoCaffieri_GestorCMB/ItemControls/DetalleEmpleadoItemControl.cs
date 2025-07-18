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
        public DetalleEmpleadoItemControl()
        {
            InitializeComponent();
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
