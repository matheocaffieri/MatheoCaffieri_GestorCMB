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
    public partial class ProveedorItemControl : UserControl
    {
        public ProveedorItemControl()
        {
            InitializeComponent();
        }

        public string Descripcion
        {
            get => labelDescripcionProveedor.Text; 
            set => labelDescripcionProveedor.Text = value; 
        }

        public string Telefono
        {
            get => labelTelefonoProveedor.Text;
            set => labelTelefonoProveedor.Text = value;
        }
    }
}
