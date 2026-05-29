using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using Services.Language;
using Services.Logs;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AddProyectosForm : Form
    {
        public Point mouseLocation;
        private readonly IGenericRepository<Proyecto> _proyectoRepo;
        private readonly IGenericRepository<Cliente> _clienteRepo;

        private const string REQUIRED = "GESTIONAR_PROYECTOS";


        public bool ProyectoGuardado { get; private set; } = false;

        public event EventHandler ProyectoCreado;

        public AddProyectosForm() : this(new ProyectoBL(), new ClienteBL())
        {
            // InitializeComponent lo llama el ctor principal
        }

        public AddProyectosForm(IGenericRepository<Proyecto> proyectoRepo, IGenericRepository<Cliente> clienteRepo)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }


            _proyectoRepo = proyectoRepo ?? throw new ArgumentNullException(nameof(proyectoRepo));
            _clienteRepo = clienteRepo ?? throw new ArgumentNullException(nameof(clienteRepo));

            Load += AddProyectosForm_Load;
        }

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

        private void buttonExitAP_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAddProyecto_Click(object sender, EventArgs e)
        {
            string descripcion = (textBoxDescProyecto.Text ?? "").Trim();
            string ubicacion = (textBoxUbicacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(ubicacion))
            {
                LoggerLogic.Warn("[AddProyectosForm] Validación: descripción o ubicación vacías al crear proyecto.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_descripcion_ubicacion") ?? "Complete Descripción y Ubicación.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCliente.SelectedValue == null || !(comboBoxCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
            {
                LoggerLogic.Warn("[AddProyectosForm] Validación: cliente no seleccionado al crear proyecto.");
                MessageBox.Show(
                    LanguageService.Current?.T("val_cliente_requerido") ?? "Seleccione un cliente.",
                    LanguageService.Current?.T("cap_validacion") ?? "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fechaInicio = dateTimePickerProyecto.Value.Date;

            // En tu DB fechaFin NO permite null, por eso lo inicializamos así.
            var proyecto = new Proyecto
            {
                IdProyecto = Guid.NewGuid(),
                IdCliente = idCliente,
                Descripcion = descripcion,
                Ubicacion = ubicacion,
                FechaInicio = fechaInicio,
                FechaFin = fechaInicio,
                Estado = EnumEstado.EnProceso
            };

            try
            {
                _proyectoRepo.Add(proyecto);
                ProyectoCreado?.Invoke(this, EventArgs.Empty);

                ProyectoGuardado = true;
                Close();
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[AddProyectosForm] Validación de negocio: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AddProyectosForm] Falla al guardar proyecto.", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarClientes()
        {
            var clientes = _clienteRepo.GetAll().ToList();

            // Texto a mostrar en el combo: primero RazonSocial; si está vacía, NombreContacto; si tampoco, ToString().
            var data = clientes
                .Select(c => new
                {
                    Id = c.IdCliente,
                    Texto =
                        (c.RazonSocial ?? "").Trim().Length > 0 ? c.RazonSocial :
                        (c.NombreContacto ?? "").Trim().Length > 0 ? c.NombreContacto :
                        c.ToString()
                })
                .OrderBy(x => x.Texto)
                .ToList();

            // Reasignamos DataSource = null antes de re-bindear para que el combo dispare el refresh.
            comboBoxCliente.DataSource = null;
            comboBoxCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCliente.DisplayMember = "Texto";
            comboBoxCliente.ValueMember = "Id";
            comboBoxCliente.DataSource = data;

            if (data.Count == 0)
                comboBoxCliente.SelectedIndex = -1;
        }



        private void AddProyectosForm_Load(object sender, EventArgs e)
        {
            CargarClientes();

        }
    }
}
