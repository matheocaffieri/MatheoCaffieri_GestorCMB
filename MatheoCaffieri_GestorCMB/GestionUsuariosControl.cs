using Services.RoleService.Logic;
using BL.LoginBL;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.LoginService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.Tools;


namespace MatheoCaffieri_GestorCMB
{
    public enum TreeMode { CategoriasPermisos, RolesPermisos }
    

    public partial class GestionUsuariosControl : UserControl
    {
        private TreeMode _treeMode;



        // Servicios BL (reciben CS)
        private readonly UsuarioService _usuarioSrv;
        private readonly RolesService _rolesSrv;
        private readonly UsuarioPermisosService _userPermsSrv;
        private readonly AccesoService _accesoSrv;

        // Estado UI
        private readonly Dictionary<TipoPermiso, TreeNode> _nodosPorPermiso = new Dictionary<TipoPermiso, TreeNode>();
        private bool _updatingChecks = false;
        private Usuario _usuarioActual;

        public GestionUsuariosControl(RolesService rolesService, UsuarioService usuarioService)
        {
            InitializeComponent();

            var cs = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"].ConnectionString;

            
            _rolesSrv = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _usuarioSrv = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));

            
            _userPermsSrv = new UsuarioPermisosService(cs);
            _accesoSrv = new AccesoService(cs);

            treeViewRoles.CheckBoxes = true;
        }


        private void CargarComboIdiomas()
        {
            var items = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string,string>("es", "Español"),
        new KeyValuePair<string,string>("en", "English")
    };

            comboBoxIdioma.DisplayMember = "Value"; // lo que se ve
            comboBoxIdioma.ValueMember = "Key";   // el código ("es"/"en")
            comboBoxIdioma.DataSource = items;
            comboBoxIdioma.SelectedValue = "es";    // por defecto
        }
        private readonly RolesService _rolService;
        private readonly UsuarioService _usuarioService;

        private void ObtenerUsuariosItems()
        {
            var usuarios = _usuarioSrv.ObtenerTodos();

            UsuariosLayoutPanel.SuspendLayout();
            UsuariosLayoutPanel.Controls.Clear();

            foreach (var u in usuarios)
            {
                // ESTO ESTÁ BIEN (los campos que sí existen y están asignados)
                var item = new ItemControls.UsuarioItemControl(_rolesSrv, _usuarioSrv)
                {
                    MailUsuario = u.Mail,
                    Activo = u.IsActive,   // pinta el switch según DB
                    Tag = u
                };

                // ON/OFF -> persiste isActive
                item.ActivoChanged += (s, on) =>
                {
                    var usr = (Usuario)((Control)s).Tag;
                    try
                    {
                        _usuarioSrv.SetActivo(usr.IdUsuario, on);
                        usr.IsActive = on;
                    }
                    catch (Exception ex)
                    {
                        var ctrl = (ItemControls.UsuarioItemControl)s;
                        ctrl.Activo = !on; // revertir
                        MessageBox.Show("No se pudo actualizar el estado: " + ex.Message);
                    }
                    item.SetUsuario(u);
                    UsuariosLayoutPanel.Controls.Add(item);
                };



                // Selección de usuario
                item.Click += (s, ev) =>
                {
                    _usuarioActual = (Usuario)((Control)s).Tag;

                    // SOLO refrescá checks si estás en la vista Categorías→Permisos
                    if (_treeMode == TreeMode.CategoriasPermisos)
                    {
                        CargarPermisosUsuario(_usuarioActual);
                    }
                    // En vista Roles→Permisos NO toques el tree (así no se borran los checks)
                };

                UsuariosLayoutPanel.Controls.Add(item);
            }

            UsuariosLayoutPanel.ResumeLayout();
        }


        // ---------- Árbol de permisos (categorías visuales + hojas enum) ----------

        private Guid GetRolId(object rol)
        {
            var p = rol.GetType().GetProperty("Id");
            if (p == null) return Guid.Empty;
            var v = p.GetValue(rol, null);
            return v is Guid g ? g : Guid.Empty;
        }

        private string GetRolNombre(object rol)
        {
            // soporta Nombre o Name
            var p = rol.GetType().GetProperty("Nombre") ?? rol.GetType().GetProperty("Name");
            var v = p != null ? p.GetValue(rol, null) as string : null;
            return string.IsNullOrWhiteSpace(v) ? "(Rol)" : v;
        }

        private void CargarComboRoles()
        {
            var items = new List<RolItem>();

            // Opción “Sin rol”
            items.Add(new RolItem { Id = Guid.Empty, Nombre = "(Sin rol)" });

            foreach (var rol in _rolesSrv.ListarRoles())
            {
                // Tu RolCompuesto tiene Id seteado por reflexión y Nombre
                var pId = rol.GetType().GetProperty("Id");
                var pNombre = rol.GetType().GetProperty("Nombre") ?? rol.GetType().GetProperty("Name");

                var id = pId != null ? (Guid)pId.GetValue(rol, null) : Guid.Empty;
                var nombre = pNombre != null ? (string)pNombre.GetValue(rol, null) : "(Rol)";

                items.Add(new RolItem { Id = id, Nombre = nombre });
            }

            // Ajustá el nombre del combo si el tuyo no es este
            comboBoxRol.DisplayMember = "Nombre";
            comboBoxRol.ValueMember = "Id";
            comboBoxRol.DataSource = items;
            comboBoxRol.SelectedIndex = 0;
        }


        private void CargarTreeRolesPermisos()
        {
            _nodosPorPermiso.Clear();
            treeViewRoles.BeginUpdate();
            treeViewRoles.Nodes.Clear();

            var roles = _rolesSrv.ListarRoles();

            foreach (var rol in roles)
            {
                var rolId = GetRolId(rol);
                var rolNombre = GetRolNombre(rol);

                var nodoRol = new TreeNode(rolNombre) { Tag = rolId };

                // permisos actuales del rol (desde RolesService)
                var actuales = new HashSet<TipoPermiso>(_rolesSrv.ObtenerPermisosDeRol(rolId));

                foreach (TipoPermiso p in Enum.GetValues(typeof(TipoPermiso)))
                {
                    var leaf = new TreeNode(p.ToString())
                    {
                        Tag = new NodoPermRol { RolId = rolId, Permiso = p },
                        Checked = actuales.Contains(p)
                    };
                    nodoRol.Nodes.Add(leaf);
                }
                treeViewRoles.Nodes.Add(nodoRol);
            }

            treeViewRoles.ExpandAll();
            treeViewRoles.EndUpdate();

            treeViewRoles.AfterCheck -= treeViewRoles_AfterCheck_Roles;
            treeViewRoles.AfterCheck += treeViewRoles_AfterCheck_Roles;
        }

        private bool _autoGuardarRoles = true; // ponelo en false si querés solo vista

        private void treeViewRoles_AfterCheck_Roles(object sender, TreeViewEventArgs e)
        {
            if (_updatingChecks) return;

            try
            {
                _updatingChecks = true;

                // Marcar/desmarcar hijos cuando tildás el ROL (padre)
                if (e.Node.Tag is Guid && e.Node.Nodes.Count > 0)
                {
                    foreach (TreeNode child in e.Node.Nodes)
                        child.Checked = e.Node.Checked;
                }
            }
            finally { _updatingChecks = false; }

            if (!_autoGuardarRoles) return;

            // Guardado inmediato
            try
            {
                AplicarGuardadoNodo(e.Node);
            }
            catch (Exception ex)
            {
                // Revertir el cambio visual si falló el guardado
                RevertCheck(e.Node, !e.Node.Checked);
                MessageBox.Show("No se pudo actualizar permisos: " + ex.Message);
            }
        }

        private void AplicarGuardadoNodo(TreeNode node)
        {
            // Hoja: permiso concreto
            if (node.Tag is NodoPermRol npr)
            {
                // Asegura que exista el Acceso para ese TipoPermiso; si no, lo crea
                var accesoId = _accesoSrv.GetOrCreateId(
                    npr.Permiso,
                    npr.Permiso.ToString().Replace('_', ' ')
                );

                if (node.Checked)
                    _rolesSrv.AsignarPermisoARol(npr.RolId, accesoId);
                else
                    _rolesSrv.QuitarPermisoDeRol(npr.RolId, accesoId);

                return;
            }

            // Padre: rol. Aplica a todos sus hijos
            if (node.Tag is Guid)
            {
                foreach (TreeNode child in node.Nodes)
                    AplicarGuardadoNodo(child);
            }
        }

        private void RevertCheck(TreeNode node, bool state)
        {
            _updatingChecks = true;
            try
            {
                node.Checked = state;
                foreach (TreeNode child in node.Nodes)
                    RevertCheck(child, state);
            }
            finally { _updatingChecks = false; }
        }



        private void AddPermisoLeaf(TreeNode parent, string texto, TipoPermiso p)
        {
            var leaf = new TreeNode(texto) { Tag = p };
            parent.Nodes.Add(leaf);
            _nodosPorPermiso[p] = leaf;
        }

        private void treeViewRoles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_updatingChecks) return;
            try
            {
                _updatingChecks = true;
                CheckAllChildren(e.Node, e.Node.Checked);
                UpdateParents(e.Node);
            }
            finally { _updatingChecks = false; }
        }

        private void CheckAllChildren(TreeNode node, bool isChecked)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = isChecked;
                if (child.Nodes.Count > 0) CheckAllChildren(child, isChecked);
            }
        }

        private void UpdateParents(TreeNode node)
        {
            var parent = node.Parent;
            while (parent != null)
            {
                bool allChecked = true, anyChecked = false;
                foreach (TreeNode child in parent.Nodes)
                {
                    if (child.Checked) anyChecked = true; else allChecked = false;
                }
                parent.Checked = allChecked && anyChecked;
                parent = parent.Parent;
            }
        }
        // -------------------------------------------------------------------------

        private void CargarPermisosUsuario(Usuario u)
        {
            _usuarioActual = u;

            _updatingChecks = true;
            foreach (TreeNode root in treeViewRoles.Nodes) UncheckAll(root);
            _updatingChecks = false;

            
            foreach (TipoPermiso permiso in Enum.GetValues(typeof(TipoPermiso)))
            {
                TreeNode node;
                if (u.TienePermiso(permiso) && _nodosPorPermiso.TryGetValue(permiso, out node))
                    node.Checked = true;
            }
        }

        private void UncheckAll(TreeNode node)
        {
            node.Checked = false;
            foreach (TreeNode child in node.Nodes) UncheckAll(child);
        }

        private List<TipoPermiso> GetCheckedPermisosLeafs()
        {
            var list = new List<TipoPermiso>();
            foreach (TreeNode root in treeViewRoles.Nodes) CollectLeafs(root, list);
            return list;
        }
        private void CollectLeafs(TreeNode node, List<TipoPermiso> acc)
        {
            if (node.Nodes.Count == 0 && node.Checked && node.Tag is TipoPermiso p) { acc.Add(p); return; }
            foreach (TreeNode child in node.Nodes) CollectLeafs(child, acc);
        }

        private void btnGuardarPermisos_Click(object sender, EventArgs e)
        {
            if (_usuarioActual == null) { MessageBox.Show("Seleccioná un usuario."); return; }
            var permisos = GetCheckedPermisosLeafs();
            _userPermsSrv.ReemplazarDirectos(_usuarioActual.IdUsuario, permisos);
            MessageBox.Show("Permisos actualizados.");
        }


        private void buttonGestionPermisos_Click(object sender, EventArgs e)
        {
            AccesosForm accesosForm = new AccesosForm();
            accesosForm.Show();
        }


        private void buttonAgregarUser_Click(object sender, EventArgs e)
        {
            var mail = (textBox1?.Text ?? "").Trim();
            var passPlano = textBox2?.Text ?? "";
            var telStr = (textBox4?.Text ?? "").Trim();
            var idi = (comboBoxIdioma?.SelectedValue as string) ?? "es";

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(mail))
            {
                MessageBox.Show("El mail es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1?.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(passPlano))
            {
                MessageBox.Show("La contraseña es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2?.Focus();
                return;
            }
            if (!string.IsNullOrWhiteSpace(telStr) && !int.TryParse(telStr, out _))
            {
                MessageBox.Show("El teléfono debe ser numérico (sin espacios ni guiones).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4?.Focus();
                return;
            }

            try
            {

                var existente = _usuarioSrv.ObtenerPorMail(mail);
                if (existente != null)
                { MessageBox.Show("Ya existe un usuario con ese mail."); return; }

                var nuevo = new Usuario
                {
                    IdUsuario = Guid.NewGuid(),
                    Mail = mail,
                    IsActive = true,
                    Telefono = string.IsNullOrWhiteSpace(telStr) ? 0 : int.Parse(telStr),
                    Idioma = idi
                };

                _usuarioSrv.CrearUsuario(nuevo, passPlano); // hashea adentro
                var sel = comboBoxRol.SelectedValue;
                if (sel is Guid rolId && rolId != Guid.Empty)
                    _rolesSrv.AsignarUsuarioARol(rolId, nuevo.IdUsuario);

                MessageBox.Show("Usuario creado correctamente.");
                textBox1.Clear(); textBox2.Clear(); textBox4.Clear();
                comboBoxRol.SelectedIndex = 0;
                ObtenerUsuariosItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear usuario: " + ex.Message);
            }
        }

        private void GestionUsuariosControl_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            if (_rolesSrv == null || _usuarioSrv == null)
                throw new InvalidOperationException("GestionUsuariosControl requiere RolesService y UsuarioService.");

            ObtenerUsuariosItems();
            CargarComboRoles();
            MostrarVistaRoles();
            treeViewRoles.CheckBoxes = true;
            CargarComboIdiomas();
        }



        private void buttonAgregarRol_Click(object sender, EventArgs e)
        {
            var nombreRol = (textBox3?.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                MessageBox.Show("El nombre del rol es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3?.Focus();
                return;
            }

            try
            {
                // Usa el servicio ya instanciado arriba
                var idRol = _rolesSrv.CrearRol(nombreRol);

                MessageBox.Show($"Rol '{nombreRol}' creado correctamente.\nID: {idRol}",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox3.Clear();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el rol: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CargarComboRoles();            // <- acá
            CargarTreeRolesPermisos();
        }

        private void MostrarVistaCategorias()
        {
            _treeMode = TreeMode.CategoriasPermisos;
            treeViewRoles.AfterCheck -= treeViewRoles_AfterCheck_Roles;
            CargarTreeRolesPermisos();               

        }

        private void MostrarVistaRoles()
        {
            _treeMode = TreeMode.RolesPermisos;
            treeViewRoles.AfterCheck -= treeViewRoles_AfterCheck;
            CargarTreeRolesPermisos();         
            treeViewRoles.AfterCheck -= treeViewRoles_AfterCheck_Roles;
            treeViewRoles.AfterCheck += treeViewRoles_AfterCheck_Roles;
        }

    }
}
