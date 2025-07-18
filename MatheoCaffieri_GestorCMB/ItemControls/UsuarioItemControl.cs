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
    public partial class UsuarioItemControl : UserControl
    {
        public UsuarioItemControl()
        {
            InitializeComponent();
        }

        public string MailUsuario
        {
            get => labelInfoMailUsuario.Text;
            set => labelInfoMailUsuario.Text = value;
        }

        public string ContraseñaUsuario
        {
            get => labelInfoContraseñaUsuario.Text;
            set => labelInfoContraseñaUsuario.Text = value;
        }
    }
}
