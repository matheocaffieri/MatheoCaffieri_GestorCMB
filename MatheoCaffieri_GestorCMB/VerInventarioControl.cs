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
    public partial class VerInventarioControl : UserControl
    {

        private readonly IGenericRepository<Inventario> _invRepo;
        private readonly MainForm _mainForm;

        private const string REQUIRED = "VER_INVENTARIO";


        public VerInventarioControl()
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = _mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                        LanguageService.Current?.T("cap_atencion") ?? "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                host.addUserControl(new HomeControl(_mainForm));
                return;
            }


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

                // *** ESTA ES LA LÍNEA QUE FALTABA ***
                inventarioItemControl.SetInventarioId(e.IdMaterialInventario);

            
                // Agregas el control al LayoutPanel
                MaterialesItemPanel.Controls.Add(inventarioItemControl);
            });
        }
        private void VerInventarioControl_Load(object sender, EventArgs e)
        {
            ObtenerMaterialesItems();
        }

        private void buttonAgregarMaterial_Click(object sender, EventArgs e)
        {
            using (var form = new AddMaterialesForm())
            {
                form.StartPosition = FormStartPosition.CenterParent; // centrado
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ObtenerMaterialesItems(); // refresca lista
                }
            }
        }

        private void buttonGestionarProveedores_Click(object sender, EventArgs e)
        {
            // 1) Permisos: si no puede, mostrar y cortar
            if (!SessionContext.Has("AGREGAR_PROVEEDORES")) // ajustá la key
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2) Recién si tiene permiso, buscamos el MainForm
            var host = _mainForm ?? (this.FindForm() as MainForm);

            if (host == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                    LanguageService.Current?.T("cap_atencion") ?? "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3) Navegación
            host.addUserControl(new ProveedorControl(/* pasá deps si corresponde */));
        }

        private void buttonVerInformesCompra_Click(object sender, EventArgs e)
        {

        }
    }
}
