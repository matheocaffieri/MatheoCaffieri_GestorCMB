using BL.AccessBL;
using BL.LoginBL;
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
        private readonly RolesService _rolService;
        private readonly UsuarioService _usuarioService;

        private Usuario _usuario;
        public Usuario Usuario
        {
            get => _usuario;
            private set => _usuario = value;
        }

        public event EventHandler<bool> ActivoChanged;
        public event EventHandler<Usuario> UsuarioEditado;

        // === SOLO diseñador ===
        public UsuarioItemControl() // SOLO diseñador
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                MessageBox.Show("UsuarioItemControl() sin servicios.\n\n" + Environment.StackTrace,
                                "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // no lanzar: solo para encontrar dónde lo instancian mal
            }
        }


        // === Runtime: NO encadenar con : this() ===
        public UsuarioItemControl(RolesService rolService, UsuarioService usuarioService)
        {
            InitializeComponent();

            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));

            // Suscribimos UNA sola vez
            SwitchHabilitarUsuario.ToggleChanged += (s, ev) =>
                ActivoChanged?.Invoke(this, SwitchHabilitarUsuario.IsOn);
        }

        public void SetUsuario(Usuario u)
        {
            _usuario = u;
            MailUsuario = u?.Mail ?? string.Empty;
            Activo = u?.IsActive ?? false;
            Tag = u; // si lo usás en otros lados
        }

        // --- Propiedades de UI ---

        public bool Activo
        {
            get => SwitchHabilitarUsuario.IsOn;
            set
            {
                SwitchHabilitarUsuario.IsOn = value;
                SwitchHabilitarUsuario.Invalidate(); // repintado
            }
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
            set => labelInfoMailUsuario.Text = value ?? string.Empty;
        }

        // --- Eventos ---

        private void buttonEditarUsuario_Click(object sender, EventArgs e)
        {
            var u = Usuario ?? Tag as Usuario;
            if (u == null) return;

            using (var frm = new EditUserForm(u, _rolService))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // refrescar UI local con el estado que haya quedado en u
                    MailUsuario = u.Mail;
                    Activo = u.IsActive;
                    UsuarioEditado?.Invoke(this, u);
                }
            }
        }
    }

}
