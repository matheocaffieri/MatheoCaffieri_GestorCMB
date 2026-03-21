using System;
using System.Windows.Forms;
using DomainModel;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class ClientesItemControl : UserControl
    {
        public Cliente Cliente { get; private set; }

        public event Action<Cliente, bool> ActiveChanged;
        public event Action<Cliente> EditRequested;

        public ClientesItemControl()
        {
            InitializeComponent();

            SwitchHabilitarCliente.ToggleChanged += (_, __) =>
            {
                if (Cliente == null) return;
                bool nuevoEstado = SwitchHabilitarCliente.IsOn;
                Cliente.IsActive = nuevoEstado;
                ActiveChanged?.Invoke(Cliente, nuevoEstado);
            };

            buttonModificarCliente.Click += (_, __) =>
            {
                if (Cliente == null) return;
                EditRequested?.Invoke(Cliente);
            };
        }

        public void Bind(Cliente c)
        {
            Cliente = c;
            labelRazonSocial.Text = c.RazonSocial;
            labelMailCliente.Text = c.Mail;
            labelTelefonoCliente.Text = c.Telefono.ToString();
            labelNombreContacto.Text = c.NombreContacto;
            SwitchHabilitarCliente.IsOn = c.IsActive;
        }
    }
}
