using Services.RoleService;
using DomainModel.Exceptions;
using Services.Login;
using Services.Language;
using Services.Logs;
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
using Services.RoleService.Logic;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;
using AccesoServiceLogic = Services.RoleService.Logic.AccesoService;



namespace MatheoCaffieri_GestorCMB
{
    public partial class AccesosForm : Form
    {
        private readonly AccesoServiceLogic _accesoSrv;
        private readonly RolesServiceLogic _rolesSrv;


        private System.Drawing.Point _mouseLocation;

        public AccesosForm()
        {
            InitializeComponent();

            FormPanel.MouseDown += (s, e) => { _mouseLocation = new System.Drawing.Point(-e.X, -e.Y); };
            FormPanel.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pos = MousePosition;
                    pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                    Location = pos;
                }
            };

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
            var roles = _rolesSrv.ListarRoles();
            comboBoxRol.DisplayMember = "Nombre";
            comboBoxRol.ValueMember = "Id";
            comboBoxRol.DataSource = roles;
        }

        private void buttonExitAE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSavePermiso_Click_1(object sender, EventArgs e)
        {
            var nombre = (textBoxNombrePermiso.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                LoggerLogic.Warn("[AccesosForm] Validación: nombre de permiso vacío.");
                MessageBox.Show(LanguageService.Current?.T("val_nombre_requerido") ?? "El nombre es obligatorio.");
                textBoxNombrePermiso.Focus();
                return;
            }

            try
            {
                var key = (TipoPermiso)comboBoxAcceso.SelectedItem;
                _accesoSrv.Crear(nombre, key);
                LoggerLogic.Info($"[AccesosForm] Permiso creado: '{nombre}' ({key})");

                MessageBox.Show(LanguageService.Current?.T("msg_permiso_creado") ?? "Permiso creado.");
                textBoxNombrePermiso.Clear();
                CargarAccesos();
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[AccesosForm] Validación al crear permiso: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AccesosForm] Falla al crear permiso.", ex);
                MessageBox.Show(LanguageService.Current?.T("err_crear_permiso") ?? "Error al crear el permiso.");
            }
        }

        private void buttonAsignarPermiso_Click_1(object sender, EventArgs e)
        {
            if (!(comboBoxPermiso.SelectedValue is Guid permisoId))
            {
                LoggerLogic.Warn("[AccesosForm] Validación: permiso no seleccionado al asignar.");
                MessageBox.Show(LanguageService.Current?.T("val_permiso_requerido") ?? "Elegí un permiso."); return;
            }

            if (!(comboBoxRol.SelectedValue is Guid rolId))
            {
                LoggerLogic.Warn("[AccesosForm] Validación: rol no seleccionado al asignar permiso.");
                MessageBox.Show(LanguageService.Current?.T("val_rol_requerido") ?? "Elegí un rol."); return;
            }

            try
            {
                _rolesSrv.AsignarPermisoARol(rolId, permisoId);
                LoggerLogic.Info($"[AccesosForm] Permiso asignado al rol. Rol={rolId} Permiso={permisoId}");
                MessageBox.Show(LanguageService.Current?.T("msg_permiso_asignado") ?? "Permiso asignado al rol.");
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[AccesosForm] Validación al asignar permiso: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AccesosForm] Falla al asignar permiso al rol.", ex);
                MessageBox.Show(LanguageService.Current?.T("err_asignar_permiso") ?? "Error al asignar el permiso al rol.");
            }
        }
    }
}
