using BL;
using DomainModel;
using DomainModel.Exceptions;
using BL.BL_Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
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

namespace MatheoCaffieri_GestorCMB
{
    public partial class AgregarMaterialProyectoForm : Form
    {
        private readonly Guid _idProyecto;

        private const string REQUIRED = "GESTIONAR_MATERIALES";



        public event EventHandler MaterialesProyectoActualizados;


        private System.Drawing.Point _mouseLocation;

        public AgregarMaterialProyectoForm(Guid idProyecto)
        {
            InitializeComponent();
            _idProyecto = idProyecto;

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


            this.Load += AgregarMaterialProyectoForm_Load;
        }
        private void ConfigurarLayout()
        {
            gestionarMaterialesDetalleLayoutPanel.AutoScroll = true;
            gestionarMaterialesDetalleLayoutPanel.WrapContents = false;
            gestionarMaterialesDetalleLayoutPanel.FlowDirection = FlowDirection.TopDown;
        }

        private void CargarInventarioItems()
        {
            gestionarMaterialesDetalleLayoutPanel.Controls.Clear();

            var invRepo = (IInventarioBL)new InventarioBL();
            List<Inventario> inventario = invRepo.GetAll();

            foreach (var inv in inventario)
            {
                var item = new AddMaterialProyectoItemControl
                {
                    DescripcionArticuloInventario = inv.Material?.DescripcionArticulo
                        ?? (LanguageService.Current?.T("txt_sin_descripcion") ?? "Sin descripción"),
                    TipoArticuloInventario = inv.Material?.TipoMaterial
                        ?? (LanguageService.Current?.T("txt_sin_tipo") ?? "Sin tipo"),
                    InfoGeneralArticuloInventario = $"| {inv.Material?.Proveedor?.Descripcion ?? (LanguageService.Current?.T("txt_sin_proveedor") ?? "Sin proveedor")} | ${inv.Material?.CostoPorUnidad ?? 0}"
                };

                item.Bind(inv.IdMaterialInventario, inv.IdMaterial, inv.Cantidad);

                item.AgregarClick += Item_AgregarClick;

                gestionarMaterialesDetalleLayoutPanel.Controls.Add(item);
            }
        }

        private void AgregarMaterialProyectoForm_Load(object sender, EventArgs e)
        {
            if (_idProyecto == Guid.Empty)
            {
                MessageBox.Show(LanguageService.Current?.T("err_proyecto_no_recibido") ?? "No se recibió el proyecto.");
                Close();
                return;
            }

            ConfigurarLayout();
            CargarInventarioItems();
        }

        private void Item_AgregarClick(object sender, AgregarMaterialEventArgs e)
        {
            try
            {
                var material = ((IMaterialBL)new MaterialBL()).GetById(e.IdMaterial);
                double valorGanancia = material != null
                    ? (double)((decimal)material.CostoPorUnidad * ParametrosContext.MargenMateriales)
                    : 0;

                var svc = new ProyectoMaterialBL();
                var r = svc.AgregarMaterialDetalleProyectoDesdeInventario(
                    _idProyecto,
                    e.IdMaterial,
                    e.Cantidad,
                    valorGanancia
                );

                CargarInventarioItems();

                // Avisar al padre (DetalleProyectoControl) para que refresque materiales + faltantes.
                MaterialesProyectoActualizados?.Invoke(this, EventArgs.Empty);

                if (r.Faltante > 0)
                {
                    MessageBox.Show(
                        string.Format(
                            LanguageService.Current?.T("msg_materiales_agregados_con_faltantes_fmt") ?? "Se agregaron {0} al proyecto y {1} se cargaron como material faltante.",
                            r.Asignado, r.Faltante),
                        LanguageService.Current?.T("cap_materiales_faltantes") ?? "Materiales faltantes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        string.Format(
                            LanguageService.Current?.T("msg_materiales_agregados_fmt") ?? "Se agregaron {0} al proyecto.",
                            r.Asignado),
                        LanguageService.Current?.T("cap_ok") ?? "OK",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[AgregarMaterialProyectoForm] Validación al asignar material: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("[AgregarMaterialProyectoForm] Falla al asignar material al proyecto.", ex);
                var msg = LanguageService.Current?.T("err_db_generic") ?? "Error al acceder a la base de datos.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExitAP_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
