using BL;
using DomainModel;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
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
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void ObtenerEmpleadosItems()
        {
            var empleados = ((IGenericRepository<Empleado>)new EmpleadoBL()).GetAll();

            empleadosLayoutPanel.Controls.Clear();

            foreach (var e in empleados)
            {
                var item = new EmpleadosItemControl();
                item.Bind(e);

                // Switch on/off
                item.ActiveChanged += (emp, nuevoEstado) =>
                {
                    if (emp == null || emp.IdEmpleado == Guid.Empty) return;
                    new EmpleadoBL().Update(emp);
                };

                // Click en el lápiz
                item.EditRequested += (emp) =>
                {
                    if (emp == null || emp.IdEmpleado == Guid.Empty) return;

                    using (var frm = new EditEmpleadoForm(_empleadoRepo, emp))
                    {
                        var owner = this.FindForm();
                        var dr = (owner != null) ? frm.ShowDialog(owner) : frm.ShowDialog();

                        if (dr == DialogResult.OK)
                            ObtenerEmpleadosItems(); // recarga lista con datos actualizados
                    }
                };

                empleadosLayoutPanel.Controls.Add(item);
            }
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
