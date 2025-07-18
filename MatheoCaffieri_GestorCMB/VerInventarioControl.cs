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
    public partial class VerInventarioControl : UserControl
    {
        public VerInventarioControl()
        {
            InitializeComponent();
        }

        private void ObtenerMaterialesItems()
        {
            List<Inventario> materiales = ((IGenericRepository<Inventario>)new InventarioBL()).GetAll();

            MaterialesItemPanel.Controls.Clear();

            materiales.ForEach(e =>
            {
                InventarioItemControl inventarioItemControl = new InventarioItemControl
                {
                    DescripcionArticuloInventario = e.Material.DescripcionArticulo,
                    TipoArticuloInventario = e.Material.TipoMaterial,
                    InfoGeneralArticuloInventario = $"| {e.Material.Proveedor.Descripcion} | Unidad: {e.Material.TipoUnidad} | {e.Material.CostoPorUnidad}",
                    CantidadArticuloInventario = e.Cantidad.ToString()
                };
                

                // Agregas el control al LayoutPanel
                MaterialesItemPanel.Controls.Add(inventarioItemControl);
            });
        }
        private void VerInventarioControl_Load(object sender, EventArgs e)
        {
            ObtenerMaterialesItems();
        }
    }
}
