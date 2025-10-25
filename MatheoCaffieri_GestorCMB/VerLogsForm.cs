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
    public partial class VerLogsForm : Form
    {
        public Point mouseLocation;
        public VerLogsForm()
        {
            InitializeComponent();
        }

     

      

        private void buttonExitAM_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormPanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void FormPanel_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }
    }
}
