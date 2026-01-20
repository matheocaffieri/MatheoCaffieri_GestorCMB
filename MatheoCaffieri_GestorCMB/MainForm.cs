using BL.AccessBL;
using BL.LoginBL;
using Services.RoleService;
using Services.RoleService.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;


namespace MatheoCaffieri_GestorCMB
{

    public partial class MainForm : Form
    {
        public Point mouseLocation;
        private readonly RolesServiceLogic _rolesService;
        private readonly UsuarioService _usuarioService;

        public MainForm()
        {
            InitializeComponent();
            var homeControl = new HomeControl(this);   // HomeControl NO lleva servicios
            addUserControl(homeControl);
        }

        // Runtime: pasás servicios acá (HomeControl sigue sin params)
        public MainForm(RolesService rolesService, UsuarioService usuarioService)
        {
            InitializeComponent();
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));

            var homeControl = new HomeControl(this);
            addUserControl(homeControl);
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {

        }

        
        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
        }

        
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void FormPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void FormPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Point mousePosition = MousePosition;
                mousePosition.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePosition;
            }
        }

        public void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void verProyectosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerProyectosControl verProyectosControl = new VerProyectosControl(this);
            addUserControl(verProyectosControl);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomeControl homeControl = new HomeControl(this);
            addUserControl(homeControl);
        }

        private void verInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerInventarioControl verInventarioControl = new VerInventarioControl();
            addUserControl(verInventarioControl);
        }

        private void verEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerEmpleadosControl verEmpleadosControl = new VerEmpleadosControl();
            addUserControl(verEmpleadosControl);
        }

        private void agregarProyectosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string REQUIRED = "AGREGAR_PROYECTOS";

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var addProyectosForm = new AddProyectosForm();

            addProyectosForm.FormClosed += (s, ev) =>
            {
                // Solo refresco si realmente guardó
                if (!addProyectosForm.ProyectoGuardado) return;

                var ver = FindControl<VerProyectosControl>(this);
                ver?.Refrescar();
            };

            addProyectosForm.Show();
        }

        private static T FindControl<T>(Control parent) where T : Control
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T t) return t;

                var found = FindControl<T>(c);
                if (found != null) return found;
            }
            return null;
        }



        private void cargarEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmpleadosForm addEmpleadosForm = new AddEmpleadosForm();
            addEmpleadosForm.Show();
        }

        private void agregarMaterialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMaterialesForm addMaterialesForm = new AddMaterialesForm();
            addMaterialesForm.Show();
        }

        private void agregarProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProveedorControl proveedorControl = new ProveedorControl();
            addUserControl(proveedorControl);
        }

        private void consultarInfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InformesDeCompraControl informesDeCompraControl = new InformesDeCompraControl();
            addUserControl(informesDeCompraControl);
        }

        private void verLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VerLogsForm verLogsForm = new VerLogsForm();
            verLogsForm.Show();
        }

        private void gestionarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Si abriste MainForm con el ctor sin servicios, marcá el error claro:
            if (_rolesService is null || _usuarioService is null)
                throw new InvalidOperationException("MainForm fue creado sin servicios. Usá MainForm(RolesService, UsuarioService).");

            var gestionUsuariosControl = new GestionUsuariosControl(_rolesService, _usuarioService);
            addUserControl(gestionUsuariosControl);
        }
    }
}
