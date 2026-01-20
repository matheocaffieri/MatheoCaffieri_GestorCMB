using BL;
using DomainModel;
using MatheoCaffieri_GestorCMB.ItemControls;
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
    public partial class InformesDeCompraControl : UserControl
    {
        private MainForm _mainForm;

        private const string REQUIRED = "CONSULTAR_INFORMES_COMPRA";



        public InformesDeCompraControl()
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show("No tenés permisos para acceder a esta pantalla.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                var host = _mainForm ?? (this.FindForm() as MainForm);
                if (host == null)
                {
                    MessageBox.Show("No se encontró el MainForm para navegar.", "Atención",
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
            buttonSearchClientes.Click += (s, ev) => CargarInformesItems(textBox1.Text);
            textBox1.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    ev.SuppressKeyPress = true;
                    CargarInformesItems(textBox1.Text);
                }
            };

            CargarInformesItems();
        }

        private void CargarInformesItems(string filtro = null)
        {
            informeLayoutPanel.Controls.Clear();

            var informeBL = new InformeDeCompraBL();
            var informes = informeBL.GetAll() ?? new List<InformeDeCompra>();

            // si no hay informes, no hay nada para mostrar
            if (informes.Count == 0) return;

            var proyectoBL = new ProyectoBL();
            var detInfBL = new DetalleInformeCompraBL();

            // agrupar por proyecto y tomar el más nuevo
            var porProyecto = informes
                .GroupBy(i => i.IdProyecto)
                .Select(g => g.OrderByDescending(x => x.FechaRealizacion).First())
                .ToList();

            // filtro se aplica sobre el proyecto
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

            int n = 1;
            foreach (var p in proyectos)
            {
                var inf = porProyecto.First(x => x.IdProyecto == p.IdProyecto);

                var item = new InformeCompraItemControl();
                item.BindProyecto(p.IdProyecto, "Proyecto #" + n++, p.Descripcion ?? "Nombre de proyecto");
                item.SetInforme(inf.IdInformeCompra);

                var faltantesDelInforme = detInfBL.GetMaterialesFaltantesDelInforme(inf.IdInformeCompra);
                item.SetFaltantes(faltantesDelInforme);

                item.AgregarCompraClicked += Item_AgregarCompraClicked;
                item.EliminarClicked += Item_EliminarClicked;

                informeLayoutPanel.Controls.Add(item);
            }
        }

        // AGREGAR COMPRA:
        // 1) mover los materiales faltantes al detalle del proyecto
        // 2) borrar detalle informe
        // 3) borrar informe
        // 4) borrar materiales faltantes del proyecto
        private void Item_AgregarCompraClicked(object sender, Guid idProyecto)
        {
            try
            {
                var item = (InformeCompraItemControl)sender;
                var idInforme = item.IdInformeCompra ?? Guid.Empty;

                var bl = new InformeDeCompraBL();
                bl.ConfirmarCompraYAplicar(idProyecto, idInforme);

                MessageBox.Show("Compra aplicada.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInformesItems(textBox1.Text);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("InformesDeCompraControl.AgregarCompra_FAIL", ex);
                MessageBox.Show("No se pudo aplicar la compra.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ELIMINAR:
        // Borrar informe (y su detalle) para rehacerlo después
        private void Item_EliminarClicked(object sender, Guid idProyecto)
        {
            try
            {
                var item = (InformeCompraItemControl)sender;
                var idInforme = item.IdInformeCompra ?? Guid.Empty;

                var bl = new InformeDeCompraBL();
                bl.EliminarInforme(idInforme);

                MessageBox.Show("Informe eliminado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarInformesItems(textBox1.Text);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error("InformesDeCompraControl.EliminarInforme_FAIL", ex);
                MessageBox.Show("No se pudo eliminar el informe.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            var host = _mainForm ?? (this.FindForm() as MainForm);
            if (host == null)
            {
                MessageBox.Show("No se encontró el MainForm para navegar.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            host.addUserControl(new VerInventarioControl());
        }
    }
}
