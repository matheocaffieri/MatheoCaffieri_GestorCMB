using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class EditProyectoForm : Form
    {
        private readonly IGenericRepository<Proyecto> _proyectoRepo;
        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly Proyecto _proyecto;

        private Point _mouseLocation;

        public EditProyectoForm(Proyecto proyecto)
            : this(proyecto, new ProyectoBL(), new ClienteBL()) { }

        public EditProyectoForm(Proyecto proyecto, IGenericRepository<Proyecto> proyectoRepo, IGenericRepository<Cliente> clienteRepo)
        {
            InitializeComponent();

            _proyecto = proyecto ?? throw new ArgumentNullException(nameof(proyecto));
            _proyectoRepo = proyectoRepo ?? throw new ArgumentNullException(nameof(proyectoRepo));
            _clienteRepo = clienteRepo ?? throw new ArgumentNullException(nameof(clienteRepo));

            Load += EditProyectoForm_Load;
        }

        private void EditProyectoForm_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarEstados();

            // Precargar campos con datos actuales
            textBoxDescripcion.Text = _proyecto.Descripcion;
            textBoxUbicacion.Text = _proyecto.Ubicacion;
            dateTimePickerFechaInicio.Value = _proyecto.FechaInicio;

            // Seleccionar cliente actual
            foreach (var item in comboBoxCliente.Items)
            {
                var prop = item.GetType().GetProperty("Id");
                if (prop != null && prop.GetValue(item) is Guid id && id == _proyecto.IdCliente)
                {
                    comboBoxCliente.SelectedItem = item;
                    break;
                }
            }

            // Seleccionar estado actual
            comboBoxEstado.SelectedItem = _proyecto.Estado;
        }

        private void CargarClientes()
        {
            var clientes = _clienteRepo.GetAll()
                .Select(c => new
                {
                    Id = c.IdCliente,
                    Texto = (!string.IsNullOrWhiteSpace(c.RazonSocial) ? c.RazonSocial : c.NombreContacto) ?? c.ToString()
                })
                .OrderBy(x => x.Texto)
                .ToList();

            comboBoxCliente.DataSource = null;
            comboBoxCliente.DisplayMember = "Texto";
            comboBoxCliente.ValueMember = "Id";
            comboBoxCliente.DataSource = clientes;
        }

        private void CargarEstados()
        {
            comboBoxEstado.DataSource = Enum.GetValues(typeof(EnumEstado));
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            var descripcion = textBoxDescripcion.Text.Trim();
            var ubicacion = textBoxUbicacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(ubicacion))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_ubicacion") ?? "Complete Descripción y Ubicación.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(comboBoxCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_cliente_requerido") ?? "Seleccione un cliente.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _proyecto.Descripcion = descripcion;
                _proyecto.Ubicacion = ubicacion;
                _proyecto.IdCliente = idCliente;
                _proyecto.FechaInicio = dateTimePickerFechaInicio.Value.Date;
                _proyecto.Estado = (EnumEstado)comboBoxEstado.SelectedItem;

                _proyectoRepo.Update(_proyecto);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExitEP_Click(object sender, EventArgs e) => Close();

        private void FormPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseLocation = new Point(-e.X, -e.Y);
        }

        private void FormPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pos = MousePosition;
                pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                Location = pos;
            }
        }
    }
}
