using BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class AddMaterialProyectoItemControl : UserControl
    {

        // === datos clave ===
        public Guid IdMaterial { get; private set; }
        public Guid IdInventario { get; private set; }

        public int StockDisponible { get; private set; }
        public int CantidadSeleccionada { get; private set; } = 0;
        public int CantidadFaltantePreview => Math.Max(0, CantidadSeleccionada - StockDisponible);

        // evento para que el FORM haga la operación real
        public event EventHandler<AgregarMaterialEventArgs> AgregarClick;

        public AddMaterialProyectoItemControl()
        {
            InitializeComponent();
            RefrescarCantidad();
        }

        // Propiedades que ya usabas
        public string DescripcionArticuloInventario
        {
            get => labelInfoDescArt.Text;
            set => labelInfoDescArt.Text = value ?? "Sin descripción";
        }

        public string TipoArticuloInventario
        {
            get => labelInfoTipoMat.Text;
            set => labelInfoTipoMat.Text = value ?? "Sin tipo";
        }

        public string InfoGeneralArticuloInventario
        {
            get => labelInfoGeneralArticulo.Text;
            set => labelInfoGeneralArticulo.Text = value ?? "Sin información general";
        }

        // Este label ahora lo vamos a usar como "cantidad seleccionada"
        public string CantidadArticuloInventario
        {
            get => labelInfoCantidadInventario.Text;
            set => labelInfoCantidadInventario.Text = value ?? "0";
        }

        // NUEVO: setear ids y stock desde el Form
        public void Bind(Guid idInventario, Guid idMaterial, int stockDisponible)
        {
            IdMaterial = idMaterial;
            StockDisponible = Math.Max(0, stockDisponible);

            CantidadSeleccionada = 0;
            labelInfoCantidadInventario.Text = "0";

            // si querés mostrar stock en el label general:
            // labelInfoGeneralArticulo.Text = $"{labelInfoGeneralArticulo.Text} | Stock: {StockDisponible}";
        }

        private void RefrescarCantidad()
        {
            labelInfoCantidadInventario.Text = CantidadSeleccionada.ToString();
        }

       

        private void buttonIncreaseQ_Click(object sender, EventArgs e)
        {
            CantidadSeleccionada++; // sin límite (si te pasás, el excedente va a faltantes)
            labelInfoCantidadInventario.Text = CantidadSeleccionada.ToString();

            // opcional: marcar cuando te pasás
            // if (CantidadFaltantePreview > 0) labelInfoCantidadInventario.ForeColor = Color.OrangeRed;
            // else labelInfoCantidadInventario.ForeColor = SystemColors.ControlText;
        }

        private void buttonDecreaseQ_Click(object sender, EventArgs e)
        {
            if (CantidadSeleccionada <= 0) return;

            CantidadSeleccionada--;
            labelInfoCantidadInventario.Text = CantidadSeleccionada.ToString();

        }


        public void SetInventarioId(Guid idInventario)
        {
            buttonIncreaseQ.Tag = idInventario;
            buttonDecreaseQ.Tag = idInventario;
        }

        private void buttonAgregarMaterial_Click(object sender, EventArgs e)
        {
            if (IdMaterial == Guid.Empty) return;
            if (CantidadSeleccionada <= 0) return;

            AgregarClick?.Invoke(this, new AgregarMaterialEventArgs(IdMaterial, CantidadSeleccionada));

            // opcional: resetear selección después de agregar
            CantidadSeleccionada = 0;
            RefrescarCantidad();
        }
    }

    public sealed class AgregarMaterialEventArgs : EventArgs
    {
        public Guid IdMaterial { get; }
        public int Cantidad { get; }
        public AgregarMaterialEventArgs(Guid idMaterial, int cantidad)
        {
            IdMaterial = idMaterial;
            Cantidad = cantidad;
        }
    }
}
