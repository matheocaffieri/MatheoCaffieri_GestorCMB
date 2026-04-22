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
    public partial class ClientesControl : UserControl
    {

        private const string REQUIRED = "GESTIONAR_CLIENTES";


        // ctor por defecto -> usa BL real
        public ClientesControl(MainForm mainForm) : this(mainForm, new ClienteBL()) { }

        // ctor inyectable (para tests)
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
                //clienteItemControls[i].Dock = DockStyle.Top;
                //clienteItemControls[i].BringToFront();

                

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
                        // Telefono es int: convertir a string antes de buscar.
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
            using (var form = new EditClienteForm(_clienteRepo, cli))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    CargarListado(textBoxBuscar.Text);
            }
        }

        private void ToggleActivoCliente(Cliente cli, bool nuevoEstado)
        {
            try
            {
                cli.IsActive = nuevoEstado;
                _clienteRepo.Update(cli);
            }
            catch (Exception ex)
            {
                Services.Logs.LoggerLogic.Error($"[ClientesControl] Error al actualizar estado cliente {cli.IdCliente}: {ex.Message}");
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Revertir el switch visualmente
                CargarListado(textBoxBuscar.Text);
            }
        }

        private void buttonAddCliente_Click(object sender, EventArgs e)
        {
            // Ajustá estos nombres a los de tu diseñador
            string razonSocial = textBoxRazonSocial.Text.Trim();
            string telefono = textBoxTelefono.Text.Trim();
            string mail = textBoxMail.Text.Trim();
            string nombreContacto = textBoxNombreContacto.Text.Trim();

            if (string.IsNullOrWhiteSpace(razonSocial))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_razon_social_requerida") ?? "La razón social es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int telefonoNum;

            if (!int.TryParse(telefono, out telefonoNum))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevo = new Cliente
            {
                IdCliente = Guid.NewGuid(),
                RazonSocial = razonSocial,
                Telefono = telefonoNum,     // CORRECTO
                Mail = mail,
                NombreContacto = nombreContacto
            };


            _clienteRepo.Add(nuevo);

            // Limpiar formulario
            textBoxRazonSocial.Clear();
            textBoxTelefono.Clear();
            textBoxMail.Clear();
            textBoxNombreContacto.Clear();

            // Refrescar listado con el filtro actual
            CargarListado(textBoxBuscar.Text);
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
