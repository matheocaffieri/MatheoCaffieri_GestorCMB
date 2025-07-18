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
    public partial class HomeControl : UserControl
    {
        private MainForm mainForm;

        public HomeControl(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void butMainProyectos_Click(object sender, EventArgs e)
        {
            VerProyectosControl verProyectosControl = new VerProyectosControl(mainForm);
            mainForm.addUserControl(verProyectosControl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VerInventarioControl verInventarioControl = new VerInventarioControl();
            mainForm.addUserControl(verInventarioControl);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerEmpleadosControl verEmpleadosControl = new VerEmpleadosControl();
            mainForm.addUserControl(verEmpleadosControl);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClientesControl clientesControl = new ClientesControl();
            mainForm.addUserControl(clientesControl);
        }
    }
}
