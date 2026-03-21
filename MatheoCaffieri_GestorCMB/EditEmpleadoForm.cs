using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;

namespace MatheoCaffieri_GestorCMB
{
    public partial class EditEmpleadoForm : Form
    {
        private readonly IGenericRepository<Empleado> _repo;
        private readonly Empleado _empleado;

        // Constructor para edición
        public EditEmpleadoForm(IGenericRepository<Empleado> repo, Empleado empleado)
        {
            InitializeComponent();

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _empleado = empleado ?? throw new ArgumentNullException(nameof(empleado));

            if (_empleado.IdEmpleado == Guid.Empty)
                throw new ArgumentException("IdEmpleado requerido.", nameof(empleado));

            // Precargar campos
            textBoxNombre.Text = _empleado.Nombre;
            textBoxApellido.Text = _empleado.Apellido;
            textBoxDocumento.Text = _empleado.NroDocumento.ToString();
            textBoxSueldo.Text = _empleado.Sueldo.ToString(CultureInfo.CurrentCulture);

            // Hook botón guardar/editar (si no lo tenés en Designer)
            buttonEditar.Click += buttonEditar_Click;
        }

      

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            var nombre = textBoxNombre.Text.Trim();
            var apellido = textBoxApellido.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show(LanguageService.Current?.T("val_nombre_requerido") ?? "Nombre requerido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show(LanguageService.Current?.T("val_apellido_requerido") ?? "Apellido requerido.");
                return;
            }

            if (!int.TryParse(textBoxDocumento.Text.Trim(), out int dni))
            {
                MessageBox.Show(LanguageService.Current?.T("val_dni_invalido") ?? "DNI inválido.");
                return;
            }

            if (!float.TryParse(textBoxSueldo.Text.Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out float sueldo) &&
                !float.TryParse(textBoxSueldo.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out sueldo))
            {
                MessageBox.Show(LanguageService.Current?.T("val_sueldo_invalido") ?? "Sueldo inválido.");
                return;
            }

            try
            {
                // Actualizar el mismo objeto (mantiene IdEmpleado)
                _empleado.Nombre = nombre;
                _empleado.Apellido = apellido;
                _empleado.NroDocumento = dni;
                _empleado.Sueldo = sueldo;

                _repo.Update(_empleado);

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
