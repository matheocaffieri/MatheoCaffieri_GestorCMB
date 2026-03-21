using System;
using System.Windows.Forms;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;

namespace MatheoCaffieri_GestorCMB
{
    public partial class EditClienteForm : Form
    {
        private readonly IGenericRepository<Cliente> _repo;
        private readonly Cliente _cliente;

        public EditClienteForm(IGenericRepository<Cliente> repo, Cliente cliente)
        {
            InitializeComponent();

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));

            if (_cliente.IdCliente == Guid.Empty)
                throw new ArgumentException("IdCliente requerido.", nameof(cliente));

            // Precargar campos
            textBoxRazonSocial.Text = _cliente.RazonSocial;
            textBoxTelefono.Text = _cliente.Telefono.ToString();
            textBoxMail.Text = _cliente.Mail;
            textBoxNombreContacto.Text = _cliente.NombreContacto;

            buttonGuardar.Click += buttonGuardar_Click;
            buttonExitEC.Click += (_, __) => Close();

            // mover form sin borde
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
            textBoxRazonSocial.KeyDown += EnterSubmit;
            textBoxTelefono.KeyDown += EnterSubmit;
            textBoxMail.KeyDown += EnterSubmit;
            textBoxNombreContacto.KeyDown += EnterSubmit;

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
            var razonSocial = textBoxRazonSocial.Text.Trim();
            var mail = textBoxMail.Text.Trim();
            var nombreContacto = textBoxNombreContacto.Text.Trim();

            if (string.IsNullOrWhiteSpace(razonSocial))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_razon_social_requerida") ?? "La razón social es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxRazonSocial.Focus();
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
                _cliente.RazonSocial = razonSocial;
                _cliente.Telefono = telefono;
                _cliente.Mail = mail;
                _cliente.NombreContacto = nombreContacto;

                _repo.Update(_cliente);

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
