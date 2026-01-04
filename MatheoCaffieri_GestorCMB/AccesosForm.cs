using BL.AccessBL;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Services.RoleService;  
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.RoleService.Logic;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;
using AccesoServiceLogic = Services.RoleService.Logic.AccesoService;



namespace MatheoCaffieri_GestorCMB
{
    public partial class AccesosForm : Form
    {
        private readonly AccesoServiceLogic _accesoSrv;
        private readonly RolesServiceLogic _rolesSrv;


        public AccesosForm()
        {
            InitializeComponent();

            var cs = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"].ConnectionString;
            _accesoSrv = AccessServicesFactory.CreateAccesoService(cs);
            _rolesSrv = AccessServicesFactory.CreateRolesService(cs);
        }

        private void AccesosForm_Load(object sender, EventArgs e)
        {
            // combo con el enum
            comboBoxAcceso.DataSource = Enum.GetValues(typeof(TipoPermiso));
            CargarAccesos();
            CargarRoles();
        }

        private void CargarAccesos()
        {
            var accesos = _accesoSrv.Listar(); // Acceso { Id, Nombre, DataKey }
            comboBoxPermiso.DisplayMember = "Nombre";
            comboBoxPermiso.ValueMember = "Id";    // Guid (getter público)
            comboBoxPermiso.DataSource = accesos;
        }

        private void CargarRoles()
        {
            var roles = _rolesSrv.ListarRoles(); // RolCompuesto con Id seteado en BL
            comboBoxRol.DisplayMember = "Nombre";
            comboBoxRol.ValueMember = "Id";        // Guid (getter público)
            comboBoxRol.DataSource = roles;
        }

       

        // (opcional) si tenés un botón “Cerrar”
        private void buttonExitAE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSavePermiso_Click_1(object sender, EventArgs e)
        {
            var nombre = (textBoxNombrePermiso.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre es obligatorio.");
                textBoxNombrePermiso.Focus();
                return;
            }

            try
            {
                var key = (TipoPermiso)comboBoxAcceso.SelectedItem;
                _accesoSrv.Crear(nombre, key);

                MessageBox.Show("Permiso creado.");
                textBoxNombrePermiso.Clear();
                CargarAccesos(); // refresca el combo de “Seleccionar permiso”
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear permiso: " + ex.Message);
            }
        }

        private void buttonAsignarPermiso_Click_1(object sender, EventArgs e)
        {
            if (!(comboBoxPermiso.SelectedValue is Guid permisoId))
            { MessageBox.Show("Elegí un permiso."); return; }

            if (!(comboBoxRol.SelectedValue is Guid rolId))
            { MessageBox.Show("Elegí un rol."); return; }

            try
            {
                _rolesSrv.AsignarPermisoARol(rolId, permisoId);
                MessageBox.Show("Permiso asignado al rol.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al asignar: " + ex.Message);
            }
        }
    }
}
