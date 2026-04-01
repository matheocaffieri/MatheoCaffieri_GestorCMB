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

        private readonly IGenericRepository<Inventario> _invRepo = new InventarioBL();
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

            WireEvents();
        }

        private void WireEvents()
        {
            if (buttonSearchMaterial != null)
                buttonSearchMaterial.Click += buttonSearchMaterial_Click;

            if (textBox1 != null)
            {
                textBox1.TextChanged += textBox1_TextChanged;
                textBox1.KeyDown += textBox1_KeyDown;
            }
        }

        private void CargarListado(string filtro = "")
        {
            List<Inventario> materiales = _invRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();

                materiales = materiales
                    .Where(e =>
                        (e.Material != null && !string.IsNullOrEmpty(e.Material.DescripcionArticulo) && e.Material.DescripcionArticulo.ToLower().Contains(filtro)) ||
                        (e.Material != null && !string.IsNullOrEmpty(e.Material.TipoMaterial) && e.Material.TipoMaterial.ToLower().Contains(filtro)) ||
                        (e.Material != null && !string.IsNullOrEmpty(e.Material.TipoUnidad) && e.Material.TipoUnidad.ToLower().Contains(filtro)) ||
                        (e.Material?.Proveedor != null && !string.IsNullOrEmpty(e.Material.Proveedor.Descripcion) && e.Material.Proveedor.Descripcion.ToLower().Contains(filtro)) ||
                        e.Cantidad.ToString().Contains(filtro)
                    )
                    .ToList();
            }

            MaterialesItemPanel.SuspendLayout();
            MaterialesItemPanel.Controls.Clear();

            foreach (var mat in materiales)
            {
                var item = CrearItemInventario(mat);
                MaterialesItemPanel.Controls.Add(item);
            }

            MaterialesItemPanel.ResumeLayout();
        }

        private InventarioItemControl CrearItemInventario(Inventario e)
        {
            var item = new InventarioItemControl
            {
                DescripcionArticuloInventario = e.Material.DescripcionArticulo,
                TipoArticuloInventario = e.Material.TipoMaterial,
                InfoGeneralArticuloInventario = $"| {e.Material.Proveedor.Descripcion} | Unidad: {e.Material.TipoUnidad} | {e.Material.CostoPorUnidad}",
                CantidadArticuloInventario = e.Cantidad.ToString()
            };
            item.SetInventarioId(e.IdMaterialInventario);
            return item;
        }

        private void VerInventarioControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                CargarListado();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CargarListado(textBox1.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CargarListado(textBox1.Text);
            }
        }

        private void buttonSearchMaterial_Click(object sender, EventArgs e)
        {
            CargarListado(textBox1.Text);
        }

        private void buttonAgregarMaterial_Click(object sender, EventArgs e)
        {
            using (var form = new AddMaterialesForm())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog(this) == DialogResult.OK)
                    CargarListado(textBox1.Text);
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
            if (!SessionContext.Has("CONSULTAR_INFORMES_COMPRA"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null) return;

            host.addUserControl(new InformesDeCompraControl());
        }
    }
}
