using BL;
using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MatheoCaffieri_GestorCMB.ItemControls;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AgregarEmpleadoProyectoForm : Form
    {
        private readonly Guid _idProyecto;

        public AgregarEmpleadoProyectoForm(Guid idProyecto)
        {
            InitializeComponent();
            _idProyecto = idProyecto;

            gestionarEmpleadosDetalleLayoutPanel.AutoScroll = true;
            gestionarEmpleadosDetalleLayoutPanel.WrapContents = false;
            gestionarEmpleadosDetalleLayoutPanel.FlowDirection = FlowDirection.TopDown;

            CargarEmpleadosItems();
        }

        private void CargarEmpleadosItems(string filtro = "")
        {
            gestionarEmpleadosDetalleLayoutPanel.Controls.Clear();

            // Traer empleados (repo/BL que ya tengas para listar)
            var empleados = ((IGenericRepository<Empleado>)new EmpleadoBL()).GetAll();

            // opcional: filtrar por nombre/apellido/dni
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLowerInvariant();
                empleados = empleados.Where(e =>
                        $"{e.Nombre} {e.Apellido}".ToLowerInvariant().Contains(f) ||
                        (e.NroDocumento + "").Contains(f))
                    .ToList();
            }

            // Solo activos (si querés)
            // empleados = empleados.Where(e => e.IsActive).ToList();

            foreach (var emp in empleados)
            {
                var item = new AddEmpleadoProyectoItemControl();

                item.Bind(
                    emp.IdEmpleado,
                    emp.Nombre,
                    emp.Apellido,
                    emp.NroDocumento,
                    (decimal)emp.Sueldo,
                    emp.CantidadProyectosActivos
                );

                item.AgregarClick += Item_AgregarEmpleadoClick;

                gestionarEmpleadosDetalleLayoutPanel.Controls.Add(item);
            }


        }

        private void Item_AgregarEmpleadoClick(object sender, AgregarEmpleadoEventArgs e)
        {
            try
            {
                var svc = new ProyectoEmpleadoBL();
                svc.AgregarEmpleadoDetalleProyecto(_idProyecto, e.IdEmpleado, valorGanancia: 0);

                // refrescar: para que cambie "Proyectos activos" y deshabilite si llegó a 3
                CargarEmpleadosItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // refresco también, por si el BL rechazó por cupo o duplicado
                CargarEmpleadosItems();
            }
        }

        private void buttonExitAP_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
