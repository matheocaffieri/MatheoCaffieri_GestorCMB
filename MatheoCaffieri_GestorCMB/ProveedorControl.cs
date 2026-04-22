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
    public partial class ProveedorControl : UserControl
    {
        public ProveedorControl() : this(new ProveedorBL())
        {
        }


        private const string REQUIRED = "AGREGAR_PROVEEDORES";


        private readonly MainForm _mainForm;


        // ctor para inyección de dependencias (tests, etc.)
        public ProveedorControl(IGenericRepository<Proveedor> proveedorRepo)
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




            _proveedorRepo = proveedorRepo ?? throw new ArgumentNullException(nameof(proveedorRepo));
            buttonBack.Text = "<<";

            this.Load += ProveedorControl_Load;
            buttonAddProveedor.Click += buttonAddProveedor_Click;
            buttonSearchClientes.Click += buttonSearchClientes_Click;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyDown += textBox1_KeyDown;

            proveedorLayoutPanel.Resize += (_, __) => AjustarAnchoItems();
        }

        private void AjustarAnchoItems()
        {
            int w = proveedorLayoutPanel.ClientSize.Width;
            if (w <= 0) return;
            int cols   = w >= 1400 ? 3 : w >= 700 ? 2 : 1;
            int itemW  = (w / cols) - 6;
            foreach (Control c in proveedorLayoutPanel.Controls)
                c.Width = itemW;
        }

        private readonly IGenericRepository<Proveedor> _proveedorRepo;


        private void CargarListado(string filtro = "")
        {
            List<Proveedor> proveedores = _proveedorRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim().ToLower();

                proveedores = proveedores
                    .Where(p =>
                        (!string.IsNullOrEmpty(p.Descripcion) && p.Descripcion.ToLower().Contains(filtro)) ||
                        p.Telefono.ToString().Contains(filtro)
                    )
                    .ToList();
            }

            proveedorLayoutPanel.SuspendLayout();
            proveedorLayoutPanel.Controls.Clear();

            foreach (var proveedor in proveedores)
            {
                var item = new ProveedorItemControl();
                item.Bind(proveedor);
                item.EditRequested += EditarProveedor;
                item.ActiveChanged += ToggleActivoProveedor;
                proveedorLayoutPanel.Controls.Add(item);
            }

            proveedorLayoutPanel.ResumeLayout();
            AjustarAnchoItems();
        }

        private void ProveedorControl_Load(object sender, EventArgs e)
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

        private void buttonSearchClientes_Click(object sender, EventArgs e)
        {
            CargarListado(textBox1.Text);
        }

        private void buttonAddProveedor_Click(object sender, EventArgs e)
        {
            string descripcion = textBoxDescripcion.Text.Trim();
            string telefonoStr = textBoxTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_requerida") ?? "La descripción es obligatoria.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long? telefono = null;
            if (!string.IsNullOrWhiteSpace(telefonoStr))
            {
                if (long.TryParse(telefonoStr, out long telParsed))
                    telefono = telParsed;
                else
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico.",
                        LanguageService.Current?.T("cap_validacion") ?? "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var nuevoProveedor = new Proveedor
            {
                Descripcion = descripcion,
                Telefono = (int)telefono
            };

            _proveedorRepo.Add(nuevoProveedor);

            textBoxDescripcion.Clear();
            textBoxTelefono.Clear();
            textBoxDescripcion.Focus();

            CargarListado(textBox1.Text);
        }

        private void ToggleActivoProveedor(Proveedor proveedor, bool nuevoEstado)
        {
            try
            {
                proveedor.IsActive = nuevoEstado;
                _proveedorRepo.Update(proveedor);
            }
            catch (Exception ex)
            {
                Services.Logs.LoggerLogic.Error($"[ProveedorControl] Error al actualizar estado proveedor {proveedor.IdProveedor}: {ex.Message}");
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarListado(textBox1.Text);
            }
        }

        private void EditarProveedor(DomainModel.Proveedor proveedor)
        {
            using (var frm = new EditProveedorForm(_proveedorRepo, proveedor))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    CargarListado(textBox1.Text);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                    LanguageService.Current?.T("cap_atencion") ?? "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new VerInventarioControl());
        }
    }
}
