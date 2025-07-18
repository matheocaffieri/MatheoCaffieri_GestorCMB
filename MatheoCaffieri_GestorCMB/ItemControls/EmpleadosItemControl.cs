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
    public partial class EmpleadosItemControl : UserControl
    {
        public EmpleadosItemControl()
        {
            InitializeComponent();
        }

        public string Info
        {
            get => labelInfoEmpleado.Text;
            set => labelInfoEmpleado.Text = value;
        }
    }
}
