using BL;
using DomainModel;
using DomainModel.Interfaces;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AddMaterialesForm : Form
    {
        private readonly IMaterialRepository _matBL;
        private readonly IGenericRepository<Proveedor> _provBL;

        public Point mouseLocation;

        // ctor por defecto: usa tus BL reales
        public AddMaterialesForm() : this(new MaterialBL(), new ProveedorBL())
        {
            // acá NO va InitializeComponent ni WireEvents
        }

        public AddMaterialesForm(IMaterialRepository materialBL, IGenericRepository<Proveedor> proveedorBL)
        {
            InitializeComponent();
            _matBL = materialBL ?? throw new ArgumentNullException(nameof(materialBL));
            _provBL = proveedorBL ?? throw new ArgumentNullException(nameof(proveedorBL));
            WireEvents();
        }


        private void WireEvents()
        {
            this.Load += AddMaterialesForm_Load;
            buttonAgregar.Click += buttonAgregar_Click;


            // mover form sin borde
            FormPanel.MouseDown += FormPanel_MouseDown;
            FormPanel.MouseMove += FormPanel_MouseMove;
            buttonExitAM.Click += buttonExitAM_Click;

            // enter para enviar
            textBoxDescripcion.KeyDown += EnterSubmitea;
            textBoxPrecio.KeyDown += EnterSubmitea;

            // filtro rápido de numérico (permite , . y backspace)
            textBoxPrecio.KeyPress += (s, e) =>
            {
                char c = e.KeyChar;
                if (char.IsControl(c) || char.IsDigit(c) || c == '.' || c == ',') return;
                e.Handled = true;
            };
        }

        // ==== Load ====
        private void AddMaterialesForm_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCombosFijos();
                CargarProveedores();
                textBoxDescripcion.Focus();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AddMaterialesForm] Error iniciando", ex);
                MessageBox.Show("No se pudo abrir el formulario de alta.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void CargarCombosFijos()
        {
            // Si luego los guardás en tablas, reemplazá por repos
            var tiposMaterial = new List<string> { "Cable", "Material", "Cañería", "Accesorio", "Electrónica", "Otro" };
            var tiposUnidad = new List<string> { "un", "cm", "m", "kg", "l" };

            comboBoxMaterial.DataSource = tiposMaterial;
            comboBoxUnidad.DataSource = tiposUnidad;
            comboBoxMaterial.SelectedIndex = 0;
            comboBoxUnidad.SelectedIndex = 0;
        }

        private void CargarProveedores()
        {
            var proveedores = _provBL.GetAll().OrderBy(p => p.Descripcion).ToList();

            comboBoxProveedor.DisplayMember = "Descripcion";
            comboBoxProveedor.ValueMember = "IdProveedor";
            comboBoxProveedor.DataSource = proveedores;

            // si la FK no acepta NULL, exigí selección
            comboBoxProveedor.SelectedIndex = proveedores.Count > 0 ? 0 : -1;
            comboBoxProveedor.Enabled = proveedores.Count > 0;
        }

        // ==== Guardar ====
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!TryBuild(out var mat)) return;

            try
            {
                _matBL.Add(mat);
                LoggerLogic.Info($"[AddMaterialesForm] Material agregado: {mat.DescripcionArticulo} ({mat.IdMaterial})");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AddMaterialesForm] Error al guardar material", ex);
                MessageBox.Show("No se pudo guardar el material.\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryBuild(out Material m)
        {
            m = null;

            var desc = textBoxDescripcion.Text.Trim();
            if (string.IsNullOrWhiteSpace(desc))
            {
                Warn("La descripción es obligatoria.", textBoxDescripcion);
                return false;
            }

            var tipoMat = comboBoxMaterial.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(tipoMat))
            {
                Warn("Seleccioná el tipo de material.", comboBoxMaterial);
                return false;
            }

            var tipoUni = comboBoxUnidad.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(tipoUni))
            {
                Warn("Seleccioná el tipo de unidad.", comboBoxUnidad);
                return false;
            }

            if (!TryParseDecimal(textBoxPrecio.Text, out var precio) || precio < 0)
            {
                Warn("Ingresá un precio válido (número positivo).", textBoxPrecio);
                return false;
            }

            // Proveedor (si tu FK no acepta NULL, esto debe ser obligatorio)
            Guid idProv = Guid.Empty;
            if (comboBoxProveedor.Enabled && comboBoxProveedor.SelectedValue is Guid g) idProv = g;

            m = new Material
            {
                IdMaterial = Guid.Empty,      // lo asigna el BL si hace falta
                DescripcionArticulo = desc,
                TipoMaterial = tipoMat,
                TipoUnidad = tipoUni,
                CostoPorUnidad = (float)precio,          // usá decimal en dominio
                IdProveedor = idProv
            };

            return true;
        }

        private static bool TryParseDecimal(string input, out decimal value)
        {
            var s = (input ?? "").Trim();
            // cultura actual
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
                return true;
            // invariante
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return true;
            // reemplazo rápido
            s = s.Replace(',', '.');
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        private static void Warn(string msg, Control focus)
        {
            MessageBox.Show(msg, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            focus.Focus();
        }

        // ==== mover form sin borde + cerrar ====
        private void FormPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);

        }

        private void FormPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }

        private void buttonExitAM_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnterSubmitea(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // evita beep
                buttonAgregar.PerformClick();
            }
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            if (!TryBuild(out var mat))
                return;

            try
            {
                _matBL.Add(mat);

                LoggerLogic.Info(
                    $"[AddMaterialesForm] Material agregado: {mat.DescripcionArticulo} ({mat.IdMaterial})");

                MessageBox.Show("Material agregado correctamente.",
                                "Información",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AddMaterialesForm] Error al guardar material", ex);

                MessageBox.Show("No se pudo guardar el material.\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
