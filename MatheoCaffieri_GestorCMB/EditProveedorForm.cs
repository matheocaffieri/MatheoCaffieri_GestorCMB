using System;
using System.Windows.Forms;
using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;

namespace MatheoCaffieri_GestorCMB
{
    public partial class EditProveedorForm : Form
    {
        private readonly IGenericRepository<Proveedor> _repo;
        private readonly Proveedor _proveedor;

        public EditProveedorForm(IGenericRepository<Proveedor> repo, Proveedor proveedor)
        {
            InitializeComponent();

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _proveedor = proveedor ?? throw new ArgumentNullException(nameof(proveedor));

            if (_proveedor.IdProveedor == Guid.Empty)
                throw new ArgumentException("IdProveedor requerido.", nameof(proveedor));

            // Precargar campos
            textBoxDescripcion.Text = _proveedor.Descripcion;
            textBoxTelefono.Text = _proveedor.Telefono.ToString();

            buttonGuardar.Click += buttonGuardar_Click;
            buttonExitEP.Click += (_, __) => Close();

            // Mover form sin borde
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

            // Enter para guardar
            textBoxDescripcion.KeyDown += EnterSubmit;
            textBoxTelefono.KeyDown += EnterSubmit;

            // Solo números en teléfono
            textBoxTelefono.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            };
        }

        private System.Drawing.Point _mouseLocation;

        private void EnterSubmit(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                buttonGuardar.PerformClick();
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            var descripcion = textBoxDescripcion.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_requerida") ?? "La descripción es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxDescripcion.Focus();
                return;
            }

            if (!int.TryParse(textBoxTelefono.Text.Trim(), out int telefono))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTelefono.Focus();
                return;
            }

            try
            {
                _proveedor.Descripcion = descripcion;
                _proveedor.Telefono = telefono;

                _repo.Update(_proveedor);

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
