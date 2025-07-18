using BL;
using DomainModel;
using DomainModel.Interfaces;
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

namespace MatheoCaffieri_GestorCMB
{
    public partial class ProveedorControl : UserControl
    {
        public ProveedorControl()
        {
            InitializeComponent();
        }

        

void ObtenerProveedoresItems()
        {
            // Obtenemos los datos de la base de datos
            List<Proveedor> proveedores = ((IGenericRepository<Proveedor>)new ProveedorBLL()).GetAll();

            // Asegúrate de limpiar el LayoutPanel antes de agregar nuevos controles
            proveedorLayoutPanel.Controls.Clear();

            // Usamos LINQ para crear y agregar los controles
            proveedores.ForEach(proveedor =>
            {
                // Creas un nuevo UserControl para cada proveedor
                ProveedorItemControl proveedorItemControl = new ProveedorItemControl
                {
                    // Asignas los datos del proveedor al control
                    Descripcion = proveedor.Descripcion,
                    Telefono = proveedor.Telefono.ToString()
                    //proveedorItemControl.IsActive = proveedor.IsActive;
                };

                // Agregas el control al LayoutPanel
                proveedorLayoutPanel.Controls.Add(proveedorItemControl);
            });
        }

        private void ProveedorControl_Load(object sender, EventArgs e)
        {
            ObtenerProveedoresItems();
        }
    }
}
