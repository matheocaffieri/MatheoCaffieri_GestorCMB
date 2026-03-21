using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services;
using Services.Language;
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

namespace MatheoCaffieri_GestorCMB
{
    public partial class DetalleProyectoControl : UserControl
    {
        private MainForm mainForm;
        private Proyecto _proyecto;

        public DetalleProyectoControl(MainForm mainForm, Proyecto proyecto)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            _proyecto = proyecto;

            NumProyecto = "Proyecto #";
            DescripcionProyecto = proyecto.Descripcion;
            NombreCliente = proyecto.Cliente?.NombreContacto ?? "Cliente desconocido";
            FechaInicio = proyecto.FechaInicio.ToString("dd/MM/yyyy") ?? "Sin fecha";
            EstadoProyecto = proyecto.Estado.ToString() ?? "Estado no disponible";
            UbicacionProyecto = proyecto.Ubicacion ?? "Ubicación desconocida";

            buttonModificar.Click += buttonModificar_Click;
        }

        public Proyecto ProyectoData
        {
            get => _proyecto;
            set => _proyecto = value;
        }

        public string NumProyecto
        {
            get => labelNumProyecto.Text;
            set => labelNumProyecto.Text = value ?? "Proyecto #N/A";
        }

        public string DescripcionProyecto
        {
            get => labelDescProyecto.Text;
            set => labelDescProyecto.Text = value ?? "Sin descripción";
        }

        public string NombreCliente
        {
            get => labelNomCliente.Text;
            set => labelNomCliente.Text = value ?? "Desconocido";
        }

        public string FechaInicio
        {
            get => labelFechaInicio.Text;
            set => labelFechaInicio.Text = value ?? "Fecha no disponible";
        }

        public string EstadoProyecto
        {
            get => labelEstado.Text;
            set => labelEstado.Text = value ?? "Estado desconocido";
        }

        public string UbicacionProyecto
        {
            get => labelUbiProyecto.Text;
            set => labelUbiProyecto.Text = value ?? "Ubicación no especificada";
        }

        public string TotalEmpleados
        {
            get => labelTotalEmpleados.Text;
            set => labelTotalEmpleados.Text = value ?? "0";
        }

        public string TotalMateriales
        {
            get => labelTotalMateriales.Text;
            set => labelTotalMateriales.Text = value ?? "0";
        }

        public string UtilidadEmpresa
        {
            get => labelUtilidadEmpresa.Text;
            set => labelUtilidadEmpresa.Text = value ?? "0";
        }

        private void ObtenerDetallesEmpleadosItems(Guid idProyecto)
        {
            IDetalleGeneric<DetalleProyectoEmpleado> detalleRepo = new DetalleEmpleadoBL();
            List<DetalleProyectoEmpleado> detalleEmpleados = detalleRepo.GetAll(idProyecto);

            flowLayoutPanelEmp.Controls.Clear();

            detalleEmpleados.ForEach(e =>
            {
                DetalleEmpleadoItemControl empleadosItemControl = new DetalleEmpleadoItemControl
                {
                    InfoNombreApellido = $"{e.Empleado.Nombre} {e.Empleado.Apellido}",
                    InfoNroDocumento = $"{e.Empleado.NroDocumento}",
                    InfoSueldo = $"{e.Empleado.Sueldo}",
                    InfoValorGananciaEmp = $"{e.ValorGanancia}"
                };

                flowLayoutPanelEmp.Controls.Add(empleadosItemControl);
            });
        }


        private void ObtenerDetallesMaterialesItems(Guid idProyecto)
        {
            IDetalleGeneric<DetalleProyectoMaterial> detalleRepo = new DetalleMaterialBL();
            List<DetalleProyectoMaterial> detalleMateriales = detalleRepo.GetAll(idProyecto);

            flowLayoutPanelMat.Controls.Clear();

            detalleMateriales.ForEach(e =>
            {
                DetalleMaterialItemControl materialesItemControl = new DetalleMaterialItemControl
                {
                    InfoDescripcionArticulo = $"{e.Material.DescripcionArticulo}",
                    InfoTipoArticulo = $"{e.Material.TipoMaterial}",
                    InfoTipoUnidad = $"{e.Material.TipoUnidad}",
                    InfoCantidad = $"{e.Cantidad}",
                    InfoCosto = $"{e.Material.CostoPorUnidad}",
                    InfoValorGananciaMat = $"{e.ValorGanancia}"
                };

                flowLayoutPanelMat.Controls.Add(materialesItemControl);
            });
        }

        private void ObtenerMaterialFaltanteItems(Guid idProyecto)
        {
            IDetalleGeneric<MaterialFaltante> detalleRepo = new MaterialFaltanteBL();
            List<MaterialFaltante> detalleMaterialesFaltantes = detalleRepo.GetAll(idProyecto);
            flowLayoutPanelMatFal.Controls.Clear();
            detalleMaterialesFaltantes.ForEach(e =>
            {
                MaterialFaltanteItemControl materialesItemControl = new MaterialFaltanteItemControl
                {
                    DescripcionArticuloFaltante = $"{e.DescripcionArticuloFaltante}",
                    TipoArticuloFaltante = $"{e.TipoMaterialFaltante}",
                    TipoUnidadArticuloFaltante = $"{e.TipoUnidadMaterialFaltante}",
                    CantidadArticuloFaltante = $"{e.CantidadFaltante}",
                };
                flowLayoutPanelMatFal.Controls.Add(materialesItemControl);
            });
        }







        private void RecalcularYActualizarTotales()
        {
            try
            {
                var informe = new InformeMontoBL().Recalcular(_proyecto.IdProyecto);
                float utilidad = informe.MontoTotal * 0.20f;

                TotalEmpleados  = $"${informe.TotalEmpleados:N0}";
                TotalMateriales = $"${informe.TotalMateriales:N0}";
                UtilidadEmpresa = $"${utilidad:N0}";
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DetalleProyectoControl_Load(object sender, EventArgs e)
        {
            if (_proyecto != null)
            {
                ObtenerDetallesEmpleadosItems(_proyecto.IdProyecto);
                ObtenerDetallesMaterialesItems(_proyecto.IdProyecto);
                ObtenerMaterialFaltanteItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();

                if (_proyecto.MaterialFaltantes != null && flowLayoutPanelMatFal.Controls.Count > 0)
                {
                    flowLayoutPanelMat.Size = new Size(flowLayoutPanelMat.Size.Width, flowLayoutPanelMat.Size.Height - 70);
                }
            }
        }

        private void linkLabelAgregarMat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Permiso primero
            if (!SessionContext.Has("AGREGAR_MATERIALES")) // ajustá key real
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EventHandler handler = (_, __) =>
            {
                ObtenerDetallesMaterialesItems(_proyecto.IdProyecto);
                ObtenerMaterialFaltanteItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();

                if (flowLayoutPanelMatFal.Controls.Count > 0)
                    flowLayoutPanelMat.Height = 70;
            };

            using (var frm = new AgregarMaterialProyectoForm(_proyecto.IdProyecto))
            {
                frm.StartPosition = FormStartPosition.CenterParent;

                frm.MaterialesProyectoActualizados += handler;
                frm.ShowDialog(this);
                frm.MaterialesProyectoActualizados -= handler;
            }



        }

        private void linkLabelAgregarEmp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 1) Permiso
            if (!SessionContext.Has("CARGAR_EMPLEADOS")) // ajustá la key real
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2) Abrir modal y refrescar si corresponde
            using (var frm = new AgregarEmpleadoProyectoForm(_proyecto.IdProyecto))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
                ObtenerDetallesEmpleadosItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();
            }
        }




        private void buttonModificar_Click(object sender, EventArgs e)
        {
            using (var frm = new EditProyectoForm(_proyecto))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    // Refrescar labels con los nuevos datos
                    DescripcionProyecto = _proyecto.Descripcion;
                    UbicacionProyecto = _proyecto.Ubicacion;
                    FechaInicio = _proyecto.FechaInicio.ToString("dd/MM/yyyy");
                    EstadoProyecto = _proyecto.Estado.ToString();
                }
            }
        }

        private void buttonGenerarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                var bl = new InformeDeCompraBL();
                var idInforme = bl.GenerarDesdeFaltantes(_proyecto.IdProyecto, unicoPorDia: true);

                MessageBox.Show($"Informe generado.\nID: {idInforme}", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
