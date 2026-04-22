using DomainModel;
using System;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class ProveedorItemControl : UserControl
    {
        public Proveedor Proveedor { get; private set; }

        public event Action<Proveedor> EditRequested;
        public event Action<Proveedor, bool> ActiveChanged;

        public ProveedorItemControl()
        {
            InitializeComponent();

            SwitchHabilitarProveedor.ToggleChanged += (_, __) =>
            {
                if (Proveedor == null) return;
                bool nuevoEstado = SwitchHabilitarProveedor.IsOn;
                Proveedor.IsActive = nuevoEstado;
                ActiveChanged?.Invoke(Proveedor, nuevoEstado);
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width < 200) return;

            SwitchHabilitarProveedor.Left  = this.Width - SwitchHabilitarProveedor.Width - 8;
            buttonEditarProveedor.Left     = SwitchHabilitarProveedor.Left - buttonEditarProveedor.Width - 4;
            labelDescripcionProveedor.Width = labelTelefonoProveedor.Left - labelDescripcionProveedor.Left - 8;
        }

        public void Bind(Proveedor p)
        {
            Proveedor = p;
            labelDescripcionProveedor.Text = p.Descripcion;
            labelTelefonoProveedor.Text = p.Telefono.ToString();
            SwitchHabilitarProveedor.IsOn = p.IsActive;

            buttonEditarProveedor.Click -= buttonEditarProveedor_Click;
            buttonEditarProveedor.Click += buttonEditarProveedor_Click;
        }

        private void buttonEditarProveedor_Click(object sender, EventArgs e)
        {
            EditRequested?.Invoke(Proveedor);
        }
    }
}
