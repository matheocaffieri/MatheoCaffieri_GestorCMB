using DomainModel.Login;
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
    public partial class EditUserForm : Form
    {
        private Usuario _usuario;
        public EditUserForm(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));

            textBoxMailEditUser.Text = _usuario.Mail;
            textBoxContraseñaEditUser.Text = _usuario.Contraseña;
            textBoxTelEditUser.Text = _usuario.Telefono.ToString();
        }
    }
}
