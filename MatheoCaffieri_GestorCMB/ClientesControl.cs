using MatheoCaffieri_GestorCMB.ItemControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using DomainModel;
using DomainModel.Interfaces;

namespace MatheoCaffieri_GestorCMB
{
    public partial class ClientesControl : UserControl
    {
        // ctor por defecto -> usa BL real
        public ClientesControl() : this(new ClienteBL())
        {
            // acá no va InitializeComponent
        }

        // ctor inyectable (para tests)
        public ClientesControl(IGenericRepository<Cliente> clienteRepo)
        {
            InitializeComponent();
            _clienteRepo = clienteRepo ?? throw new ArgumentNullException(nameof(clienteRepo));

            WireEvents();
        }

        private readonly IGenericRepository<Cliente> _clienteRepo;

        private void WireEvents()
        {
            // NO enganchar Load ni TextChanged acá: ya están en el Designer

            if (buttonSearchClientes != null)
                buttonSearchClientes.Click += buttonSearchClientes_Click;

            if (textBoxBuscar != null)
                textBoxBuscar.KeyDown += textBoxBuscar_KeyDown;

            if (buttonAddCliente != null)
                buttonAddCliente.Click += buttonAddCliente_Click;
        }



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
            var item = new ClientesItemControl
            {
                IdCliente = cli.IdCliente,
                RazonSocial = cli.RazonSocial,
                Telefono = cli.Telefono.ToString(),
                Mail = cli.Mail,
                NombreContacto = cli.NombreContacto,
                Dock = DockStyle.Top
            };

            item.ModificarClick += (s, e) => EditarCliente(cli);
            item.HabilitarClick += (s, e) => HabilitarCliente(cli);

            return item;
        }

        private void EditarCliente(Cliente cli)
        {
            // Luego lo hacemos bien con un form de edición
            MessageBox.Show($"Editar cliente: {cli.RazonSocial}");
        }

        private void HabilitarCliente(Cliente cli)
        {
            // Cuando tengas campo Activo/Habilitado, lo actualizamos acá
            MessageBox.Show($"Habilitar / deshabilitar cliente: {cli.RazonSocial}");
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
                MessageBox.Show("La razón social es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int telefonoNum;

            if (!int.TryParse(telefono, out telefonoNum))
            {
                MessageBox.Show("El teléfono debe ser numérico.", "Validación",
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

        }
    }
}
