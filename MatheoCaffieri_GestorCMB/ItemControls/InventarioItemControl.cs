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
    public partial class InventarioItemControl : UserControl
    {
        public InventarioItemControl()
        {
            InitializeComponent();
        }


        private readonly InventarioBL _invBL = new InventarioBL();


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

        public string CantidadArticuloInventario
        {
            get => labelInfoCantidadInventario.Text;
            set => labelInfoCantidadInventario.Text = value ?? "Sin cantidad";
        }

        private void buttonIncreaseQ_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Tag is Guid idInv)
            {
                try
                {
                    int nuevaCantidad = _invBL.CambiarCantidad(idInv, +1);
                    labelInfoCantidadInventario.Text = nuevaCantidad.ToString(); // label de esa fila
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo aumentar la cantidad.\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void SetInventarioId(Guid idInventario)
        {
            buttonIncreaseQ.Tag = idInventario;
            buttonDecreaseQ.Tag = idInventario;
        }


        private void buttonDecreaseQ_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Tag is Guid idInv)
            {
                try
                {
                    int nuevaCantidad = _invBL.CambiarCantidad(idInv, -1);

                    // BL ya evita negativos, pero si querés también lo chequeás acá
                    labelInfoCantidadInventario.Text = nuevaCantidad.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo disminuir la cantidad.\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
