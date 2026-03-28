using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AgregarEmpleadoProyectoForm : Form
    {
        private readonly Guid _idProyecto;
        private const string REQUIRED = "CARGAR_EMPLEADOS";

        private System.Drawing.Point _mouseLocation;

        public AgregarEmpleadoProyectoForm(Guid idProyecto)
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

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            _idProyecto = idProyecto;

            gestionarEmpleadosDetalleLayoutPanel.AutoScroll = true;
            gestionarEmpleadosDetalleLayoutPanel.WrapContents = false;
            gestionarEmpleadosDetalleLayoutPanel.FlowDirection = FlowDirection.TopDown;

            CargarEmpleadosItems();
        }

        private void CargarEmpleadosItems(string filtro = "")
        {
            gestionarEmpleadosDetalleLayoutPanel.Controls.Clear();

            var empleados = ((IGenericRepository<Empleado>)new EmpleadoBL()).GetAll();

            // ✅ SOLO ACTIVOS
            empleados = empleados.Where(e => e.IsActive).ToList();

            // opcional: filtrar por nombre/apellido/dni (pero siempre dentro de activos)
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLowerInvariant();
                empleados = empleados.Where(e =>
                        $"{e.Nombre} {e.Apellido}".ToLowerInvariant().Contains(f) ||
                        (e.NroDocumento + "").Contains(f))
                    .ToList();
            }

            foreach (var emp in empleados)
            {
                var item = new AddEmpleadoProyectoItemControl();

                item.Bind(emp.IdEmpleado, emp.Nombre, emp.Apellido, emp.NroDocumento,
                          (decimal)emp.Sueldo, emp.CantidadProyectosActivos);

                item.AgregarClick += Item_AgregarEmpleadoClick;

                gestionarEmpleadosDetalleLayoutPanel.Controls.Add(item);
            }
        }

        private void Item_AgregarEmpleadoClick(object sender, AgregarEmpleadoEventArgs e)
        {
            try
            {
                var valorGanancia = (double)(e.Sueldo * ParametrosContext.MargenEmpleados);
                var svc = new ProyectoEmpleadoBL();
                svc.AgregarEmpleadoDetalleProyecto(_idProyecto, e.IdEmpleado, valorGanancia);

                // refrescar: para que cambie "Proyectos activos" y deshabilite si llegó a 3
                CargarEmpleadosItems();
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarEmpleadosItems();
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarEmpleadosItems();
            }
        }

        private void buttonExitAP_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
