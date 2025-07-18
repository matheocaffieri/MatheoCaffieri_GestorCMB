using BL;
using DomainModel;
using DomainModel.Interfaces;
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
    public partial class VerProyectosControl : UserControl
    {
        private MainForm mainForm;

        public VerProyectosControl(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }


        private void ObtenerProyectosItems()
        {
            List<Proyecto> proyectos = ((IGenericRepository<Proyecto>)new ProyectoBL()).GetAll();
            proyectoItemPanel.Controls.Clear();

            proyectos.ForEach(p =>
            {
                ProyectoItemControl proyectoItemControl = new ProyectoItemControl(mainForm, p);
                proyectoItemPanel.Controls.Add(proyectoItemControl);
            });
        }





        private void VerProyectosControl_Load(object sender, EventArgs e)
        {
            ObtenerProyectosItems();
        }
    }
}
