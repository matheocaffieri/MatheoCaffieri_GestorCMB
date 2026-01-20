using BL;
using DomainModel;
using DomainModel.Interfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
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

        private const string REQUIRED = "AGREGAR_MATERIALES";



        public event EventHandler MaterialesProyectoActualizados;


        public AgregarMaterialProyectoForm(Guid idProyecto)
        {
            InitializeComponent();
            _idProyecto = idProyecto;

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }


            this.Load += AgregarMaterialProyectoForm_Load;
        }
        private void ConfigurarLayout()
        {
            // Si es FlowLayoutPanel
            gestionarMaterialesDetalleLayoutPanel.AutoScroll = true;
            gestionarMaterialesDetalleLayoutPanel.WrapContents = false;
            gestionarMaterialesDetalleLayoutPanel.FlowDirection = FlowDirection.TopDown;
        }

        private void CargarInventarioItems()
        {
            gestionarMaterialesDetalleLayoutPanel.Controls.Clear();

            var invRepo = (IGenericRepository<Inventario>)new InventarioBL();
            List<Inventario> inventario = invRepo.GetAll();

            foreach (var inv in inventario)
            {
                // si querés ocultar stock 0:
                // if (inv.Cantidad <= 0) continue;

                var item = new AddMaterialProyectoItemControl
                {
                    DescripcionArticuloInventario = inv.Material?.DescripcionArticulo ?? "Sin descripción",
                    TipoArticuloInventario = inv.Material?.TipoMaterial ?? "Sin tipo",
                    InfoGeneralArticuloInventario = $"| {inv.Material?.Proveedor?.Descripcion ?? "Sin proveedor"} | ${inv.Material?.CostoPorUnidad ?? 0}"
                };

                // Bind: idInventario + idMaterial + stock
                item.Bind(inv.IdMaterialInventario, inv.IdMaterial, inv.Cantidad);

                item.AgregarClick += Item_AgregarClick;

                gestionarMaterialesDetalleLayoutPanel.Controls.Add(item);
            }
        }

        private void AgregarMaterialProyectoForm_Load(object sender, EventArgs e)
        {
            if (_idProyecto == Guid.Empty)
            {
                MessageBox.Show("No se recibió el proyecto.");
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
                double valorGanancia = 0;

                var svc = new ProyectoMaterialBL();
                var r = svc.AgregarMaterialDetalleProyectoDesdeInventario(
                    _idProyecto,
                    e.IdMaterial,
                    e.Cantidad,
                    valorGanancia
                );

                // refrescar stock del inventario en este form
                CargarInventarioItems();

                // avisar al padre (DetalleProyectoControl) para refrescar materiales + faltantes
                MaterialesProyectoActualizados?.Invoke(this, EventArgs.Empty);

                // opcional: feedback al usuario
                if (r.Faltante > 0)
                {
                    MessageBox.Show(
                        $"Se agregaron {r.Asignado} al proyecto y {r.Faltante} se cargaron como material faltante.",
                        "Materiales faltantes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        $"Se agregaron {r.Asignado} al proyecto.",
                        "OK",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo agregar el material al proyecto.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonExitAP_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
