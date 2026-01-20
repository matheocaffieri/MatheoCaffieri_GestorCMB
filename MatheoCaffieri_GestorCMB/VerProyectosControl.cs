using BL;
using DomainModel;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.RoleService;
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

        private const string REQUIRED = "VER_PROYECTOS";


        public VerProyectosControl(MainForm mainForm)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show("No se encontró el MainForm para navegar.", "Atención",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                host.addUserControl(new HomeControl(mainForm));
                return;
            }

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


        public void Refrescar()
        {
            ObtenerProyectosItems(); // tu método que consulta y repinta los items
        }



        private void VerProyectosControl_Load(object sender, EventArgs e)
        {
            ObtenerProyectosItems();
        }
    }
}
