using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services.LoginComposite;

namespace MatheoCaffieri_GestorCMB
{
    public partial class GestionUsuariosControl : UserControl
    {
        public GestionUsuariosControl()
        {
            InitializeComponent();
        }

        private void buttonGestionPermisos_Click(object sender, EventArgs e)
        {
            AccesosForm accesosForm = new AccesosForm();
            accesosForm.Show();
        }

        private void ObtenerUsuariosItems()
        {
         /*  List<Usuario> usuarios = ((IGenericRepository<Usuario>)new UsuarioBL()).GetAll();
            UsuariosLayoutPanel.Controls.Clear();
            usuarios.ForEach(u =>
            {
                UsuarioItemControl usuarioItemControl = new UsuarioItemControl
                {
                    MailUsuario = u.Mail,
                    ContraseñaUsuario = u.Contraseña
                };
                // Agregas el control al LayoutPanel
                UsuariosLayoutPanel.Controls.Add(usuarioItemControl);
            });  */
           
        }
    }
}
