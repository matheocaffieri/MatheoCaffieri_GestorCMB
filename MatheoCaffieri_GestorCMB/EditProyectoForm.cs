using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;
using Services.Logs;
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

            AplicarTraducciones();
            Load += EditProyectoForm_Load;
        }

        private void AplicarTraducciones()
        {
            this.Text             = LanguageService.Current?.T("cap_editar_proyecto")        ?? "Editar proyecto";
            labelTitulo.Text      = LanguageService.Current?.T("cap_editar_proyecto")        ?? "Editar proyecto";
            labelDescripcion.Text = LanguageService.Current?.T("lbl_descripcion_proyecto")   ?? "Descripción del proyecto";
            labelCliente.Text     = LanguageService.Current?.T("lbl_cliente")                ?? "Cliente";
            labelUbicacion.Text   = LanguageService.Current?.T("lbl_ubicacion")              ?? "Ubicación";
            labelFechaInicio.Text = LanguageService.Current?.T("lbl_fecha_inicio")           ?? "Fecha de inicio";
            labelEstado.Text      = LanguageService.Current?.T("lbl_estado")                 ?? "Estado";
            labelFechaCierre.Text = LanguageService.Current?.T("lbl_fecha_cierre")           ?? "Fecha de cierre";
            buttonGuardar.Text    = LanguageService.Current?.T("btn_guardar")                ?? "Guardar";
        }

        private static string FormatEstado(EnumEstado estado)
        {
            switch (estado)
            {
                case EnumEstado.EnProceso:  return LanguageService.Current?.T("val_estado_en_proceso") ?? "En proceso";
                case EnumEstado.Suspendido: return LanguageService.Current?.T("val_estado_suspendido") ?? "Suspendido";
                case EnumEstado.Finalizado: return LanguageService.Current?.T("val_estado_finalizado") ?? "Finalizado";
                default:                    return estado.ToString();
            }
        }

        private void EditProyectoForm_Load(object sender, EventArgs e)
        {
            CargarClientes();
            CargarEstados();

            // Precargar campos con datos actuales
            textBoxDescripcion.Text = _proyecto.Descripcion;
            textBoxUbicacion.Text = _proyecto.Ubicacion;
            dateTimePickerFechaInicio.Value = _proyecto.FechaInicio;
            dateTimePickerFechaCierre.Value = _proyecto.FechaFin < _proyecto.FechaInicio
                ? _proyecto.FechaInicio
                : _proyecto.FechaFin;

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
            comboBoxEstado.SelectedValue = _proyecto.Estado;
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
            var items = Enum.GetValues(typeof(EnumEstado))
                .Cast<EnumEstado>()
                .Select(e => new { Valor = e, Texto = FormatEstado(e) })
                .ToList();
            comboBoxEstado.DataSource    = items;
            comboBoxEstado.DisplayMember = "Texto";
            comboBoxEstado.ValueMember   = "Valor";
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            var descripcion = textBoxDescripcion.Text.Trim();
            var ubicacion = textBoxUbicacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(ubicacion))
            {
                LoggerLogic.Warn($"[EditProyectoForm] Validación: descripción o ubicación vacías (Id={_proyecto.IdProyecto}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_ubicacion") ?? "Complete Descripción y Ubicación.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(comboBoxCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
            {
                LoggerLogic.Warn($"[EditProyectoForm] Validación: cliente no seleccionado (Id={_proyecto.IdProyecto}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_cliente_requerido") ?? "Seleccione un cliente.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fechaInicio = dateTimePickerFechaInicio.Value.Date;
            var fechaCierre = dateTimePickerFechaCierre.Value.Date;

            if (fechaCierre < fechaInicio)
            {
                LoggerLogic.Warn($"[EditProyectoForm] Validación: fecha de cierre anterior a fecha de inicio (Id={_proyecto.IdProyecto}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_fecha_cierre_anterior") ?? "La fecha de cierre no puede ser anterior a la fecha de inicio.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _proyecto.Descripcion = descripcion;
                _proyecto.Ubicacion = ubicacion;
                _proyecto.IdCliente = idCliente;
                _proyecto.FechaInicio = fechaInicio;
                _proyecto.FechaFin = fechaCierre;
                _proyecto.Estado = (EnumEstado)comboBoxEstado.SelectedValue;

                _proyectoRepo.Update(_proyecto);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[EditProyectoForm] Validación al actualizar proyecto: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EditProyectoForm] Falla al actualizar proyecto. Id={_proyecto.IdProyecto}", ex);
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
