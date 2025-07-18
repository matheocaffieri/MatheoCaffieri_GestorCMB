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
    public partial class DetalleMaterialItemControl : UserControl
    {
        public DetalleMaterialItemControl()
        {
            InitializeComponent();
        }

        public string InfoDescripcionArticulo
        {
            get => labelInfoDescripcionArticulo.Text;
            set => labelInfoDescripcionArticulo.Text = value ?? "Material #N/A";
        }

        public string InfoCantidad
        {
            get => labelInfoCantidad.Text;
            set => labelInfoCantidad.Text = value ?? "Cantidad #N/A";
        }

        public string InfoTipoArticulo
        {
            get => labelInfoTipoArt.Text;
            set => labelInfoTipoArt.Text = value ?? "Tipo Artículo #N/A";
        }

        public string InfoTipoUnidad
        {
            get => labelInfoTipoUnidad.Text;
            set => labelInfoTipoUnidad.Text = value ?? "Tipo Unidad #N/A";
        }

        public string InfoCosto
        {
            get => labelInfoCosto.Text;
            set => labelInfoCosto.Text = value ?? "Costo #N/A";
        }

        public string InfoValorGananciaMat
        {
            get => labelInfoValorGananciaMat.Text;
            set => labelInfoValorGananciaMat.Text = value ?? "Valor Ganancia #N/A";
        }

    }
}
