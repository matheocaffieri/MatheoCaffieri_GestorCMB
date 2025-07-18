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
    public partial class MaterialFaltanteItemControl : UserControl
    {
        public MaterialFaltanteItemControl()
        {
            InitializeComponent();
        }

        public string DescripcionArticuloFaltante
        {
            get => labelInfoDescripcionArtFalt.Text;
            set => labelInfoDescripcionArtFalt.Text = value ?? "Sin descripción";
        }

        public string TipoArticuloFaltante
        {
            get => labelInfoTipoArtFalt.Text;
            set => labelInfoTipoArtFalt.Text = value ?? "Sin tipo";
        }

        public string TipoUnidadArticuloFaltante
        {
            get => labelInfoUnidadFalt.Text;
            set => labelInfoUnidadFalt.Text = value ?? "Sin tipo de unidad";
        }

        public string CantidadArticuloFaltante
        {
            get => labelInfoCantidadFaltante.Text;
            set => labelInfoCantidadFaltante.Text = value ?? "Sin cantidad";
        }
    }
}
