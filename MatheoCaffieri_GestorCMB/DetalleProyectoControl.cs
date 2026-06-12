using BL;
using DomainModel;
using DomainModel.Exceptions;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services;
using Services.Language;
using Services.Logs;
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
using System.Drawing.Drawing2D;

namespace MatheoCaffieri_GestorCMB
{
    public partial class DetalleProyectoControl : UserControl
    {
        private MainForm mainForm;
        private Proyecto _proyecto;
        private Label _labelFechaCierreStatic;
        private Label _labelFechaCierre;
        private Panel _headerPanel;
        private LinkLabel _linkVerAnalisis;

        public DetalleProyectoControl(MainForm mainForm, Proyecto proyecto)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.mainForm = mainForm;
            _proyecto = proyecto;

            NumProyecto = LanguageService.Current?.T("txt_proyecto_num_default") ?? "Proyecto #";
            DescripcionProyecto = proyecto.Descripcion;
            NombreCliente = proyecto.Cliente?.NombreContacto
                ?? (LanguageService.Current?.T("txt_cliente_desconocido") ?? "Cliente desconocido");
            FechaInicio = proyecto.FechaInicio.ToString("dd/MM/yyyy")
                ?? (LanguageService.Current?.T("txt_sin_fecha") ?? "Sin fecha");
            AplicarEstado(proyecto.Estado);
            UbicacionProyecto = proyecto.Ubicacion
                ?? (LanguageService.Current?.T("txt_ubicacion_desconocida") ?? "Ubicación desconocida");

            buttonModificar.Click += buttonModificar_Click;

            buttonGenerarInforme.FlatStyle = FlatStyle.Flat;
            buttonGenerarInforme.BackColor = Color.DodgerBlue;
            buttonGenerarInforme.ForeColor = Color.White;
            buttonGenerarInforme.FlatAppearance.BorderSize = 0;
            buttonGenerarInforme.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 118, 210);
            buttonGenerarInforme.Cursor = Cursors.Hand;

            InicializarHeader(proyecto);

            this.Resize += (_, __) =>
            {
                AjustarHeaderEstadoYFecha();
                ActualizarHeaderPanelBounds();
                ResetearScrolls();
            };
            AjustarHeaderEstadoYFecha();
            ActualizarHeaderPanelBounds();
        }

        private void InicializarHeader(Proyecto proyecto)
        {
            labelNumProyecto.Visible = false;

            labelDescProyecto.Location = new Point(25, 20);
            labelDescProyecto.ForeColor = Color.FromArgb(25, 25, 25);

            _labelFechaCierreStatic = new Label
            {
                Text = LanguageService.Current?.T("lbl_fecha_de_cierre") ?? "Fecha de cierre:",
                Font = new Font("Microsoft YaHei UI", 10f),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true
            };

            _labelFechaCierre = new Label
            {
                Text = proyecto.FechaFin.ToString("dd/MM/yyyy"),
                Font = new Font("Microsoft YaHei UI", 10f),
                ForeColor = Color.FromArgb(25, 25, 25),
                AutoSize = true
            };

            AjustarHeaderEstadoYFecha();
            EstablecerHeaderPanel();
            ActualizarHeaderPanelBounds();
        }

        private void AjustarHeaderEstadoYFecha()
        {
            if (_labelFechaCierreStatic == null || _labelFechaCierre == null)
                return;

            // Fechas (cierre debajo de inicio)
            _labelFechaCierreStatic.Location = new Point(label3.Left, label3.Bottom + 8);
            _labelFechaCierre.Location = new Point(labelFechaInicio.Left, _labelFechaCierreStatic.Top);

            // Estado arriba de Ubicación (misma columna izquierda)
            int estadoY = Math.Max(0, label1.Top - 26); // label1 = "Ubicación:"
            labelE.Location = new Point(label1.Left, estadoY);
            labelEstado.Location = new Point(labelE.Right + 6, estadoY);
        }

        public Proyecto ProyectoData
        {
            get => _proyecto;
            set => _proyecto = value;
        }

        public string NumProyecto
        {
            get => labelNumProyecto.Text;
            set => labelNumProyecto.Text = value ?? (LanguageService.Current?.T("txt_proyecto_na") ?? "Proyecto #N/A");
        }

        public string DescripcionProyecto
        {
            get => labelDescProyecto.Text;
            set => labelDescProyecto.Text = value ?? (LanguageService.Current?.T("txt_sin_descripcion") ?? "Sin descripción");
        }

        public string NombreCliente
        {
            get => labelNomCliente.Text;
            set => labelNomCliente.Text = value ?? (LanguageService.Current?.T("txt_desconocido") ?? "Desconocido");
        }

        public string FechaInicio
        {
            get => labelFechaInicio.Text;
            set => labelFechaInicio.Text = value ?? (LanguageService.Current?.T("txt_fecha_no_disponible") ?? "Fecha no disponible");
        }

        public string EstadoProyecto
        {
            get => labelEstado.Text;
            set => labelEstado.Text = value ?? (LanguageService.Current?.T("txt_estado_desconocido") ?? "Estado desconocido");
        }

        public string UbicacionProyecto
        {
            get => labelUbiProyecto.Text;
            set => labelUbiProyecto.Text = value ?? (LanguageService.Current?.T("txt_ubicacion_no_especificada") ?? "Ubicación no especificada");
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

        private void AplicarEstado(EnumEstado estado)
        {
            EstadoProyecto = FormatEstado(estado);
            switch (estado)
            {
                case EnumEstado.EnProceso: labelEstado.ForeColor = Color.CornflowerBlue; break;
                case EnumEstado.Suspendido: labelEstado.ForeColor = Color.DarkOrange; break;
                case EnumEstado.Finalizado: labelEstado.ForeColor = Color.MediumSeaGreen; break;
                default: labelEstado.ForeColor = SystemColors.ControlText; break;
            }

            AjustarHeaderEstadoYFecha();
        }

        private static string FormatEstado(EnumEstado estado)
        {
            switch (estado)
            {
                case EnumEstado.EnProceso:  return LanguageService.Current?.T("val_estado_en_proceso") ?? "En proceso";
                case EnumEstado.Suspendido: return LanguageService.Current?.T("val_estado_suspendido") ?? "Suspendido";
                case EnumEstado.Finalizado: return LanguageService.Current?.T("val_estado_finalizado") ?? "Finalizado";
                default:                    return estado.ToString();
            }
        }

        private void ObtenerDetallesEmpleadosItems(Guid idProyecto)
        {
            IDetalleGeneric<DetalleProyectoEmpleado> detalleRepo = new DetalleEmpleadoBL();
            List<DetalleProyectoEmpleado> detalleEmpleados = detalleRepo.GetAll(idProyecto);

            flowLayoutPanelEmp.SuspendLayout();
            flowLayoutPanelEmp.Controls.Clear();
            flowLayoutPanelEmp.Controls.Add(MakeSectionTitle(LanguageService.Current?.T("hdr_empleados") ?? "Empleados"));
            flowLayoutPanelEmp.Controls.Add(new DetalleEmpleadoHeaderControl());
            flowLayoutPanelEmp.Controls.Add(MakeSeparator());

            detalleEmpleados.ForEach(e =>
            {
                var item = new DetalleEmpleadoItemControl
                {
                    IdEmpleado           = e.Empleado.IdEmpleado,
                    InfoNombreApellido   = $"{e.Empleado.Nombre} {e.Empleado.Apellido}",
                    InfoNroDocumento     = $"{e.Empleado.NroDocumento}",
                    InfoSueldo           = $"${e.Empleado.Sueldo:N0}",
                    InfoValorGananciaEmp = $"${e.ValorGanancia:N0}"
                };
                item.DoubleClick += (s, ev) => EliminarEmpleadoDelProyecto(
                    e.Empleado.IdEmpleado,
                    $"{e.Empleado.Nombre} {e.Empleado.Apellido}");
                flowLayoutPanelEmp.Controls.Add(item);
            });
            flowLayoutPanelEmp.ResumeLayout(true);
        }


        private void ObtenerDetallesMaterialesItems(Guid idProyecto)
        {
            IDetalleGeneric<DetalleProyectoMaterial> detalleRepo = new DetalleMaterialBL();
            List<DetalleProyectoMaterial> detalleMateriales = detalleRepo.GetAll(idProyecto);

            flowLayoutPanelMat.SuspendLayout();
            flowLayoutPanelMat.Controls.Clear();
            flowLayoutPanelMat.Controls.Add(MakeSectionTitle(LanguageService.Current?.T("hdr_materiales") ?? "Materiales"));
            flowLayoutPanelMat.Controls.Add(new DetalleMaterialHeaderControl());
            flowLayoutPanelMat.Controls.Add(MakeSeparator());

            detalleMateriales.ForEach(e =>
            {
                var item = new DetalleMaterialItemControl
                {
                    IdMaterial              = e.Material.IdMaterial,
                    InfoDescripcionArticulo = $"{e.Material.DescripcionArticulo}",
                    InfoTipoArticulo        = $"{e.Material.TipoMaterial}",
                    InfoTipoUnidad          = $"{e.Material.TipoUnidad}",
                    InfoCantidad            = $"{e.Cantidad}",
                    InfoCosto               = $"${e.Material.CostoPorUnidad:N0}",
                    InfoValorGananciaMat    = $"${e.ValorGanancia:N0}"
                };
                item.DoubleClick += (s, ev) => EliminarMaterialDelProyecto(
                    e.Material.IdMaterial,
                    e.Material.DescripcionArticulo);
                flowLayoutPanelMat.Controls.Add(item);
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
                flowLayoutPanelMat.Controls.Add(new MaterialFaltanteHeaderControl());
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
                labelUtilidadEmpresa.ForeColor = Color.MediumSeaGreen;
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

            // Freeze flow panels FIRST — before any size/width changes — so no intermediate
            // layout runs while items are still at stale widths (which can corrupt AutoScrollPosition).
            flowLayoutPanelEmp.SuspendLayout();
            flowLayoutPanelMat.SuspendLayout();
            flowLayoutPanelEmp.AutoScrollPosition = Point.Empty;
            flowLayoutPanelMat.AutoScrollPosition = Point.Empty;

            panel1.SuspendLayout();

            const int hMargin = 6;
            const int vMargin = 4;
            const int minH = 37;

            int sepLeft = (panel1.ClientSize.Width - panel2.Width) / 2;
            int sepRight = sepLeft + panel2.Width;
            int empX = flowLayoutPanelEmp.Left;
            int empWidth = Math.Max(80, sepLeft - empX - hMargin);
            int matX = sepRight + hMargin;
            int matWidth = Math.Max(80, panel1.ClientSize.Width - matX - hMargin);

            flowLayoutPanelEmp.Width = empWidth;
            flowLayoutPanelMat.Left = matX;
            flowLayoutPanelMat.Width = matWidth;

            int bottomBound = panel3.Top - vMargin;
            int linkH = linkLabelAgregarEmp.Height + vMargin * 2;
            int empTop = flowLayoutPanelEmp.Top;
            int matTop = flowLayoutPanelMat.Top;

            int empContentH = Math.Max(minH, GetFlowHeight(flowLayoutPanelEmp));
            int empAvailH = Math.Max(minH, bottomBound - empTop - linkH);
            flowLayoutPanelEmp.Height = Math.Min(empContentH, empAvailH);
            linkLabelAgregarEmp.Left = empX;
            linkLabelAgregarEmp.Top = Math.Min(
                flowLayoutPanelEmp.Bottom + vMargin,
                bottomBound - linkLabelAgregarEmp.Height);

            int matContentH = Math.Max(minH, GetFlowHeight(flowLayoutPanelMat));
            int matAvailH = Math.Max(minH, bottomBound - matTop - linkH);
            flowLayoutPanelMat.Height = Math.Min(matContentH, matAvailH);
            linkLabelAgregarMat.Left = matX;
            linkLabelAgregarMat.Top = Math.Min(
                flowLayoutPanelMat.Bottom + vMargin,
                bottomBound - linkLabelAgregarMat.Height);

            ResizeFlowChildren(flowLayoutPanelEmp, empWidth);
            ResizeFlowChildren(flowLayoutPanelMat, matWidth);

            // Resume with one clean layout pass now that all widths are correct.
            flowLayoutPanelEmp.ResumeLayout(true);
            flowLayoutPanelMat.ResumeLayout(true);
            flowLayoutPanelEmp.AutoScrollPosition = Point.Empty;
            flowLayoutPanelMat.AutoScrollPosition = Point.Empty;

            panel1.ResumeLayout(true);

            ResetearScrolls();
        }

        private int GetFlowHeight(System.Windows.Forms.FlowLayoutPanel panel)
        {
            int h = 0;
            foreach (System.Windows.Forms.Control c in panel.Controls)
                h += c.Height + c.Margin.Top + c.Margin.Bottom;
            return h;
        }

        private void ResizeFlowChildren(FlowLayoutPanel panel, int panelWidth)
        {
            if (panel == null) return;

            // Use the explicitly passed panelWidth (not panel.ClientSize.Width) to avoid
            // stale scrollbar state from reducing ClientSize incorrectly before layout runs.
            bool willOverflow = panel.AutoScroll && GetFlowHeight(panel) > panel.Height;
            int w = panelWidth - panel.Padding.Left - panel.Padding.Right;
            if (willOverflow)
                w -= SystemInformation.VerticalScrollBarWidth;
            w = Math.Max(1, w);

            foreach (Control c in panel.Controls)
            {
                c.Margin = new Padding(0, c.Margin.Top, 0, c.Margin.Bottom);
                c.Width = w;
            }
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
                Text      = LanguageService.Current?.T("hdr_faltante") ?? "Faltante",
                Font      = new System.Drawing.Font("Microsoft YaHei UI", 8F),
                ForeColor = System.Drawing.Color.DarkOrange,
                AutoSize  = false,
                Size      = new System.Drawing.Size(100, 18),
                Margin    = new System.Windows.Forms.Padding(0, 6, 0, 2),
                BackColor = System.Drawing.Color.Transparent,
            };
        }

        private void DetalleProyectoControl_Load(object sender, EventArgs e)
        {
            panel1.SizeChanged += (_, __) => BeginInvoke(new Action(UpdateLayout));

            this.ParentChanged += (_, __) =>
            {
                var form = this.FindForm();
                if (form != null)
                    form.ResizeEnd += (s2, e2) => { if (IsHandleCreated) BeginInvoke(new Action(UpdateLayout)); };
            };

            if (_proyecto != null)
            {
                ObtenerDetallesEmpleadosItems(_proyecto.IdProyecto);
                ObtenerDetallesMaterialesItems(_proyecto.IdProyecto);
                ObtenerMaterialFaltanteItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();
            }

            UpdateLayout();
            flowLayoutPanelEmp.AutoScrollPosition = Point.Empty;
            flowLayoutPanelMat.AutoScrollPosition = Point.Empty;
            flowLayoutPanelMatFal.AutoScrollPosition = Point.Empty;

            InicializarLinkAnalisis();
            ActualizarHeaderPanelBounds();
        }

        private void InicializarLinkAnalisis()
        {
            _linkVerAnalisis = new LinkLabel
            {
                Text      = LanguageService.Current?.T("lnk_ver_analisis") ?? "Ver análisis",
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9f),
                Anchor    = AnchorStyles.Bottom | AnchorStyles.Right,
                Cursor    = Cursors.Hand
            };
            _linkVerAnalisis.LinkClicked += (_, __) =>
            {
                using (var frm = new AnalisisProyectoForm(_proyecto.IdProyecto, _proyecto.Descripcion))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            };

            panel1.Controls.Add(_linkVerAnalisis);

            // Posicionar alineado verticalmente con labelTotalMateriales,
            // pegado al margen derecho del panel.
            _linkVerAnalisis.Location = new Point(
                panel1.ClientSize.Width - _linkVerAnalisis.Width - 16,
                labelTotalMateriales.Top
            );
        }

        private void ConfigurarPanelDetalle()
        {
            panel1.BackColor = Color.FromArgb(241, 243, 247);
            panel1.Padding = new Padding(12);

            panel1.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = MakeCardPath(new Rectangle(0, 0, panel1.Width - 1, panel1.Height - 1), 12))
                using (var pen = new Pen(Color.FromArgb(220, 223, 230)))
                    e.Graphics.DrawPath(pen, path);
            };

            panel1.Resize += (s, e) =>
            {
                if (panel1.Width > 4 && panel1.Height > 4)
                {
                    using (var path = MakeCardPath(new Rectangle(0, 0, panel1.Width - 1, panel1.Height - 1), 12))
                        panel1.Region = new Region(path);
                }
            };
        }

        private static GraphicsPath MakeCardPath(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Interfaces.LoginInterfaces.TipoPermiso.GESTIONAR_PROYECTOS))
                return;

            using (var frm = new EditProyectoForm(_proyecto))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    DescripcionProyecto = _proyecto.Descripcion;
                    UbicacionProyecto = _proyecto.Ubicacion;
                    FechaInicio = _proyecto.FechaInicio.ToString("dd/MM/yyyy");
                    AplicarEstado(_proyecto.Estado);

                    if (_labelFechaCierre != null)
                        _labelFechaCierre.Text = _proyecto.FechaFin.ToString("dd/MM/yyyy");

                    AjustarHeaderEstadoYFecha();
                }
            }
        }

        private void buttonGenerarInforme_Click(object sender, EventArgs e)
        {
            if (!PermisosUI.Require(Interfaces.LoginInterfaces.TipoPermiso.GESTIONAR_INFORMES_COMPRA))
                return;

            try
            {
                var bl = new InformeDeCompraBL();
                var idInforme = bl.GenerarDesdeFaltantes(_proyecto.IdProyecto, unicoPorDia: true);

                MessageBox.Show(
                    string.Format(LanguageService.Current?.T("msg_informe_generado_fmt") ?? "Informe generado.\nID: {0}", idInforme),
                    LanguageService.Current?.T("cap_ok") ?? "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[DetalleProyectoControl] Validación al generar informe: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleProyectoControl] Falla al generar informe (Proy={_proyecto.IdProyecto}).", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstablecerHeaderPanel()
        {
            const int panX = 12, panY = 4;
            int panH     = Math.Max(1, panel1.Top - panY - 4);
            var bgColor  = Color.FromArgb(245, 245, 245);

            _headerPanel = new RoundedPanel
            {
                BackColor = bgColor,
                Location  = new Point(panX, panY),
                Size      = new Size(Math.Max(1, this.Width - panX * 2), panH),
                Anchor    = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            };

            Control[] headerControls =
            {
                labelNumProyecto, labelDescProyecto,
                label2, labelNomCliente,
                label1, labelUbiProyecto,
                buttonModificar, buttonGenerarInforme,
                label3, labelFechaInicio,
                labelE, labelEstado,
                _labelFechaCierreStatic, _labelFechaCierre,
            };

            foreach (var ctrl in headerControls)
            {
                if (ctrl == null) continue;
                if (ctrl is Label lbl)
                    lbl.BackColor = bgColor;
                ctrl.Location = new Point(ctrl.Left - panX, ctrl.Top - panY);
                _headerPanel.Controls.Add(ctrl); // WinForms auto-remueve del padre anterior
            }

            this.Controls.Add(_headerPanel);
        }

        private sealed class RoundedPanel : Panel
        {
            private const int Radius = 10;

            public RoundedPanel()
            {
                SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = BuildPath(ClientRectangle, Radius))
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillPath(brush, path);
            }

            private static System.Drawing.Drawing2D.GraphicsPath BuildPath(Rectangle r, int rad)
            {
                int d = rad * 2;
                var gp = new System.Drawing.Drawing2D.GraphicsPath();
                gp.AddArc(r.X,         r.Y,          d, d, 180, 90);
                gp.AddArc(r.Right - d, r.Y,          d, d, 270, 90);
                gp.AddArc(r.Right - d, r.Bottom - d, d, d,   0, 90);
                gp.AddArc(r.X,         r.Bottom - d, d, d,  90, 90);
                gp.CloseFigure();
                return gp;
            }
        }

        private void linkLabelAgregarMat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!SessionContext.Has("GESTIONAR_MATERIALES"))
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
                flowLayoutPanelMat.AutoScrollPosition = Point.Empty;
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
            if (!SessionContext.Has("GESTIONAR_EMPLEADOS"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private void ActualizarHeaderPanelBounds()
        {
            if (_headerPanel == null)
                return;

            const int panX = 12;
            const int panY = 4;
            int panH = Math.Max(1, panel1.Top - panY - 4);

            _headerPanel.Location = new Point(panX, panY);
            _headerPanel.Size = new Size(Math.Max(1, this.Width - panX * 2), panH);
        }

        private void EliminarEmpleadoDelProyecto(Guid idEmpleado, string nombreCompleto)
        {
            if (!SessionContext.Has("GESTIONAR_EMPLEADOS"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var respuesta = MessageBox.Show(
                string.Format(LanguageService.Current?.T("msg_confirmar_eliminar_empleado_fmt") ?? "¿Querés eliminar a {0} del proyecto?", nombreCompleto),
                LanguageService.Current?.T("cap_eliminar_empleado") ?? "Eliminar empleado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                new ProyectoEmpleadoBL().QuitarEmpleadoDelProyecto(_proyecto.IdProyecto, idEmpleado);
                ObtenerDetallesEmpleadosItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();
                UpdateLayout();
                flowLayoutPanelEmp.AutoScrollPosition = Point.Empty;
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[DetalleProyectoControl] Validación al quitar empleado: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleProyectoControl] Falla al quitar empleado del proyecto (Proy={_proyecto.IdProyecto}, Emp={idEmpleado}).", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarMaterialDelProyecto(Guid idMaterial, string descripcion)
        {
            if (!SessionContext.Has("GESTIONAR_MATERIALES"))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var respuesta = MessageBox.Show(
                string.Format(LanguageService.Current?.T("msg_confirmar_eliminar_material_fmt") ?? "¿Querés eliminar \"{0}\" del proyecto?", descripcion),
                LanguageService.Current?.T("cap_eliminar_material") ?? "Eliminar material",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                new ProyectoMaterialBL().QuitarMaterialDelProyecto(_proyecto.IdProyecto, idMaterial);
                ObtenerDetallesMaterialesItems(_proyecto.IdProyecto);
                ObtenerMaterialFaltanteItems(_proyecto.IdProyecto);
                RecalcularYActualizarTotales();
                UpdateLayout();
                flowLayoutPanelMat.AutoScrollPosition = Point.Empty;
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[DetalleProyectoControl] Validación al quitar material: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[DetalleProyectoControl] Falla al quitar material del proyecto (Proy={_proyecto.IdProyecto}, Mat={idMaterial}).", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetearScrolls()
        {
            if (!IsHandleCreated) return;

            BeginInvoke(new Action(() =>
            {
                ResetFlow(flowLayoutPanelEmp);
                ResetFlow(flowLayoutPanelMat);
                ResetFlow(flowLayoutPanelMatFal);
            }));
        }

        private static void ResetFlow(FlowLayoutPanel panel)
        {
            if (panel == null) return;

            panel.AutoScrollPosition = Point.Empty;

            if (panel.Controls.Count > 0)
                panel.ScrollControlIntoView(panel.Controls[0]);
        }
    }
}
