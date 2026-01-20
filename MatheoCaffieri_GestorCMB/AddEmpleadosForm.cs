using BL;
using DomainModel;
using DomainModel.Interfaces;
using Services.RoleService;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AddEmpleadosForm : Form
    {
        private const string REQUIRED = "CARGAR_EMPLEADOS";

        private readonly IGenericRepository<Empleado> _empleadoRepo;

        public Point mouseLocation;

        public AddEmpleadosForm() : this(new EmpleadoBL())
        {
            // acá NO va InitializeComponent, ya lo llama el otro ctor
        }

        public AddEmpleadosForm(IGenericRepository<Empleado> empleadoRepo)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            _empleadoRepo = empleadoRepo ?? throw new ArgumentNullException(nameof(empleadoRepo));
        }



        private void FormPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }

        private void FormPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void buttonExitAE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            // 1. Validaciones básicas
            string nombre = textBoxNombre.Text.Trim();
            string apellido = textBoxApellido.Text.Trim();
            string documentoStr = textBoxDocumento.Text.Trim();
            string sueldoStr = textBoxSueldo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(documentoStr) ||
                string.IsNullOrWhiteSpace(sueldoStr))
            {
                MessageBox.Show("Complete todos los campos.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar y parsear documento a int
            if (!int.TryParse(documentoStr, NumberStyles.Integer, CultureInfo.CurrentCulture, out int documento) ||
                documento < 0)
            {
                MessageBox.Show("El número de documento no es válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(sueldoStr, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal sueldo) ||
                sueldo < 0)
            {
                MessageBox.Show("El sueldo no es válido.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Crear el objeto de dominio
            var empleado = new Empleado
            {
                Nombre = nombre,
                Apellido = apellido,
                NroDocumento = documento,
                Sueldo = (float)sueldo
                // agregá acá otros campos si tu entidad los tiene
            };

            try
            {
                // 3. Guardar usando la BL / repositorio
                _empleadoRepo.Add(empleado);

                // 4. Avisar al llamador que todo salió bien
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el empleado:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
