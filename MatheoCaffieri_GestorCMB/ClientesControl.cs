using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using DomainModel.Login;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.Logs;
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
    public partial class ClientesControl : UserControl
    {

        private const string REQUIRED = "VER_CLIENTES";


        public ClientesControl(MainForm mainForm) : this(mainForm, new ClienteBL()) { }

        // DI / tests
        public ClientesControl(MainForm mainForm, IGenericRepository<Cliente> clienteRepo)
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

            _mainForm = mainForm;
            _clienteRepo = clienteRepo ?? throw new ArgumentNullException(nameof(clienteRepo));
        }

        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly MainForm _mainForm;



        private void ObtenerClientesItems()
        {
            ClientesItemControl[] clienteItemControls = new ClientesItemControl[5];
            for (int i = 0; i < clienteItemControls.Length; i++)
            {
                clienteItemControls[i] = new ClientesItemControl();
                gestionarClientesLayoutPanel.Controls.Add(clienteItemControls[i]);
            }
        }

        private void ClientesControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                CargarListado();

        }



        private void CargarListado(string filtro = "")
        {
            List<Cliente> clientes = _clienteRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();

                clientes = clientes
                    .Where(c =>
                        (!string.IsNullOrEmpty(c.RazonSocial) && c.RazonSocial.ToLower().Contains(filtro)) ||
                        (c != null && c.Telefono.ToString().Contains(filtro)) ||
                        (!string.IsNullOrEmpty(c.Mail) && c.Mail.ToLower().Contains(filtro)) ||
                        (!string.IsNullOrEmpty(c.NombreContacto) && c.NombreContacto.ToLower().Contains(filtro))
                    )
                    .ToList();
            }

            gestionarClientesLayoutPanel.SuspendLayout();
            gestionarClientesLayoutPanel.Controls.Clear();

            foreach (var cli in clientes)
            {
                var item = CrearItemCliente(cli);
                gestionarClientesLayoutPanel.Controls.Add(item);
            }

            gestionarClientesLayoutPanel.ResumeLayout();
        }



        private ClientesItemControl CrearItemCliente(Cliente cli)
        {
            var item = new ClientesItemControl { Dock = DockStyle.Top };
            item.Bind(cli);

            item.EditRequested += EditarCliente;
            item.ActiveChanged += ToggleActivoCliente;

            return item;
        }

        private void EditarCliente(Cliente cli)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_CLIENTES))
                return;

            using (var form = new EditClienteForm(_clienteRepo, cli))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CargarListado(textBoxBuscar.Text);
            }
        }

        private void ToggleActivoCliente(Cliente cli, bool nuevoEstado)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_CLIENTES))
            {
                CargarListado(textBoxBuscar.Text); // revertir el switch al estado real
                return;
            }

            try
            {
                cli.IsActive = nuevoEstado;
                _clienteRepo.Update(cli);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[ClientesControl] Validación al cambiar estado de cliente: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarListado(textBoxBuscar.Text);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ClientesControl] Falla al cambiar estado de cliente. Id={cli.IdCliente}", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Recargar para revertir el switch visualmente al estado real de la BD.
                CargarListado(textBoxBuscar.Text);
            }
        }

        private void buttonAddCliente_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(TipoPermiso.GESTIONAR_CLIENTES))
                return;

            string razonSocial = textBoxRazonSocial.Text.Trim();
            string telefono = textBoxTelefono.Text.Trim();
            string mail = textBoxMail.Text.Trim();
            string nombreContacto = textBoxNombreContacto.Text.Trim();

            if (string.IsNullOrWhiteSpace(razonSocial))
            {
                LoggerLogic.Warn("[ClientesControl] Validación: razón social vacía al crear cliente.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_razon_social_requerida") ?? "La razón social es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(telefono))
            {
                LoggerLogic.Warn("[ClientesControl] Validación: teléfono vacío al crear cliente.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_requerido") ?? "El teléfono es obligatorio.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int telefonoNum;

            if (!int.TryParse(telefono, out telefonoNum) || telefonoNum <= 0)
            {
                LoggerLogic.Warn($"[ClientesControl] Validación: teléfono inválido al crear cliente ('{telefono}').");
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(mail))
            {
                LoggerLogic.Warn("[ClientesControl] Validación: mail vacío al crear cliente.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_mail_requerido") ?? "El mail es obligatorio.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Validaciones.EsMailValido(mail))
            {
                LoggerLogic.Warn($"[ClientesControl] Validación: mail con formato inválido al crear cliente ('{mail}').");
                MessageBox.Show(
                    LanguageService.Current?.T("val_mail_invalido") ?? "El mail no tiene un formato válido.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(nombreContacto))
            {
                LoggerLogic.Warn("[ClientesControl] Validación: nombre de contacto vacío al crear cliente.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_nombre_contacto_requerido") ?? "El nombre de contacto es obligatorio.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevo = new Cliente
            {
                IdCliente = Guid.NewGuid(),
                RazonSocial = razonSocial,
                Telefono = telefonoNum,
                Mail = mail,
                NombreContacto = nombreContacto,
                IsActive = true
            };

            try
            {
                _clienteRepo.Add(nuevo);

                textBoxRazonSocial.Clear();
                textBoxTelefono.Clear();
                textBoxMail.Clear();
                textBoxNombreContacto.Clear();

                CargarListado(textBoxBuscar.Text);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[ClientesControl] Validación al crear cliente: {ex.MessageKey}");
                MessageBox.Show(
                    LanguageService.Current?.T(ex.MessageKey) ?? ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[ClientesControl] Falla al crear cliente.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxBuscar_TextChanged(object sender, EventArgs e)
        {
            CargarListado(textBoxBuscar.Text);

        }

        private void buttonSearchClientes_Click(object sender, EventArgs e)
        {
            CargarListado(textBoxBuscar.Text);

        }

        private void textBoxBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CargarListado(textBoxBuscar.Text);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            _mainForm?.addUserControl(new HomeControl(_mainForm));
        }
    }
}
