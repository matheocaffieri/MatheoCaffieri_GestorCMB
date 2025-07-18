using MatheoCaffieri_GestorCMB.ItemControls;
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
    public partial class ClientesControl : UserControl
    {
        public ClientesControl()
        {
            InitializeComponent();
        }

        private void ObtenerClientesItems()
        {
            ClientesItemControl[] clienteItemControls = new ClientesItemControl[5];
            for (int i = 0; i < clienteItemControls.Length; i++)
            {
                clienteItemControls[i] = new ClientesItemControl();
                //clienteItemControls[i].Dock = DockStyle.Top;
                //clienteItemControls[i].BringToFront();

                

                gestionarClientesLayoutPanel.Controls.Add(clienteItemControls[i]);

            }
        }

        private void ClientesControl_Load(object sender, EventArgs e)
        {
            ObtenerClientesItems();
        }
    }
}
