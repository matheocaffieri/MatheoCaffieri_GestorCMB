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
    public partial class VerEmpleadosControl : UserControl
    {
        public VerEmpleadosControl()
        {
            InitializeComponent();
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

        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {
            AddEmpleadosForm addEmpleadosForm = new AddEmpleadosForm();
            addEmpleadosForm.Show();
        }
    }
}
