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

            this.Load += ProveedorControl_Load;
            buttonAddProveedor.Click += buttonAddProveedor_Click;
        }

        private readonly IGenericRepository<Proveedor> _proveedorRepo;


        private void ObtenerProveedoresItems()
        {
            // Traemos los proveedores desde la BL
            List<Proveedor> proveedores = _proveedorRepo.GetAll();

            proveedorLayoutPanel.SuspendLayout();
            proveedorLayoutPanel.Controls.Clear();

            proveedores.ForEach(proveedor =>
            {
                var proveedorItemControl = new ProveedorItemControl
                {
                    Descripcion = proveedor.Descripcion,
                    Telefono = proveedor.Telefono.ToString()
                    // proveedorItemControl.IsActive = proveedor.IsActive;  // si tenés este campo
                };

                proveedorLayoutPanel.Controls.Add(proveedorItemControl);
            });

            proveedorLayoutPanel.ResumeLayout();
        }

        private void ProveedorControl_Load(object sender, EventArgs e)
        {
            ObtenerProveedoresItems();
        }

        private void buttonAddProveedor_Click(object sender, EventArgs e)
        {
            // Usá los nombres reales de tus TextBox
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
                // IsActive = true;  // si tu entidad tiene este campo
            };

            _proveedorRepo.Add(nuevoProveedor);

            // Refrescamos el listado
            ObtenerProveedoresItems();

            // Limpiamos el formulario
            textBoxDescripcion.Clear();
            textBoxTelefono.Clear();
            textBoxDescripcion.Focus();
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
