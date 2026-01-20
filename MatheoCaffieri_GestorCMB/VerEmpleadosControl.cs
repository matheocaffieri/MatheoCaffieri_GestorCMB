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
    public partial class VerEmpleadosControl : UserControl
    {

        private const string REQUIRED = "VER_EMPLEADOS";


        public VerEmpleadosControl()
        {
            InitializeComponent();


            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void ObtenerEmpleadosItems()
        {
            List<Empleado> empleados = ((IGenericRepository<Empleado>)new EmpleadoBL()).GetAll();

            empleadosLayoutPanel.Controls.Clear();

            empleados.ForEach(e =>
            {
                EmpleadosItemControl empleadosItemControl = new EmpleadosItemControl
                {
                    Info = e.Nombre + " " + e.Apellido + " | DNI: " + e.NroDocumento + " | $" + e.Sueldo + " | Proyectos activos: " + e.CantidadProyectosActivos
                };

                // Agregas el control al LayoutPanel
                empleadosLayoutPanel.Controls.Add(empleadosItemControl);
            });
        }

        private void VerEmpleadosControl_Load(object sender, EventArgs e)
        {
            ObtenerEmpleadosItems();
        }

        private readonly IGenericRepository<Empleado> _empleadoRepo = new EmpleadoBL();


        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {

            // Pasás el mismo repo al form
            using (var addEmpleadosForm = new AddEmpleadosForm(_empleadoRepo))
            {
                var owner = this.FindForm();
                DialogResult dr = (owner != null)
                    ? addEmpleadosForm.ShowDialog(owner)
                    : addEmpleadosForm.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    // Se agregó correctamente: recargás la lista
                    ObtenerEmpleadosItems();
                }
            }
        }
    }
}
