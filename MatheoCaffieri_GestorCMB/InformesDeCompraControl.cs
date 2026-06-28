using BL;
using DomainModel;
using DomainModel.Exceptions;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.Logs;
using Services.RoleService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public partial class InformesDeCompraControl : UserControl
    {
        private MainForm _mainForm;

        private const string REQUIRED = "VER_INFORMES_COMPRA";



        public InformesDeCompraControl()
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = _mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show(
                        LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                        LanguageService.Current?.T("cap_atencion") ?? "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                host.addUserControl(new HomeControl(_mainForm));
                return;
            }



        }

        public InformesDeCompraControl(MainForm mainForm) : this()
        {
            _mainForm = mainForm;
        }

        private void InformesDeCompraControl_Load(object sender, EventArgs e)
        {
            AplicarEstilo();

            buttonSearchClientes.Click += (s, ev) => CargarInformesItems(textBox1.Text);
            textBox1.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    ev.SuppressKeyPress = true;
                    CargarInformesItems(textBox1.Text);
                }
            };

            informeLayoutPanel.Resize += (_, __) => AjustarAnchoItems();

            this.Resize += (_, __) =>
            {
                AjustarHeaderLayout();
                AplicarRadiusBotones();
                AjustarAnchoItems();
            };

            buttonBack.Resize += (_, __) => AplicarRadioControl(buttonBack, 8);
            buttonSearchClientes.Resize += (_, __) => AplicarRadioControl(buttonSearchClientes, 8);
            buttonHistorial.Resize += (_, __) => AplicarRadioControl(buttonHistorial, 8);

            AjustarHeaderLayout();
            AplicarRadiusBotones();
            CargarInformesItems();
        }

        // ── Visual styling ─────────────────────────────────────────────────────

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private void AplicarEstilo()
        {
            BackColor = Color.FromArgb(245, 246, 250);
            informeLayoutPanel.BackColor = Color.Transparent;
            informeLayoutPanel.Padding = new Padding(16, 8, 16, 8);

            label2.Font = new Font("Microsoft YaHei UI", 16f, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(30, 30, 30);
            label2.AutoSize = true;

            buttonBack.Text = "«";
            buttonBack.Font = new Font("Microsoft YaHei UI", 12f, FontStyle.Bold);
            buttonBack.FlatStyle = FlatStyle.Flat;
            buttonBack.FlatAppearance.BorderSize = 1;
            buttonBack.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 210);
            buttonBack.BackColor = Color.White;
            buttonBack.Cursor = Cursors.Hand;

            textBox1.Font = new Font("Microsoft YaHei UI", 9.5f);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            SendMessage(textBox1.Handle, 0x1501, 1, LanguageService.Current?.T("txt_buscar_placeholder") ?? "Buscar...");

            buttonSearchClientes.Text = LanguageService.Current?.T("btn_buscar_icon") ?? "⌕  Buscar";
            buttonSearchClientes.Font = new Font("Microsoft YaHei UI", 10f, FontStyle.Bold);
            buttonSearchClientes.BackColor = Color.FromArgb(76, 175, 80);
            buttonSearchClientes.ForeColor = Color.White;
            buttonSearchClientes.FlatStyle = FlatStyle.Flat;
            buttonSearchClientes.FlatAppearance.BorderSize = 0;
            buttonSearchClientes.FlatAppearance.MouseOverBackColor = Color.FromArgb(56, 142, 60);
            buttonSearchClientes.Cursor = Cursors.Hand;

            buttonHistorial.Text = LanguageService.Current?.T("btn_historial") ?? "Historial";
            buttonHistorial.Font = new Font("Microsoft YaHei UI", 8.5f, FontStyle.Bold);
            buttonHistorial.BackColor = Color.FromArgb(63, 81, 181);
            buttonHistorial.ForeColor = Color.White;
            buttonHistorial.FlatStyle = FlatStyle.Flat;
            buttonHistorial.FlatAppearance.BorderSize = 0;
            buttonHistorial.FlatAppearance.MouseOverBackColor = Color.FromArgb(48, 63, 159);
            buttonHistorial.Cursor = Cursors.Hand;
        }

        private void AjustarAnchoItems()
        {
            int w = informeLayoutPanel.ClientSize.Width;
            if (w <= 0) return;
            int cols = w >= 1100 ? 2 : 1;
            int itemW = (w / cols) - 10;
            foreach (Control c in informeLayoutPanel.Controls)
                c.Width = itemW;
        }

        private void CargarInformesItems(string filtro = null)
        {
            informeLayoutPanel.Controls.Clear();

            List<InformeDeCompra> informes;
            try
            {
                informes = new InformeDeCompraBL().GetAll() ?? new List<InformeDeCompra>();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[InformesDeCompraControl] Falla al cargar listado de informes.", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (informes.Count == 0) return;

            var proyectoBL = new ProyectoBL();
            var detInfBL = new DetalleInformeCompraBL();

            var porProyecto = informes
                .GroupBy(i => i.IdProyecto)
                .Select(g => g.OrderByDescending(x => x.FechaRealizacion).First())
                .ToList();

            var proyectos = porProyecto
                .Select(x => proyectoBL.GetById(x.IdProyecto))
                .Where(p => p != null)
                .ToList();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.Trim().ToLowerInvariant();
                proyectos = proyectos.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Descripcion) && p.Descripcion.ToLowerInvariant().Contains(f)) ||
                    (p.Cliente != null && !string.IsNullOrWhiteSpace(p.Cliente.RazonSocial) &&
                     p.Cliente.RazonSocial.ToLowerInvariant().Contains(f))
                ).ToList();
            }

            foreach (var p in proyectos)
            {
                var inf = porProyecto.First(x => x.IdProyecto == p.IdProyecto);

                var item = new InformeCompraItemControl();
                item.BindProyecto(p.IdProyecto, inf.FechaRealizacion.ToString("dd/MM/yyyy"), p.Descripcion ?? "Nombre de proyecto");
                item.SetInforme(inf.IdInformeCompra);

                var faltantesDelInforme = detInfBL.GetMaterialesFaltantesDelInforme(inf.IdInformeCompra);
                item.SetFaltantes(faltantesDelInforme);

                item.AgregarCompraClicked += Item_AgregarCompraClicked;
                item.EliminarClicked += Item_EliminarClicked;

                informeLayoutPanel.Controls.Add(item);
            }

            AjustarAnchoItems();
        }

        // AGREGAR COMPRA:
        // 1) mover los materiales faltantes al detalle del proyecto
        // 2) borrar detalle informe
        // 3) borrar informe
        // 4) borrar materiales faltantes del proyecto
        private void Item_AgregarCompraClicked(object sender, Guid idProyecto)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.GESTIONAR_INFORMES_COMPRA))
                return;

            try
            {
                var item = (InformeCompraItemControl)sender;
                var idInforme = item.IdInformeCompra ?? Guid.Empty;

                var bl = new InformeDeCompraBL();
                bl.ConfirmarCompraYAplicar(idProyecto, idInforme);

                MessageBox.Show(
                    LanguageService.Current?.T("msg_compra_aplicada") ?? "Compra aplicada.",
                    LanguageService.Current?.T("cap_ok") ?? "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInformesItems(textBox1.Text);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[InformesDeCompraControl] Validación al confirmar compra: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[InformesDeCompraControl] Falla al confirmar compra desde el listado.", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ELIMINAR:
        // Borrar informe (y su detalle) para rehacerlo después
        private void Item_EliminarClicked(object sender, Guid idProyecto)
        {
            if (!PermisosUI.Require(Services.Login.TipoPermiso.GESTIONAR_INFORMES_COMPRA))
                return;

            try
            {
                var item = (InformeCompraItemControl)sender;
                var idInforme = item.IdInformeCompra ?? Guid.Empty;

                var bl = new InformeDeCompraBL();
                bl.EliminarInforme(idInforme);

                MessageBox.Show(
                    LanguageService.Current?.T("msg_informe_eliminado") ?? "Informe eliminado.",
                    LanguageService.Current?.T("cap_ok") ?? "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInformesItems(textBox1.Text);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[InformesDeCompraControl] Validación al eliminar informe: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[InformesDeCompraControl] Falla al eliminar informe desde el listado.", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                    LanguageService.Current?.T("cap_atencion") ?? "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new VerInventarioControl());
        }

        private void buttonHistorial_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_mainform_no_encontrado") ?? "No se encontró el MainForm para navegar.",
                    LanguageService.Current?.T("cap_atencion") ?? "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new HistorialInformesControl(_mainForm));
        }

        private void AjustarHeaderLayout()
        {
            int left = 16;
            int right = 16;
            int top = 16;
            int gap = 10;

            buttonBack.SetBounds(left, top, 40, 34);

            int tituloX = buttonBack.Right + 10;
            label2.Location = new Point(tituloX, top + 2);

            int buscarW = 102;
            int buscarH = 34;
            int buscarX = ClientSize.Width - right - buscarW;
            buttonSearchClientes.SetBounds(buscarX, top, buscarW, buscarH);

            int txtX = Math.Max(label2.Right + 14, tituloX + 210);
            int txtW = Math.Max(190, buscarX - gap - txtX);
            textBox1.SetBounds(txtX, top, txtW, 34);

            buttonHistorial.SetBounds(tituloX, top + 38, 120, 28);

            int listadoTop = Math.Max(buttonHistorial.Bottom + 12, textBox1.Bottom + 12);
            informeLayoutPanel.Location = new Point(16, listadoTop);
            informeLayoutPanel.Size = new Size(ClientSize.Width - 32, ClientSize.Height - listadoTop - 10);
        }

        private void AplicarRadiusBotones()
        {
            AplicarRadioControl(buttonBack, 8);
            AplicarRadioControl(buttonSearchClientes, 8);
            AplicarRadioControl(buttonHistorial, 8);
        }

        private void AplicarRadioControl(Control control, int radius)
        {
            if (control.Width < 2 || control.Height < 2)
                return;

            using (var path = CrearRutaRectRedondeado(new Rectangle(0, 0, control.Width - 1, control.Height - 1), radius))
                control.Region = new Region(path);
        }

        private static GraphicsPath CrearRutaRectRedondeado(Rectangle r, int radius)
        {
            int d = Math.Max(2, radius * 2);
            var path = new GraphicsPath();

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
