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

        private readonly IGenericRepository<Empleado> _empleadoRepo = new EmpleadoBL();

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

            WireEvents();
        }

        private void WireEvents()
        {
            if (buttonSearchEmpleado != null)
                buttonSearchEmpleado.Click += buttonSearchEmpleado_Click;

            if (textBox1 != null)
            {
                textBox1.TextChanged += textBox1_TextChanged;
                textBox1.KeyDown += textBox1_KeyDown;
            }
        }

        private void CargarListado(string filtro = "")
        {
            List<Empleado> empleados = _empleadoRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();

                empleados = empleados
                    .Where(e =>
                        (!string.IsNullOrEmpty(e.Nombre) && e.Nombre.ToLower().Contains(filtro)) ||
                        (!string.IsNullOrEmpty(e.Apellido) && e.Apellido.ToLower().Contains(filtro)) ||
                        e.NroDocumento.ToString().Contains(filtro) ||
                        e.Sueldo.ToString().Contains(filtro)
                    )
                    .ToList();
            }

            empleadosLayoutPanel.SuspendLayout();
            empleadosLayoutPanel.Controls.Clear();

            foreach (var emp in empleados)
            {
                var item = CrearItemEmpleado(emp);
                empleadosLayoutPanel.Controls.Add(item);
            }

            empleadosLayoutPanel.ResumeLayout();
        }

        private EmpleadosItemControl CrearItemEmpleado(Empleado emp)
        {
            var item = new EmpleadosItemControl();
            item.Bind(emp);

            item.ActiveChanged += (e, nuevoEstado) =>
            {
                if (e == null || e.IdEmpleado == Guid.Empty) return;
                try
                {
                    _empleadoRepo.Update(e);
                }
                catch (Exception ex)
                {
                    Services.Logs.LoggerLogic.Error($"[VerEmpleadosControl] Error al actualizar estado: {ex.Message}");
                    MessageBox.Show(
                        LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CargarListado(textBox1.Text);
                }
            };

            item.EditRequested += (e) =>
            {
                if (e == null || e.IdEmpleado == Guid.Empty) return;

                using (var frm = new EditEmpleadoForm(_empleadoRepo, e))
                {
                    var owner = this.FindForm();
                    var dr = (owner != null) ? frm.ShowDialog(owner) : frm.ShowDialog();

                    if (dr == DialogResult.OK)
                        CargarListado(textBox1.Text);
                }
            };

            return item;
        }

        private void VerEmpleadosControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                CargarListado();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Width <= 0) return;

            const int rightMargin = 30;

            buttonAgregarEmpleado.Left = Width - buttonAgregarEmpleado.Width - rightMargin;
            buttonSearchEmpleado.Left  = Width - buttonSearchEmpleado.Width  - rightMargin;
            textBox1.Width = buttonSearchEmpleado.Left - textBox1.Left - 4;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CargarListado(textBox1.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CargarListado(textBox1.Text);
            }
        }

        private void buttonSearchEmpleado_Click(object sender, EventArgs e)
        {
            CargarListado(textBox1.Text);
        }

        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {
            using (var addEmpleadosForm = new AddEmpleadosForm(_empleadoRepo))
            {
                var owner = this.FindForm();
                DialogResult dr = (owner != null)
                    ? addEmpleadosForm.ShowDialog(owner)
                    : addEmpleadosForm.ShowDialog();

                if (dr == DialogResult.OK)
                    CargarListado(textBox1.Text);
            }
        }
    }
}
