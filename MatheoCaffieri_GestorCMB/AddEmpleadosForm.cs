using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AddEmpleadosForm : Form
    {

        public Point mouseLocation;

        public AddEmpleadosForm()
        {
            InitializeComponent();
        }

        private void FormPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }

        private void FormPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void buttonExitAE_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
