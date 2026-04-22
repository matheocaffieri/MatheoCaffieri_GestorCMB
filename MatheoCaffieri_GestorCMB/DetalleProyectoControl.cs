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
            this.DoubleBuffered = true;
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

            flowLayoutPanelEmp.SuspendLayout();
            flowLayoutPanelEmp.Controls.Clear();
            flowLayoutPanelEmp.Controls.Add(MakeSectionTitle("Empleados"));
            flowLayoutPanelEmp.Controls.Add(MakeColumnHeader(
                "Nombre               |  Documento  |  Sueldo    |  Ganancia"));
            flowLayoutPanelEmp.Controls.Add(MakeSeparator());

            detalleEmpleados.ForEach(e =>
            {
                flowLayoutPanelEmp.Controls.Add(new DetalleEmpleadoItemControl
                {
                    InfoNombreApellido   = $"{e.Empleado.Nombre} {e.Empleado.Apellido}",
                    InfoNroDocumento     = $"{e.Empleado.NroDocumento}",
                    InfoSueldo           = $"${e.Empleado.Sueldo:N0}",
                    InfoValorGananciaEmp = $"${e.ValorGanancia:N0}"
                });
            });
            flowLayoutPanelEmp.ResumeLayout(true);
        }


        private void ObtenerDetallesMaterialesItems(Guid idProyecto)
        {
            IDetalleGeneric<DetalleProyectoMaterial> detalleRepo = new DetalleMaterialBL();
            List<DetalleProyectoMaterial> detalleMateriales = detalleRepo.GetAll(idProyecto);

            flowLayoutPanelMat.SuspendLayout();
            flowLayoutPanelMat.Controls.Clear();
            flowLayoutPanelMat.Controls.Add(MakeSectionTitle("Materiales"));
            flowLayoutPanelMat.Controls.Add(MakeColumnHeader(
                "Descripción  |  Categoría  |  Unidad  |  Cant.  |  Precio/U  |  Ganancia/U"));
            flowLayoutPanelMat.Controls.Add(MakeSeparator());

            detalleMateriales.ForEach(e =>
            {
                flowLayoutPanelMat.Controls.Add(new DetalleMaterialItemControl
                {
                    InfoDescripcionArticulo = $"{e.Material.DescripcionArticulo}",
                    InfoTipoArticulo        = $"{e.Material.TipoMaterial}",
                    InfoTipoUnidad          = $"{e.Material.TipoUnidad}",
                    InfoCantidad            = $"{e.Cantidad}",
                    InfoCosto               = $"${e.Material.CostoPorUnidad:N0}",
                    InfoValorGananciaMat    = $"${e.ValorGanancia:N0}"
                });
            });
            flowLayoutPanelMat.ResumeLayout(true);
        }

        private void ObtenerMaterialFaltanteItems(Guid idProyecto)
        {
            IDetalleGeneric<MaterialFaltante> detalleRepo = new MaterialFaltanteBL();
            List<MaterialFaltante> detalleMaterialesFaltantes = detalleRepo.GetAll(idProyecto);

            label7.Visible = false;
            flowLayoutPanelMatFal.Controls.Clear();
            flowLayoutPanelMatFal.Visible = false;

            if (detalleMaterialesFaltantes.Count > 0)
            {
                flowLayoutPanelMat.SuspendLayout();
                flowLayoutPanelMat.Controls.Add(MakeFaltanteDivider());
                flowLayoutPanelMat.Controls.Add(MakeColumnHeader(
                    "Descripción           |  Categoría           |  Unidad  |  Cant. faltante",
                    compact: true));
                flowLayoutPanelMat.Controls.Add(MakeSeparator());

                detalleMaterialesFaltantes.ForEach(e =>
                {
                    var item = new MaterialFaltanteItemControl
                    {
                        DescripcionArticuloFaltante = $"{e.DescripcionArticuloFaltante}",
                        TipoArticuloFaltante        = $"{e.TipoMaterialFaltante}",
                        TipoUnidadArticuloFaltante  = $"{e.TipoUnidadMaterialFaltante}",
                        CantidadArticuloFaltante    = $"{e.CantidadFaltante}",
                    };
                    item.Height = 28;
                    item.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
                    flowLayoutPanelMat.Controls.Add(item);
                });
                flowLayoutPanelMat.ResumeLayout(true);
            }
        }







        private void RecalcularYActualizarTotales()
        {
            try
            {
                var informe = new InformeMontoBL().Recalcular(_proyecto.IdProyecto);
                float utilidad = informe.MontoTotal * (1f + (float)ParametrosContext.UtilidadEmpresa);

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

        private void UpdateLayout()
        {
            if (panel1.ClientSize.Width == 0 || panel1.ClientSize.Height == 0) return;
            panel1.SuspendLayout();

            const int hMargin = 6;
            const int vMargin = 4;
            const int minH    = 37;

            // ── Horizontal ──────────────────────────────────────────────
            // panel2 sin anchor Left/Right → centrado automáticamente
            int sepLeft  = (panel1.ClientSize.Width - panel2.Width) / 2;
            int sepRight = sepLeft + panel2.Width;
            int empX     = flowLayoutPanelEmp.Left;
            int empWidth = Math.Max(80, sepLeft - empX - hMargin);
            int matX     = sepRight + hMargin;
            int matWidth = Math.Max(80, panel1.ClientSize.Width - matX - hMargin);

            flowLayoutPanelEmp.Width = empWidth;
            flowLayoutPanelMat.Left  = matX;
            flowLayoutPanelMat.Width = matWidth;

            // ── Vertical ────────────────────────────────────────────────
            // panel3 (Anchor=Bottom) da el límite inferior duro
            int bottomBound = panel3.Top - vMargin;
            int linkH       = linkLabelAgregarEmp.Height + vMargin * 2;
            int empTop      = flowLayoutPanelEmp.Top;
            int matTop      = flowLayoutPanelMat.Top;

            // Columna empleados
            int empContentH = Math.Max(minH, GetFlowHeight(flowLayoutPanelEmp));
            int empAvailH   = Math.Max(minH, bottomBound - empTop - linkH);
            flowLayoutPanelEmp.Height  = Math.Min(empContentH, empAvailH);
            linkLabelAgregarEmp.Left   = empX;
            linkLabelAgregarEmp.Top    = Math.Min(
                flowLayoutPanelEmp.Bottom + vMargin,
                bottomBound - linkLabelAgregarEmp.Height);

            // Columna derecha: materiales + faltantes unificados en flowLayoutPanelMat
            int matContentH = Math.Max(minH, GetFlowHeight(flowLayoutPanelMat));
            int matAvailH   = Math.Max(minH, bottomBound - matTop - linkH);
            flowLayoutPanelMat.Height   = Math.Min(matContentH, matAvailH);
            linkLabelAgregarMat.Left    = matX;
            linkLabelAgregarMat.Top     = Math.Min(
                flowLayoutPanelMat.Bottom + vMargin,
                bottomBound - linkLabelAgregarMat.Height);

            // ── Hijos ───────────────────────────────────────────────────
            ResizeFlowChildren(flowLayoutPanelEmp, empWidth);
            ResizeFlowChildren(flowLayoutPanelMat, matWidth);
            panel1.ResumeLayout(true);
        }

        private int GetFlowHeight(System.Windows.Forms.FlowLayoutPanel panel)
        {
            int h = 0;
            foreach (System.Windows.Forms.Control c in panel.Controls)
                h += c.Height + c.Margin.Top + c.Margin.Bottom;
            return h;
        }

        private void ResizeFlowChildren(System.Windows.Forms.FlowLayoutPanel panel, int targetWidth)
        {
            // Si el contenido va a desbordar verticalmente, el scrollbar ocupa ~17px de ancho.
            // Reducimos el ancho de los hijos para que no aparezca scrollbar horizontal.
            bool willOverflow = panel.AutoScroll && GetFlowHeight(panel) > panel.Height;
            int w = willOverflow
                ? Math.Max(1, targetWidth - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth)
                : targetWidth;
            foreach (System.Windows.Forms.Control c in panel.Controls)
                c.Width = w;
        }

        private Label MakeSectionTitle(string text)
        {
            return new Label
            {
                Text      = text,
                Font      = new System.Drawing.Font("Microsoft YaHei UI", 13F, System.Drawing.FontStyle.Regular),
                ForeColor = System.Drawing.Color.FromArgb(25, 25, 25),
                AutoSize  = false,
                Size      = new System.Drawing.Size(100, 37),
                Margin    = new System.Windows.Forms.Padding(0, 2, 0, 4),
                BackColor = System.Drawing.Color.Transparent,
            };
        }

        private Label MakeColumnHeader(string text, bool compact = false)
        {
            return new Label
            {
                Text      = text,
                Font      = new System.Drawing.Font("Microsoft YaHei UI", 7F),
                ForeColor = System.Drawing.Color.Gray,
                AutoSize  = false,
                Size      = new System.Drawing.Size(100, compact ? 13 : 16),
                Margin    = compact
                    ? new System.Windows.Forms.Padding(0, 1, 0, 1)
                    : new System.Windows.Forms.Padding(0, 2, 0, 2),
                BackColor = System.Drawing.Color.Transparent,
            };
        }

        private Label MakeSeparator()
        {
            return new Label
            {
                Text      = string.Empty,
                AutoSize  = false,
                Size      = new System.Drawing.Size(100, 1),
                BackColor = System.Drawing.Color.FromArgb(210, 210, 210),
                Margin    = new System.Windows.Forms.Padding(0, 1, 0, 3),
            };
        }

        private Label MakeFaltanteDivider()
        {
            return new Label
            {
                Text      = "Faltante",
                Font      = new System.Drawing.Font("Microsoft YaHei UI", 8F),
                ForeColor = System.Drawing.Color.FromArgb(80, 80, 80),
                AutoSize  = false,
                Size      = new System.Drawing.Size(100, 18),
                Margin    = new System.Windows.Forms.Padding(0, 6, 0, 2),
                BackColor = System.Drawing.Color.Transparent,
            };
        }

        private void DetalleProyectoControl_Load(object sender, EventArgs e)
        {
            panel1.SizeChanged += (_, __) => UpdateLayout();

            if (_proyecto != null)
            {
                ObtenerDetallesEmpleadosItems(_proyecto.IdProyecto);
                ObtenerDetallesMaterialesItems(_proyecto.IdProyecto);
                ObtenerMaterialFaltanteItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();
            }

            UpdateLayout();
            // Resetear scroll para que siempre arranquen desde arriba
            flowLayoutPanelEmp.AutoScrollPosition    = Point.Empty;
            flowLayoutPanelMat.AutoScrollPosition    = Point.Empty;
            flowLayoutPanelMatFal.AutoScrollPosition = Point.Empty;
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
                UpdateLayout();
                flowLayoutPanelMat.AutoScrollPosition    = Point.Empty;
                flowLayoutPanelMatFal.AutoScrollPosition = Point.Empty;
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
                UpdateLayout();
                flowLayoutPanelEmp.AutoScrollPosition = Point.Empty;
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
