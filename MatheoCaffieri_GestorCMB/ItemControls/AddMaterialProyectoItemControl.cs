using BL;
using Services.Language;
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
        public Guid IdMaterial { get; private set; }
        public Guid IdInventario { get; private set; }

        public int StockDisponible { get; private set; }
        public int CantidadSeleccionada { get; private set; } = 0;

        // Cantidad que excede el stock disponible. Lo que se pase de aquí se registra como faltante.
        public int CantidadFaltantePreview => Math.Max(0, CantidadSeleccionada - StockDisponible);

        public event EventHandler<AgregarMaterialEventArgs> AgregarClick;

        public AddMaterialProyectoItemControl()
        {
            InitializeComponent();
            RefrescarCantidad();
        }

        public string DescripcionArticuloInventario
        {
            get => labelInfoDescArt.Text;
            set => labelInfoDescArt.Text = value ?? (LanguageService.Current?.T("txt_sin_descripcion") ?? "Sin descripción");
        }

        public string TipoArticuloInventario
        {
            get => labelInfoTipoMat.Text;
            set => labelInfoTipoMat.Text = value ?? (LanguageService.Current?.T("txt_sin_tipo") ?? "Sin tipo");
        }

        public string InfoGeneralArticuloInventario
        {
            get => labelInfoGeneralArticulo.Text;
            set => labelInfoGeneralArticulo.Text = value ?? (LanguageService.Current?.T("txt_sin_info_general") ?? "Sin información general");
        }

        public string CantidadArticuloInventario
        {
            get => labelInfoCantidadInventario.Text;
            set => labelInfoCantidadInventario.Text = value ?? "0";
        }

        public void Bind(Guid idInventario, Guid idMaterial, int stockDisponible)
        {
            IdMaterial = idMaterial;
            StockDisponible = Math.Max(0, stockDisponible);

            CantidadSeleccionada = 0;
            labelInfoCantidadInventario.Text = "0";
        }

        private void RefrescarCantidad()
        {
            labelInfoCantidadInventario.Text = CantidadSeleccionada.ToString();
        }

        private void buttonIncreaseQ_Click(object sender, EventArgs e)
        {
            // Sin tope superior: si la cantidad seleccionada supera el stock, el excedente se registra como faltante.
            CantidadSeleccionada++;
            labelInfoCantidadInventario.Text = CantidadSeleccionada.ToString();
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
