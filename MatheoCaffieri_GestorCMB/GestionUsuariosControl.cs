using BL.AccessBL;
using BL.LoginBL;
using DomainModel.Exceptions;
using DomainModel.Login;
using Interfaces.LoginInterfaces;
using MatheoCaffieri_GestorCMB.ItemControls;
using Services.Language;
using Services.Logs;
using Services.LoginService;
using Services.RoleService;
using Services.RoleService.Logic;
using Services.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AccesoServiceLogic  = Services.RoleService.Logic.AccesoService;
using RolesServiceLogic   = Services.RoleService.Logic.RolesService;
using UsuarioPermisosServiceLogic = Services.RoleService.Logic.UsuarioPermisosService;

namespace MatheoCaffieri_GestorCMB
{
    public partial class GestionUsuariosControl : UserControl
    {
        private const string REQUIRED = "GESTIONAR_USUARIOS";

        private readonly UsuarioService            _usuarioSrv;
        private readonly RolesService              _rolesSrv;
        private readonly UsuarioPermisosService    _userPermsSrv;
        private readonly AccesoService             _accesoSrv;

        // ── estado UI ──────────────────────────────────────────────
        private Guid _rolSeleccionado = Guid.Empty;
        private FlowLayoutPanel _panelUsuariosLista;
        private FlowLayoutPanel _panelRolesBotones;
        private Panel           _panelPermisosContenido;
        private Label           _labelRolTitulo;
        private ComboBox        _comboRolCrear;
        private TextBox         _txtMail, _txtPass, _txtTel;
        private ComboBox        _comboIdioma;
        private readonly Dictionary<TipoPermiso, ToggleSwitch> _toggles =
            new Dictionary<TipoPermiso, ToggleSwitch>();

        // ── panel izquierdo scrollable ─────────────────────────────
        private Panel _leftScrollArea;
        private Panel _leftSepH;
        private Panel _leftFormCrear;
        private bool  _relayouting;

        // ── lista de usuarios con colapso ──────────────────────────
        private const int USUARIOS_PREVIEW = 4;
        private bool _usersExpanded = false;
        private List<Usuario> _cachedUsuarios = new List<Usuario>();

        // ── sección roles dinámica ─────────────────────────────────
        private Panel _rolesWrapper;
        private Panel _rolesScrollInner;
        private Panel _secRoles;

        // ── definición de módulos ──────────────────────────────────
        // NombreKey es la clave de Resources.resx; se traduce en MakeModuloRow.
        private class Modulo
        {
            public string      NombreKey { get; set; }
            public string      NombreFallback { get; set; }
            public TipoPermiso? Ver      { get; set; }
            public TipoPermiso? Gestionar { get; set; }
        }

        private static readonly Modulo[] _modulos = new Modulo[]
        {
            new Modulo { NombreKey = "mod_proyectos",       NombreFallback = "Proyectos",       Ver = TipoPermiso.VER_PROYECTOS,       Gestionar = TipoPermiso.GESTIONAR_PROYECTOS       },
            new Modulo { NombreKey = "mod_inventario",      NombreFallback = "Inventario",      Ver = TipoPermiso.VER_INVENTARIO,      Gestionar = TipoPermiso.GESTIONAR_MATERIALES      },
            new Modulo { NombreKey = "mod_empleados",       NombreFallback = "Empleados",       Ver = TipoPermiso.VER_EMPLEADOS,       Gestionar = TipoPermiso.GESTIONAR_EMPLEADOS       },
            new Modulo { NombreKey = "mod_clientes",        NombreFallback = "Clientes",        Ver = TipoPermiso.VER_CLIENTES,        Gestionar = TipoPermiso.GESTIONAR_CLIENTES        },
            new Modulo { NombreKey = "mod_proveedores",     NombreFallback = "Proveedores",     Ver = TipoPermiso.VER_PROVEEDORES,     Gestionar = TipoPermiso.GESTIONAR_PROVEEDORES     },
            new Modulo { NombreKey = "mod_informes_compra", NombreFallback = "Informes compra", Ver = TipoPermiso.VER_INFORMES_COMPRA, Gestionar = TipoPermiso.GESTIONAR_INFORMES_COMPRA },
            new Modulo { NombreKey = "mod_logs",             NombreFallback = "Logs",            Ver = TipoPermiso.VER_LOGS,            Gestionar = null                                  },
            new Modulo { NombreKey = "mod_configuracion",   NombreFallback = "Configuración",   Ver = null,                            Gestionar = TipoPermiso.CONFIGURAR_PARAMETROS     },
            new Modulo { NombreKey = "mod_usuarios",        NombreFallback = "Usuarios",        Ver = null,                            Gestionar = TipoPermiso.GESTIONAR_USUARIOS        },
        };

        // ══════════════════════════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════════════════════════

        public GestionUsuariosControl(RolesService rolesService, UsuarioService usuarioService)
        {
            InitializeComponent();

            if (!SessionContext.Has(REQUIRED))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_sin_permisos")    ?? "No tenés permisos para acceder a esta pantalla.",
                    LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cs = ConfigurationManager
                .ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"]
                .ConnectionString;

            _rolesSrv    = rolesService    ?? throw new ArgumentNullException(nameof(rolesService));
            _usuarioSrv  = usuarioService  ?? throw new ArgumentNullException(nameof(usuarioService));
            _userPermsSrv = AccessServicesFactory.CreateUsuarioPermisosService(cs);
            _accesoSrv    = AccessServicesFactory.CreateAccesoService(cs);
        }

        // ══════════════════════════════════════════════════════════
        // LOAD
        // ══════════════════════════════════════════════════════════

        private void GestionUsuariosControl_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            if (_rolesSrv == null || _usuarioSrv == null) return;

            this.Controls.Clear();
            BuildUI();

            // Diferir la carga hasta que el layout esté completo para que
            // ClientSize de los paneles hijos tenga el valor correcto.
            BeginInvoke((Action)(() =>
            {
                CargarUsuarios();
                CargarRolesBotones();
            }));
        }

        // ══════════════════════════════════════════════════════════
        // BUILD UI
        // ══════════════════════════════════════════════════════════

        private void BuildUI()
        {
            this.BackColor    = Color.White;
            this.DoubleBuffered = true;

            var root = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            var panelLeft = BuildPanelLeft();
            panelLeft.Dock  = DockStyle.Left;
            panelLeft.Width = 400;

            var sep = new Panel { Dock = DockStyle.Left, Width = 1, BackColor = Color.FromArgb(220, 220, 220) };

            var panelRight = BuildPanelRight();
            panelRight.Dock = DockStyle.Fill;

            // Dock=Fill debe quedar primero en Controls para que Left se procese antes
            root.Controls.Add(panelRight);
            root.Controls.Add(sep);
            root.Controls.Add(panelLeft);

            this.Controls.Add(root);
        }

        // ── LEFT ──────────────────────────────────────────────────

        private Panel BuildPanelLeft()
        {
            var bg = Color.FromArgb(247, 248, 250);
            var panel = new Panel { BackColor = bg, Padding = new Padding(12, 12, 8, 8) };

            var title = MakeLbl(LanguageService.Current?.T("hdr_usuarios") ?? "Usuarios", 13f, FontStyle.Bold, Color.FromArgb(30, 30, 30));
            title.Dock   = DockStyle.Top;
            title.Height = 36;

            // Área scrollable única: items de usuario + separador + formulario
            _leftScrollArea = new Panel
            {
                Dock       = DockStyle.Fill,
                AutoScroll = true,
                BackColor  = bg,
            };

            _panelUsuariosLista = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                AutoSize      = true,
                AutoSizeMode  = AutoSizeMode.GrowAndShrink,
                BackColor     = bg,
                Location      = new Point(0, 0),
                Padding       = new Padding(0, 4, 0, 4),
            };

            _leftSepH    = new Panel { BackColor = Color.FromArgb(215, 215, 215) };
            _leftFormCrear = BuildFormCrearUsuario();

            _leftScrollArea.Controls.Add(_panelUsuariosLista);
            _leftScrollArea.Controls.Add(_leftSepH);
            _leftScrollArea.Controls.Add(_leftFormCrear);

            _leftScrollArea.Resize          += (s, e) => RelayoutLeftScroll();
            _panelUsuariosLista.SizeChanged += (s, e) => RelayoutLeftScroll();

            panel.Controls.Add(_leftScrollArea);
            panel.Controls.Add(title);

            return panel;
        }

        private void RelayoutLeftScroll()
        {
            if (_relayouting || _leftScrollArea == null || _panelUsuariosLista == null) return;
            _relayouting = true;
            try
            {
                int w = _leftScrollArea.ClientSize.Width;
                if (w <= 0) return;

                _panelUsuariosLista.Width = w;

                int y = _panelUsuariosLista.Bottom + 4;
                _leftSepH.SetBounds(0, y, w, 1);

                y = _leftSepH.Bottom + 8;
                _leftFormCrear.SetBounds(0, y, w, _leftFormCrear.Height);
            }
            finally
            {
                _relayouting = false;
            }
        }

        private Panel BuildFormCrearUsuario()
        {
            var bg    = Color.FromArgb(247, 248, 250);
            var form  = new Panel { BackColor = bg, Height = 228, Padding = new Padding(0, 8, 0, 4) };

            var titleCrear = MakeLbl(LanguageService.Current?.T("hdr_nuevo_usuario") ?? "Nuevo usuario", 9.5f, FontStyle.Bold, Color.DimGray);
            titleCrear.Dock   = DockStyle.Top;
            titleCrear.Height = 22;

            var rowMail   = MakeCampo(LanguageService.Current?.T("lbl_mail")       ?? "Mail",       out _txtMail);
            var rowPass   = MakeCampo(LanguageService.Current?.T("lbl_contrasena") ?? "Contraseña", out _txtPass, isPassword: true);
            var rowTel    = MakeCampo(LanguageService.Current?.T("lbl_telefono")   ?? "Teléfono",   out _txtTel);
            var rowIdioma = MakeCampoCombo(LanguageService.Current?.T("lbl_idioma") ?? "Idioma", out _comboIdioma);
            _comboIdioma.Items.AddRange(new object[] { "Español", "English" });
            _comboIdioma.SelectedIndex = 0;

            var rowRol = MakeCampoCombo(LanguageService.Current?.T("lbl_rol") ?? "Rol", out _comboRolCrear);

            var btnCrear = new Button
            {
                Text      = LanguageService.Current?.T("btn_crear_usuario") ?? "Crear usuario",
                Dock      = DockStyle.Top,
                Height    = 30,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = MakeFont(9.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                Margin    = new Padding(0, 4, 0, 0),
            };
            btnCrear.FlatAppearance.BorderSize = 0;
            btnCrear.Click += BtnCrearUsuario_Click;

            // Dock=Top: el último agregado queda visualmente arriba.
            // Agregar en orden inverso: btnCrear primero (queda abajo), titleCrear último (queda arriba).
            form.Controls.Add(btnCrear);
            form.Controls.Add(rowRol);
            form.Controls.Add(rowIdioma);
            form.Controls.Add(rowTel);
            form.Controls.Add(rowPass);
            form.Controls.Add(rowMail);
            form.Controls.Add(titleCrear);

            return form;
        }

        // ── RIGHT ─────────────────────────────────────────────────

        private Panel BuildPanelRight()
        {
            var panel = new Panel { BackColor = Color.White, Padding = new Padding(16, 12, 12, 8) };

            _secRoles       = BuildSeccionRoles();
            _secRoles.Dock  = DockStyle.Top;
            _secRoles.Height = 112; // se ajusta dinámicamente en AjustarAlturaRoles

            var sepH = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(215, 215, 215) };

            var secPermisos  = BuildSeccionPermisos();
            secPermisos.Dock = DockStyle.Fill;

            panel.Controls.Add(secPermisos);
            panel.Controls.Add(sepH);
            panel.Controls.Add(_secRoles);

            return panel;
        }

        private Panel BuildSeccionRoles()
        {
            var sec = new Panel { BackColor = Color.White };

            var title = MakeLbl(LanguageService.Current?.T("hdr_roles") ?? "Roles", 13f, FontStyle.Bold, Color.FromArgb(30, 30, 30));
            title.Dock   = DockStyle.Top;
            title.Height = 34;

            var rowNuevoRol = BuildRowNuevoRol();
            rowNuevoRol.Dock   = DockStyle.Top;
            rowNuevoRol.Height = 36;

            // Outer panel: altura fija que clipea el scrollbar (truco: inner es más alto
            // que outer, por lo que la scrollbar queda fuera del área visible).
            _rolesWrapper = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 34,
                BackColor = Color.White,
            };

            // Inner panel: AutoScroll habilitado; su scrollbar queda por debajo del clip.
            _rolesScrollInner = new Panel
            {
                Location   = new Point(0, 0),
                Height     = 34 + SystemInformation.HorizontalScrollBarHeight + 2,
                AutoScroll = true,
                TabStop    = true,
                BackColor  = Color.White,
            };

            _panelRolesBotones = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                AutoSize      = true,
                AutoSizeMode  = AutoSizeMode.GrowAndShrink,
                BackColor     = Color.White,
                Padding       = new Padding(0, 4, 0, 0),
                Height        = 34,
            };

            _rolesScrollInner.Controls.Add(_panelRolesBotones);
            _rolesWrapper.Controls.Add(_rolesScrollInner);

            // Rueda del mouse desplaza horizontalmente
            _rolesScrollInner.MouseWheel += (s, e) =>
            {
                var inner = (Panel)s;
                int newX = Math.Max(0, Math.Min(
                    inner.HorizontalScroll.Maximum,
                    -inner.AutoScrollPosition.X - e.Delta / 3));
                inner.AutoScrollPosition = new Point(newX, 0);
            };

            // Al entrar con el mouse, enfocar el inner para recibir scroll
            _rolesWrapper.MouseEnter += (s, e) => _rolesScrollInner.Focus();

            _rolesWrapper.Resize += (s, e) =>
            {
                var p = (Panel)s;
                _rolesScrollInner.Width = Math.Max(1, p.ClientSize.Width);
            };

            _panelRolesBotones.SizeChanged += (s, e) => AjustarAlturaRoles();

            // orden inverso de Add: el último agregado queda visualmente arriba
            sec.Controls.Add(rowNuevoRol);
            sec.Controls.Add(_rolesWrapper);
            sec.Controls.Add(title);

            return sec;
        }

        private const int ROLES_MAX_H  = 110;
        private const int ROLES_ROW_H  = 34;  // título
        private const int ROLES_CREAR_H = 36;  // fila crear rol
        private const int ROLES_PADDING = 8;

        private void AjustarAlturaRoles()
        {
            if (_rolesWrapper == null || _secRoles == null) return;
            _rolesWrapper.Height = 34;
            _secRoles.Height     = ROLES_ROW_H + 34 + ROLES_CREAR_H + 6;
        }

        private Panel BuildRowNuevoRol()
        {
            var row = new Panel { BackColor = Color.White };

            var txtNombre = new TextBox
            {
                Width       = 170,
                Font        = MakeFont(9f),
                BorderStyle = BorderStyle.FixedSingle,
                Location    = new Point(0, 4),
            };

            var btnCrearRol = new Button
            {
                Text      = LanguageService.Current?.T("btn_crear_rol") ?? "+ Crear rol",
                Width     = 88,
                Height    = 26,
                BackColor = Color.FromArgb(100, 149, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = MakeFont(9f, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                Location  = new Point(176, 3),
            };
            btnCrearRol.FlatAppearance.BorderSize = 0;
            btnCrearRol.Click += (s, e) =>
            {
                var nombre = txtNombre.Text.Trim();
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show(LanguageService.Current?.T("val_nombre_rol_requerido") ?? "El nombre del rol es obligatorio.");
                    txtNombre.Focus();
                    return;
                }
                try
                {
                    _rolesSrv.CrearRol(nombre);
                    txtNombre.Clear();
                    CargarRolesBotones();
                }
                catch (Exception)
                {
                    MessageBox.Show(LanguageService.Current?.T("err_crear_rol") ?? "Error al crear el rol.");
                }
            };

            row.Controls.Add(txtNombre);
            row.Controls.Add(btnCrearRol);
            return row;
        }

        private Panel BuildSeccionPermisos()
        {
            var sec = new Panel { BackColor = Color.White, Padding = new Padding(0, 10, 0, 0) };

            _labelRolTitulo = MakeLbl(LanguageService.Current?.T("msg_selecciona_rol") ?? "Seleccioná un rol para ver sus permisos", 10f, FontStyle.Regular, Color.DimGray);
            _labelRolTitulo.Dock   = DockStyle.Top;
            _labelRolTitulo.Height = 28;

            _panelPermisosContenido = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, AutoScroll = true };

            sec.Controls.Add(_panelPermisosContenido);
            sec.Controls.Add(_labelRolTitulo);

            return sec;
        }

        // ══════════════════════════════════════════════════════════
        // ROLE CHIPS
        // ══════════════════════════════════════════════════════════

        private void CargarRolesBotones()
        {
            _panelRolesBotones.SuspendLayout();
            _panelRolesBotones.Controls.Clear();

            foreach (var rol in _rolesSrv.ListarRoles())
            {
                var id     = GetRolId(rol);
                var nombre = GetRolNombre(rol);
                _panelRolesBotones.Controls.Add(MakeRolChip(id, nombre));
            }

            _panelRolesBotones.ResumeLayout();
            AjustarAlturaRoles();
            ActualizarComboRoles();
        }

        private Button MakeRolChip(Guid id, string nombre)
        {
            bool seleccionado = id == _rolSeleccionado;
            var chip = new Button
            {
                AutoSize  = false,
                Height    = 26,
                Font      = MakeFont(8.5f),
                FlatStyle = FlatStyle.Flat,
                Cursor    = Cursors.Hand,
                Margin    = new Padding(0, 0, 6, 4),
                BackColor = seleccionado ? Color.FromArgb(76, 175, 80) : Color.FromArgb(228, 228, 228),
                ForeColor = seleccionado ? Color.White : Color.FromArgb(50, 50, 50),
                Tag       = id,
            };
            chip.FlatAppearance.BorderSize = 0;
            chip.MouseEnter += (s, e) => _rolesScrollInner?.Focus();

            // texto con candado si es admin
            bool isAdmin = id == RolesService.AdminRoleId;
            chip.Text = isAdmin ? $"{nombre}  🔒" : nombre;

            // ancho dinámico según texto
            using (var g = chip.CreateGraphics())
                chip.Width = (int)g.MeasureString(chip.Text, chip.Font).Width + 22;

            chip.Click += (s, e) =>
            {
                _rolSeleccionado = (Guid)((Button)s).Tag;
                RefrescarEstiloChips();
                CargarPermisosRol(_rolSeleccionado);
            };

            return chip;
        }

        private void RefrescarEstiloChips()
        {
            foreach (Control c in _panelRolesBotones.Controls)
            {
                if (!(c is Button chip) || !(chip.Tag is Guid id)) continue;
                bool sel = id == _rolSeleccionado;
                chip.BackColor = sel ? Color.FromArgb(76, 175, 80) : Color.FromArgb(228, 228, 228);
                chip.ForeColor = sel ? Color.White : Color.FromArgb(50, 50, 50);
            }
        }

        // ══════════════════════════════════════════════════════════
        // PERMISSIONS MATRIX
        // ══════════════════════════════════════════════════════════

        private void CargarPermisosRol(Guid rolId)
        {
            _toggles.Clear();
            _panelPermisosContenido.SuspendLayout();
            _panelPermisosContenido.Controls.Clear();

            bool esAdmin   = rolId == RolesService.AdminRoleId;
            var activosSet = new HashSet<TipoPermiso>(_rolesSrv.ObtenerPermisosDeRol(rolId));

            string rolNombre = GetRolNombreById(rolId);
            _labelRolTitulo.Text      = esAdmin
                ? string.Format(LanguageService.Current?.T("lbl_permisos_admin_fmt") ?? "Permisos: {0}   (protegido, solo lectura)", rolNombre)
                : string.Format(LanguageService.Current?.T("lbl_permisos_fmt")       ?? "Permisos: {0}", rolNombre);
            _labelRolTitulo.Font      = MakeFont(10f, FontStyle.Bold);
            _labelRolTitulo.ForeColor = esAdmin ? Color.Crimson : Color.FromArgb(40, 40, 40);

            // Dock=Top: el último control agregado queda visualmente arriba.
            // → módulos en orden inverso (Usuarios primero = queda abajo), header al final = queda arriba.
            for (int i = _modulos.Length - 1; i >= 0; i--)
                _panelPermisosContenido.Controls.Add(
                    MakeModuloRow(_modulos[i], activosSet, rolId, esAdmin, i % 2 == 0));

            _panelPermisosContenido.Controls.Add(MakeHeaderRow());

            _panelPermisosContenido.ResumeLayout(true);
        }

        private const int COL_VER       = 160;
        private const int COL_GESTIONAR = 230;

        private Panel MakeHeaderRow()
        {
            var row = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 28,
                BackColor = Color.FromArgb(238, 241, 245),
            };

            void AddHdr(string txt, int x)
            {
                var lbl = MakeLbl(txt, 8.5f, FontStyle.Bold, Color.DimGray);
                lbl.AutoSize = true;
                lbl.Location = new Point(x, 7);
                row.Controls.Add(lbl);
            }

            AddHdr(LanguageService.Current?.T("hdr_modulo")    ?? "Módulo",     10);
            AddHdr(LanguageService.Current?.T("hdr_ver")       ?? "VER",        COL_VER);
            AddHdr(LanguageService.Current?.T("hdr_gestionar") ?? "GESTIONAR",  COL_GESTIONAR);

            return row;
        }

        private Panel MakeModuloRow(Modulo mod, HashSet<TipoPermiso> activos,
                                    Guid rolId, bool esAdmin, bool altRow)
        {
            var rowBg = altRow ? Color.White : Color.FromArgb(249, 250, 251);
            var row   = new Panel { Dock = DockStyle.Top, Height = 38, BackColor = rowBg };

            var nombreMod = LanguageService.Current?.T(mod.NombreKey) ?? mod.NombreFallback;
            var lblNombre = MakeLbl(nombreMod, 9f, FontStyle.Regular, Color.FromArgb(40, 40, 40));
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(10, (38 - 18) / 2);
            row.Controls.Add(lblNombre);

            if (mod.Ver.HasValue)
            {
                var perm = mod.Ver.Value;
                var tog  = MakeToggle(activos.Contains(perm), !esAdmin, rowBg);
                tog.Location = new Point(COL_VER, (38 - tog.Height) / 2);
                if (!esAdmin) tog.ToggleChanged += (s, e) => GuardarToggle(rolId, perm, ((ToggleSwitch)s).IsOn);
                row.Controls.Add(tog);
                _toggles[perm] = tog;
            }

            if (mod.Gestionar.HasValue)
            {
                var perm = mod.Gestionar.Value;
                var tog  = MakeToggle(activos.Contains(perm), !esAdmin, rowBg);
                tog.Location = new Point(COL_GESTIONAR, (38 - tog.Height) / 2);
                if (!esAdmin) tog.ToggleChanged += (s, e) => GuardarToggle(rolId, perm, ((ToggleSwitch)s).IsOn);
                row.Controls.Add(tog);
                _toggles[perm] = tog;
            }

            return row;
        }

        private static ToggleSwitch MakeToggle(bool isOn, bool enabled, Color parentBg)
        {
            return new ToggleSwitch
            {
                Width    = 44,
                Height   = 22,
                IsOn     = isOn,
                Enabled  = enabled,
                OnColor  = Color.FromArgb(76, 175, 80),
                OffColor = Color.FromArgb(190, 190, 190),
            };
        }

        private void GuardarToggle(Guid rolId, TipoPermiso permiso, bool activar)
        {
            try
            {
                var accesoId = _accesoSrv.GetOrCreateId(permiso, permiso.ToString().Replace('_', ' '));
                if (activar)
                    _rolesSrv.AsignarPermisoARol(rolId, accesoId);
                else
                    _rolesSrv.QuitarPermisoDeRol(rolId, accesoId);
                LoggerLogic.Info($"[GestionUsuariosControl] Permiso {(activar ? "asignado" : "quitado")} al rol. Rol={rolId} Permiso={permiso}");
            }
            catch (AppException ex)
            {
                RevertirToggle(permiso, !activar);
                LoggerLogic.Warn($"[GestionUsuariosControl] Validación al cambiar permiso de rol: {ex.MessageKey}");
                MessageBox.Show(LanguageService.Current?.T(ex.MessageKey) ?? ex.Message);
            }
            catch (Exception ex)
            {
                RevertirToggle(permiso, !activar);
                LoggerLogic.Error($"[GestionUsuariosControl] Falla al actualizar permisos. Rol={rolId} Permiso={permiso}", ex);
                MessageBox.Show(LanguageService.Current?.T("err_actualizar_permisos") ?? "No se pudo actualizar los permisos.");
            }
        }

        private void RevertirToggle(TipoPermiso permiso, bool valorOriginal)
        {
            if (_toggles.TryGetValue(permiso, out var tog))
                tog.IsOn = valorOriginal;
        }

        // ══════════════════════════════════════════════════════════
        // USERS
        // ══════════════════════════════════════════════════════════

        private void CargarUsuarios()
        {
            _cachedUsuarios = _usuarioSrv.ObtenerTodos().ToList();
            RenderUsuarios();
        }

        private void RenderUsuarios()
        {
            _panelUsuariosLista.SuspendLayout();
            _panelUsuariosLista.Controls.Clear();

            int itemW = _leftScrollArea?.ClientSize.Width > 0
                ? _leftScrollArea.ClientSize.Width - 2 : 310;

            var toShow = _usersExpanded
                ? _cachedUsuarios
                : _cachedUsuarios.Take(USUARIOS_PREVIEW).ToList();

            int num = 1;
            foreach (var u in toShow)
            {
                var item = new ItemControls.UsuarioItemControl(_rolesSrv, _usuarioSrv)
                {
                    MailUsuario = u.Mail,
                    Activo      = u.IsActive,
                    Tag         = u,
                    Width       = itemW,
                    Height      = 46,
                    Margin      = new Padding(0, 0, 0, 5),
                };
                item.Numero = num++;
                item.SetUsuario(u);

                item.ActivoChanged += (s, on) =>
                {
                    var ctrl = (ItemControls.UsuarioItemControl)s;
                    var usr  = (Usuario)ctrl.Tag;
                    try
                    {
                        _usuarioSrv.SetActivo(usr.IdUsuario, on);
                        usr.IsActive = on;
                        LoggerLogic.Info($"[GestionUsuariosControl] Usuario {(on ? "activado" : "desactivado")}. Id={usr.IdUsuario} Mail='{usr.Mail}'");
                    }
                    catch (AppException ex)
                    {
                        ctrl.Activo = !on;
                        LoggerLogic.Warn($"[GestionUsuariosControl] Validación al cambiar estado de usuario: {ex.MessageKey}");
                        MessageBox.Show(LanguageService.Current?.T(ex.MessageKey) ?? ex.Message);
                    }
                    catch (Exception ex)
                    {
                        ctrl.Activo = !on;
                        LoggerLogic.Error($"[GestionUsuariosControl] Falla al cambiar estado de usuario. Id={usr.IdUsuario}", ex);
                    }
                };

                _panelUsuariosLista.Controls.Add(item);
            }

            if (_cachedUsuarios.Count > USUARIOS_PREVIEW)
            {
                int resto = _cachedUsuarios.Count - USUARIOS_PREVIEW;
                var link = new LinkLabel
                {
                    Text      = _usersExpanded
                        ? (LanguageService.Current?.T("lnk_ver_menos") ?? "Ver menos")
                        : string.Format(LanguageService.Current?.T("lnk_ver_mas_fmt") ?? "Ver {0} más...", resto),
                    Width     = itemW,
                    Height    = 22,
                    Font      = MakeFont(8.5f),
                    Margin    = new Padding(4, 2, 0, 4),
                    LinkColor = Color.FromArgb(100, 149, 237),
                    BackColor = Color.Transparent,
                };
                link.LinkClicked += (s, e) =>
                {
                    _usersExpanded = !_usersExpanded;
                    RenderUsuarios();
                };
                _panelUsuariosLista.Controls.Add(link);
            }

            _panelUsuariosLista.ResumeLayout();
            RelayoutLeftScroll();
        }

        private void BtnCrearUsuario_Click(object sender, EventArgs e)
        {
            var mail   = (_txtMail?.Text ?? "").Trim();
            var pass   = _txtPass?.Text ?? "";
            var telStr = (_txtTel?.Text ?? "").Trim();
            var idi    = (_comboIdioma?.SelectedIndex ?? 0) == 1 ? "en" : "es";

            if (string.IsNullOrWhiteSpace(mail))
            {
                LoggerLogic.Warn("[GestionUsuariosControl] Validación: mail vacío al crear usuario.");
                MessageBox.Show(LanguageService.Current?.T("val_mail_requerido") ?? "El mail es obligatorio."); _txtMail?.Focus(); return;
            }
            if (!Validaciones.EsMailValido(mail))
            {
                LoggerLogic.Warn($"[GestionUsuariosControl] Validación: mail con formato inválido al crear usuario ('{mail}').");
                MessageBox.Show(LanguageService.Current?.T("val_mail_invalido") ?? "El mail no tiene un formato válido."); _txtMail?.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(pass))
            {
                LoggerLogic.Warn("[GestionUsuariosControl] Validación: contraseña vacía al crear usuario.");
                MessageBox.Show(LanguageService.Current?.T("val_contrasena_requerida") ?? "La contraseña es obligatoria."); _txtPass?.Focus(); return;
            }
            if (!string.IsNullOrWhiteSpace(telStr) && !int.TryParse(telStr, out _))
            {
                LoggerLogic.Warn($"[GestionUsuariosControl] Validación: teléfono inválido al crear usuario ('{telStr}').");
                MessageBox.Show(LanguageService.Current?.T("val_telefono_invalido") ?? "El teléfono debe ser numérico."); _txtTel?.Focus(); return;
            }

            try
            {
                if (_usuarioSrv.ObtenerPorMail(mail) != null)
                {
                    LoggerLogic.Warn($"[GestionUsuariosControl] Validación: mail duplicado al crear usuario ('{mail}').");
                    MessageBox.Show(LanguageService.Current?.T("err_mail_duplicado") ?? "Ya existe un usuario con ese mail."); return;
                }

                var nuevo = new Usuario
                {
                    IdUsuario = Guid.NewGuid(),
                    Mail      = mail,
                    IsActive  = true,
                    Telefono  = string.IsNullOrWhiteSpace(telStr) ? 0 : int.Parse(telStr),
                    Idioma    = idi,
                };
                _usuarioSrv.CrearUsuario(nuevo, pass);
                LoggerLogic.Info($"[GestionUsuariosControl] Usuario creado. Id={nuevo.IdUsuario} Mail='{nuevo.Mail}'");

                if (_comboRolCrear?.SelectedValue is Guid rolId && rolId != Guid.Empty)
                {
                    _rolesSrv.AsignarUsuarioARol(rolId, nuevo.IdUsuario);
                    LoggerLogic.Info($"[GestionUsuariosControl] Rol inicial asignado al nuevo usuario. Rol={rolId} Usuario={nuevo.IdUsuario}");
                }

                MessageBox.Show(LanguageService.Current?.T("msg_usuario_creado") ?? "Usuario creado correctamente.");
                _txtMail.Clear(); _txtPass.Clear(); _txtTel.Clear();
                _comboRolCrear.SelectedIndex = 0;
                CargarUsuarios();
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[GestionUsuariosControl] Validación al crear usuario: {ex.MessageKey}");
                MessageBox.Show(LanguageService.Current?.T(ex.MessageKey) ?? ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[GestionUsuariosControl] Falla al crear usuario ('{mail}').", ex);
                MessageBox.Show(LanguageService.Current?.T("err_crear_usuario") ?? "Error al crear el usuario.");
            }
        }

        private void ActualizarComboRoles()
        {
            if (_comboRolCrear == null) return;
            var items = new List<RolItem> { new RolItem { Id = Guid.Empty, Nombre = LanguageService.Current?.T("txt_sin_rol") ?? "(Sin rol)" } };
            foreach (var rol in _rolesSrv.ListarRoles())
                items.Add(new RolItem { Id = GetRolId(rol), Nombre = GetRolNombre(rol) });
            _comboRolCrear.DisplayMember = "Nombre";
            _comboRolCrear.ValueMember   = "Id";
            _comboRolCrear.DataSource    = items;
            _comboRolCrear.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════════════════
        // HELPERS
        // ══════════════════════════════════════════════════════════

        private string GetRolNombreById(Guid id)
        {
            foreach (var r in _rolesSrv.ListarRoles())
                if (GetRolId(r) == id) return GetRolNombre(r);
            return "(Rol)";
        }

        private static Guid GetRolId(object rol)
        {
            var p = rol.GetType().GetProperty("Id");
            return p?.GetValue(rol, null) is Guid g ? g : Guid.Empty;
        }

        private static string GetRolNombre(object rol)
        {
            var p = rol.GetType().GetProperty("Nombre") ?? rol.GetType().GetProperty("Name");
            var v = p?.GetValue(rol, null) as string;
            return string.IsNullOrWhiteSpace(v) ? "(Rol)" : v;
        }

        private static Label MakeLbl(string text, float size, FontStyle style, Color color)
            => new Label
            {
                Text      = text,
                Font      = new Font("Microsoft YaHei UI", size, style),
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize  = false,
            };

        private static Font MakeFont(float size, FontStyle style = FontStyle.Regular)
            => new Font("Microsoft YaHei UI", size, style);

        private static Panel MakeCampo(string label, out TextBox tb, bool isPassword = false)
        {
            var row = new Panel { Dock = DockStyle.Top, Height = 27, BackColor = Color.Transparent, Margin = new Padding(0, 2, 0, 0) };
            var lbl = new Label
            {
                Text      = label,
                Width     = 82,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock      = DockStyle.Left,
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.DimGray,
            };
            tb = new TextBox { Dock = DockStyle.Fill, Font = new Font("Microsoft YaHei UI", 9f), BorderStyle = BorderStyle.FixedSingle };
            if (isPassword) tb.PasswordChar = '●';
            row.Controls.Add(tb);
            row.Controls.Add(lbl);
            return row;
        }

        private static Panel MakeCampoCombo(string label, out ComboBox combo)
        {
            var row = new Panel { Dock = DockStyle.Top, Height = 27, BackColor = Color.Transparent, Margin = new Padding(0, 2, 0, 0) };
            var lbl = new Label
            {
                Text      = label,
                Width     = 82,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock      = DockStyle.Left,
                Font      = new Font("Microsoft YaHei UI", 8.5f),
                ForeColor = Color.DimGray,
            };
            combo = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Microsoft YaHei UI", 9f) };
            row.Controls.Add(combo);
            row.Controls.Add(lbl);
            return row;
        }

        // stubs requeridos por el designer (los controles originales se eliminan en Load)
        private void buttonAgregarUser_Click(object sender, EventArgs e) { }
        private void buttonAgregarRol_Click(object sender, EventArgs e) { }
    }
}
