using BL.LoginBL;
using DomainModel.Login;
using System;
using System.ComponentModel;
using System.Windows.Forms;

// Alias para evitar conflictos con otros RolesService viejos
using RolesServiceLogic = Services.RoleService.Logic.RolesService;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class UsuarioItemControl : UserControl
    {
        private readonly RolesServiceLogic _rolService;
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
        public UsuarioItemControl()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                MessageBox.Show("UsuarioItemControl() sin servicios.\n\n" + Environment.StackTrace,
                                "Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === Runtime ===
        public UsuarioItemControl(RolesServiceLogic rolService, UsuarioService usuarioService)
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
            Tag = u;
        }

        // --- Propiedades de UI ---
        public bool Activo
        {
            get => SwitchHabilitarUsuario.IsOn;
            set
            {
                SwitchHabilitarUsuario.IsOn = value;
                SwitchHabilitarUsuario.Invalidate();
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
                    MailUsuario = u.Mail;
                    Activo = u.IsActive;
                    UsuarioEditado?.Invoke(this, u);
                }
            }
        }
    }

}
