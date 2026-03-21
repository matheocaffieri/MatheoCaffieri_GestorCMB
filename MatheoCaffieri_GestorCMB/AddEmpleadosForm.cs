using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;
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
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
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
                MessageBox.Show(
                    LanguageService.Current?.T("val_campos_completos") ?? "Complete todos los campos.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar y parsear documento a int
            if (!int.TryParse(documentoStr, NumberStyles.Integer, CultureInfo.CurrentCulture, out int documento) ||
                documento < 0)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_dni_invalido") ?? "El número de documento no es válido.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(sueldoStr, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal sueldo) ||
                sueldo < 0)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_sueldo_invalido") ?? "El sueldo no es válido.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
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
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
