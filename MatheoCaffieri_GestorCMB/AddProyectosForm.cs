using BL;
using DomainModel;
using DomainModel.Interfaces;
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

        private const string REQUIRED = "AGREGAR_PROYECTOS";


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
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }


            _proyectoRepo = proyectoRepo ?? throw new ArgumentNullException(nameof(proyectoRepo));
            _clienteRepo = clienteRepo ?? throw new ArgumentNullException(nameof(clienteRepo));

            // No está en el diseñador
            buttonAddProyecto.Click += buttonAddProyecto_Click;
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
                MessageBox.Show("Complete Descripción y Ubicación.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCliente.SelectedValue == null || !(comboBoxCliente.SelectedValue is Guid idCliente) || idCliente == Guid.Empty)
            {
                MessageBox.Show("Seleccione un cliente.", "Validación",
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
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo agregar el proyecto:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarClientes()
        {
            var clientes = _clienteRepo.GetAll().ToList();

            // Diagnóstico rápido
            // MessageBox.Show("Clientes encontrados: " + clientes.Count);

            // Armo un datasource simple (Id + Texto)
            var data = clientes
                .Select(c => new
                {
                    Id = c.IdCliente,
                    Texto =
                        // probá primero RazonSocial, si no existe o viene null usa otras
                        (c.RazonSocial ?? "").Trim().Length > 0 ? c.RazonSocial :
                        (c.NombreContacto ?? "").Trim().Length > 0 ? c.NombreContacto :
                        c.ToString()
                })
                .OrderBy(x => x.Texto)
                .ToList();

            comboBoxCliente.DataSource = null; // importante para refrescar
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
