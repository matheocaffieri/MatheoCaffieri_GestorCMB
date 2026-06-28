using Services.RoleService;
using Services.LoginService.Logic;
using Services.Login;
using Services.Language;
using Services.RoleService.Logic;
using System;
using ParametrosServiceLogic = Services.RoleService.Logic.ParametrosService;
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
        private readonly ParametrosServiceLogic _parametrosService;

        public MainForm()
        {
            InitializeComponent();
            AplicarTraducciones();
            var homeControl = new HomeControl(this);   // HomeControl NO lleva servicios
            addUserControl(homeControl);
        }

        // Runtime: pasás servicios acá (HomeControl sigue sin params)
        public MainForm(RolesService rolesService, UsuarioService usuarioService, ParametrosServiceLogic parametrosService)
        {
            InitializeComponent();
            AplicarTraducciones();
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _parametrosService = parametrosService ?? throw new ArgumentNullException(nameof(parametrosService));


            SetupMenuPermissionTags();
            ApplyMenuPermissions();

            var homeControl = new HomeControl(this);
            addUserControl(homeControl);
        }

        // Items del menú que no están localizados via Designer (sin ApplyResources)
        private void AplicarTraducciones()
        {
            configurarParametrosToolStripMenuItem.Text =
                LanguageService.Current?.T("mnu_configurar_parametros") ?? "Configurar Parámetros";
        }

        private bool Require(string permiso)
        {
            if (SessionContext.Has(permiso)) return true;
            MessageBox.Show(
                LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos.",
                LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }



        private void SetupMenuPermissionTags()
        {
            // Proyectos
            verProyectosToolStripMenuItem.Tag = TipoPermiso.VER_PROYECTOS.ToString();
            agregarProyectosToolStripMenuItem.Tag = TipoPermiso.GESTIONAR_PROYECTOS.ToString();

            // Inventario
            verInventarioToolStripMenuItem.Tag = TipoPermiso.VER_INVENTARIO.ToString();
            agregarMaterialesToolStripMenuItem.Tag = TipoPermiso.GESTIONAR_MATERIALES.ToString();
            consultarInfToolStripMenuItem.Tag = TipoPermiso.VER_INFORMES_COMPRA.ToString();
            agregarProveedoresToolStripMenuItem.Tag = TipoPermiso.GESTIONAR_PROVEEDORES.ToString();

            // Personal
            verEmpleadosToolStripMenuItem.Tag = TipoPermiso.VER_EMPLEADOS.ToString();
            cargarEmpleadosToolStripMenuItem.Tag = TipoPermiso.GESTIONAR_EMPLEADOS.ToString();

            // Ajustes
            verLogsToolStripMenuItem.Tag = TipoPermiso.VER_LOGS.ToString();
            gestionarUsuariosToolStripMenuItem.Tag = TipoPermiso.GESTIONAR_USUARIOS.ToString();
            configurarParametrosToolStripMenuItem.Tag = TipoPermiso.CONFIGURAR_PARAMETROS.ToString();

            // Menú raíz sin permiso propio (se habilita si tiene algún hijo habilitado)
            homeToolStripMenuItem.Tag = null;
            proyectosToolStripMenuItem.Tag = null;
            inventarioToolStripMenuItem.Tag = null;
            personalToolStripMenuItem.Tag = null;
            ajustesToolStripMenuItem.Tag = null;
        }


        private void ApplyMenuPermissions()
        {
            // Si todavía no hay sesión cargada, no tocar el menú
            if (SessionContext.Accesos == null || SessionContext.Accesos.Count == 0)
                return;

            foreach (var top in menuStrip1.Items.OfType<ToolStripMenuItem>())
                ApplyMenuItemPermissions(top);
        }

        private static void ApplyMenuItemPermissions(ToolStripMenuItem item)
        {
            // Primero procesar hijos
            foreach (var child in item.DropDownItems.OfType<ToolStripMenuItem>())
                ApplyMenuItemPermissions(child);

            // Evaluar permiso propio si existe
            var required = item.Tag as string;

            if (!string.IsNullOrWhiteSpace(required))
            {
                item.Enabled = SessionContext.Has(required);
                return;
            }

            // Si no tiene permiso propio, y es un "contenedor", se habilita si algún hijo quedó habilitado
            if (item.DropDownItems.Count > 0)
            {
                bool anyChildEnabled = item.DropDownItems
                    .OfType<ToolStripMenuItem>()
                    .Any(mi => mi.Enabled);

                item.Enabled = anyChildEnabled;
            }
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        // Maximizado manual: con FormBorderStyle.None, el maximizado nativo (MaximizedBounds /
        // WM_GETMINMAXINFO) interpreta los valores relativos al monitor PRIMARIO y los "compensa"
        // contra el monitor real, así que en un monitor de otro tamaño queda corrido o se pasa.
        // Seteamos Bounds directo al área de trabajo del monitor actual y listo.
        private bool _maximizado;
        private Rectangle _boundsRestaurar;

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;
            const int borderWidth = 6;

            if (m.Msg == WM_NCHITTEST && WindowState != FormWindowState.Maximized && !_maximizado)
            {
                base.WndProc(ref m);
                short screenX = (short)(m.LParam.ToInt32() & 0xFFFF);
                short screenY = (short)(m.LParam.ToInt32() >> 16);
                Point cursor = PointToClient(new Point(screenX, screenY));

                bool left   = cursor.X < borderWidth;
                bool right  = cursor.X >= ClientSize.Width - borderWidth;
                bool top    = cursor.Y < borderWidth;
                bool bottom = cursor.Y >= ClientSize.Height - borderWidth;

                if      (top    && left)  m.Result = (IntPtr)HTTOPLEFT;
                else if (top    && right) m.Result = (IntPtr)HTTOPRIGHT;
                else if (bottom && left)  m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (bottom && right) m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (left)            m.Result = (IntPtr)HTLEFT;
                else if (right)           m.Result = (IntPtr)HTRIGHT;
                else if (top)             m.Result = (IntPtr)HTTOP;
                else if (bottom)          m.Result = (IntPtr)HTBOTTOM;
                return;
            }
            base.WndProc(ref m);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void buttonMaximize_Click(object sender, EventArgs e)
        {
            if (_maximizado)
            {
                Bounds = _boundsRestaurar;
                _maximizado = false;
                buttonMaximize.Text = "□";
            }
            else
            {
                _boundsRestaurar = Bounds;
                Bounds = Screen.FromControl(this).WorkingArea;
                _maximizado = true;
                buttonMaximize.Text = "❐";
            }
        }

        private void FormPanel_DoubleClick(object sender, EventArgs e)
        {
            buttonMaximize_Click(sender, e);
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
            if (e.Button == MouseButtons.Left && WindowState != FormWindowState.Maximized && !_maximizado)
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
            if (!Require(TipoPermiso.VER_PROYECTOS.ToString()))
                return;

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
            if (!Require(TipoPermiso.VER_INVENTARIO.ToString()))
                return;

            VerInventarioControl verInventarioControl = new VerInventarioControl();
            addUserControl(verInventarioControl);
        }

        private void verEmpleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.VER_EMPLEADOS.ToString()))
                return;

            VerEmpleadosControl verEmpleadosControl = new VerEmpleadosControl();
            addUserControl(verEmpleadosControl);
        }

        private void agregarProyectosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string REQUIRED = "GESTIONAR_PROYECTOS";

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
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

            addProyectosForm.Show(this);
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
            if (!Require(TipoPermiso.GESTIONAR_EMPLEADOS.ToString()))
                return;

            AddEmpleadosForm addEmpleadosForm = new AddEmpleadosForm();
            addEmpleadosForm.Show(this);
        }

        private void agregarMaterialesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.GESTIONAR_MATERIALES.ToString()))
                return;

            AddMaterialesForm addMaterialesForm = new AddMaterialesForm();

            addMaterialesForm.FormClosed += (s, ev) =>
            {
                // Solo refresco si realmente guardó
                if (addMaterialesForm.DialogResult != DialogResult.OK) return;

                var ver = FindControl<VerInventarioControl>(this);
                ver?.Refrescar();
            };

            addMaterialesForm.Show(this);
        }

        private void agregarProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.GESTIONAR_PROVEEDORES.ToString()))
                return;

            ProveedorControl proveedorControl = new ProveedorControl();
            addUserControl(proveedorControl);
        }

        private void consultarInfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.VER_INFORMES_COMPRA.ToString()))
                return;

            InformesDeCompraControl informesDeCompraControl = new InformesDeCompraControl();
            addUserControl(informesDeCompraControl);
        }

        private void verLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.VER_LOGS.ToString()))
                return;

            VerLogsForm verLogsForm = new VerLogsForm();
            verLogsForm.Show(this);
        }

        private void gestionarUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.GESTIONAR_USUARIOS.ToString()))
                return;

            if (_rolesService is null || _usuarioService is null)
                throw new InvalidOperationException("MainForm fue creado sin servicios. Usá MainForm(RolesService, UsuarioService).");

            var gestionUsuariosControl = new GestionUsuariosControl(_rolesService, _usuarioService);
            addUserControl(gestionUsuariosControl);
        }

        private void configurarParametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Require(TipoPermiso.CONFIGURAR_PARAMETROS.ToString()))
                return;

            var control = new ConfigurarParametrosControl(_parametrosService);
            addUserControl(control);
        }
    }
}
