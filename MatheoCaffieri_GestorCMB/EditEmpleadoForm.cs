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
using Services.Logs;
// Las claves de traducción para los labels del Designer están en Properties/Resources.resx (btn_editar, lbl_*, cap_editar_empleado).

namespace MatheoCaffieri_GestorCMB
{
    public partial class EditEmpleadoForm : Form
    {
        private readonly IGenericRepository<Empleado> _repo;
        private readonly Empleado _empleado;

        // Constructor para edición
        private System.Drawing.Point _mouseLocation;

        public EditEmpleadoForm(IGenericRepository<Empleado> repo, Empleado empleado)
        {
            InitializeComponent();

            FormPanel.MouseDown += (s, e) => { _mouseLocation = new System.Drawing.Point(-e.X, -e.Y); };
            FormPanel.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pos = MousePosition;
                    pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                    Location = pos;
                }
            };

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _empleado = empleado ?? throw new ArgumentNullException(nameof(empleado));

            if (_empleado.IdEmpleado == Guid.Empty)
                throw new ArgumentException("IdEmpleado requerido.", nameof(empleado));

            AplicarTraducciones();

            // Precargar campos
            textBoxNombre.Text = _empleado.Nombre;
            textBoxApellido.Text = _empleado.Apellido;
            textBoxDocumento.Text = _empleado.NroDocumento.ToString();
            textBoxSueldo.Text = _empleado.Sueldo.ToString(CultureInfo.CurrentCulture);

            buttonEditar.Click  += buttonEditar_Click;
            buttonExitAE.Click  += (s, ev) => Close();
            buttonExit.Click    += (s, ev) => Close();
        }

        private void AplicarTraducciones()
        {
            buttonEditar.Text = LanguageService.Current?.T("btn_editar")           ?? buttonEditar.Text;
            label1.Text       = LanguageService.Current?.T("lbl_nombre")           ?? label1.Text;
            label2.Text       = LanguageService.Current?.T("cap_editar_empleado")  ?? label2.Text;
            label3.Text       = LanguageService.Current?.T("lbl_apellido")         ?? label3.Text;
            label4.Text       = LanguageService.Current?.T("lbl_numero_documento") ?? label4.Text;
            label5.Text       = LanguageService.Current?.T("lbl_sueldo")           ?? label5.Text;
        }

      

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            var nombre = textBoxNombre.Text.Trim();
            var apellido = textBoxApellido.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                LoggerLogic.Warn($"[EditEmpleadoForm] Validación: nombre vacío (Id={_empleado.IdEmpleado}).");
                MessageBox.Show(LanguageService.Current?.T("val_nombre_requerido") ?? "Nombre requerido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(apellido))
            {
                LoggerLogic.Warn($"[EditEmpleadoForm] Validación: apellido vacío (Id={_empleado.IdEmpleado}).");
                MessageBox.Show(LanguageService.Current?.T("val_apellido_requerido") ?? "Apellido requerido.");
                return;
            }

            if (!int.TryParse(textBoxDocumento.Text.Trim(), out int dni) || dni <= 0)
            {
                LoggerLogic.Warn($"[EditEmpleadoForm] Validación: DNI inválido (Id={_empleado.IdEmpleado}).");
                MessageBox.Show(LanguageService.Current?.T("val_dni_invalido") ?? "DNI inválido.");
                return;
            }

            if ((!float.TryParse(textBoxSueldo.Text.Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out float sueldo) &&
                 !float.TryParse(textBoxSueldo.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out sueldo)) ||
                sueldo < 0)
            {
                LoggerLogic.Warn($"[EditEmpleadoForm] Validación: sueldo inválido (Id={_empleado.IdEmpleado}).");
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
                LoggerLogic.Warn($"[EditEmpleadoForm] Validación al actualizar empleado: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EditEmpleadoForm] Falla al actualizar empleado. Id={_empleado.IdEmpleado}", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
