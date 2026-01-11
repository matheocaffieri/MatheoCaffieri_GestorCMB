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
    public partial class InformesDeCompraControl : UserControl
    {
        public InformesDeCompraControl()
        {
            InitializeComponent();
        }

        private readonly MainForm _mainForm;

        private void ObtenerInformesItems()
        {
            InformeCompraItemControl[] informeCompraItemControls = new InformeCompraItemControl[5];

            for (int i = 0; i < informeCompraItemControls.Length; i++)
            {
                informeCompraItemControls[i] = new InformeCompraItemControl();
                //informeCompraItemControls[i].Dock = DockStyle.Top;
                //informeCompraItemControls[i].BringToFront();
                
                informeCompraItemControls[i].NumeroProyecto = "Proyecto #" + i;
                informeCompraItemControls[i].NombreProyecto = "Nombre de proyecto";

                
                    informeLayoutPanel.Controls.Add(informeCompraItemControls[i]);
                
            }
        }

        private void InformesDeCompraControl_Load(object sender, EventArgs e)
        {
            ObtenerInformesItems();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show("No se encontró el MainForm para navegar.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new VerInventarioControl());
        }
    }
}
