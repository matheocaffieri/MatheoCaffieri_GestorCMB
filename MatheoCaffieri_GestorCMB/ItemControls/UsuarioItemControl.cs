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

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class UsuarioItemControl : UserControl
    {
        public Usuario Usuario { get; set; }

        public event EventHandler<bool> ActivoChanged;
        public UsuarioItemControl()
        {
            InitializeComponent();
            SwitchHabilitarUsuario.ToggleChanged += (s, e) =>
                ActivoChanged?.Invoke(this, SwitchHabilitarUsuario.IsOn);
        }

        // UsuarioItemControl
        public bool Activo
        {
            get => SwitchHabilitarUsuario.IsOn;
            set
            {
                SwitchHabilitarUsuario.IsOn = value;
                SwitchHabilitarUsuario.Invalidate(); // fuerza repintado
            }
        }



        private void UsuarioItemControl_Load(object sender, EventArgs e)
        {
            SwitchHabilitarUsuario.ToggleChanged += (s, ev) =>
            {
                ActivoChanged?.Invoke(this, SwitchHabilitarUsuario.IsOn);
            };
        }


        public int Numero
        {
            get => int.TryParse(labelNumero.Tag?.ToString(), out var n) ? n : 0;
            set
            {
                labelNumero.Tag = value;        
                labelNumero.Text = $"#{value}"; 
            }
        }

        public string MailUsuario
        {
            get => labelInfoMailUsuario.Text;
            set => labelInfoMailUsuario.Text = value;
        }

        private void buttonEditarUsuario_Click(object sender, EventArgs e)
        {
            if (Usuario != null)
            {
                using (var frm = new EditUserForm(Usuario))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // refrescar datos en el control con la info editada
                        this.MailUsuario = Usuario.Mail;
                        this.Activo = Usuario.IsActive;
                    }
                }
            }
        }
    }
}
