using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class ClientesItemControl : UserControl
    {
        public event EventHandler ModificarClick;
        public event EventHandler HabilitarClick;

        public Guid IdCliente { get; set; }

        public string RazonSocial
        {
            get => labelRazonSocial.Text;
            set => labelRazonSocial.Text = value;
        }

        public string Telefono
        {
            get => labelTelefonoCliente.Text;
            set => labelTelefonoCliente.Text = value;
        }

        public string Mail
        {
            get => labelMailCliente.Text;
            set => labelMailCliente.Text = value;
        }

        public string NombreContacto
        {
            get => labelNombreContacto.Text;
            set => labelNombreContacto.Text = value;
        }

        public ClientesItemControl()
        {
            InitializeComponent();
            buttonModificarCliente.Click += (s, e) => ModificarClick?.Invoke(this, EventArgs.Empty);
            buttonHabDesCliente.Click += (s, e) => HabilitarClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
