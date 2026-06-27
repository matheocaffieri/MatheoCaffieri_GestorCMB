using Services.RoleService;
using Services.LoginService.Logic;
using DomainModel.Exceptions;
using DomainModel.Login;
using Services.Language;
using Services.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RolesServiceLogic = Services.RoleService.Logic.RolesService;


namespace MatheoCaffieri_GestorCMB
{
    public partial class EditUserForm : Form
    {
        private readonly RolesServiceLogic _rolService;
        private readonly UsuarioService _usuarioService;
        private Usuario _usuario;
        private readonly Guid _loggedUserId;


        private System.Drawing.Point _mouseLocation;

        public EditUserForm(Usuario usuario, RolesServiceLogic rolService, UsuarioService usuarioService, Guid loggedUserId)
        {
            InitializeComponent();

            FormPanel.MouseDown += (s, e) => { _mouseLocation = new System.Drawing.Point(-e.X, -e.Y); };
            FormPanel.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var pos = MousePosition;
                    pos.Offset(_mouseLocation.X, _mouseLocation.Y);
                    Location = pos;
                }
            };

            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _loggedUserId = loggedUserId;

            textBoxMailEditUser.Text = _usuario.Mail;
            // El campo arranca vacío (vacío = mantener la contraseña actual) y enmascarado.
            // Nunca precargar _usuario.Contraseña: es el hash BCrypt, no la contraseña.
            textBoxContraseñaEditUser.Text = string.Empty;
            textBoxContraseñaEditUser.UseSystemPasswordChar = true;
            textBoxTelEditUser.Text = _usuario.Telefono.ToString();

            dataGridRoles.CellDoubleClick += dataGridRoles_CellDoubleClick;

            Shown += (s, e) =>
            {
                var usuarioId = _usuario.IdUsuario;
                CargarRolesGrid(usuarioId);
                CargarComboRoles(usuarioId);
            };
        }


        private class RolOption
        {
            public Guid IdRol { get; set; }
            public string Nombre { get; set; }
        }


        private void CargarRolesGrid(Guid usuarioId)
        {
            var roles = _rolService.RolesDeUsuario(usuarioId); // List<RolPlano> { IdRol, Nombre }

            var items = roles.Select((r, i) => new
            {
                N = i + 1,          // índice humano
                IdRol = r.IdRol,    // queda oculto
                Rol = r.Nombre
            }).ToList();

            dataGridRoles.AutoGenerateColumns = false;
            dataGridRoles.Columns.Clear();

            // Columna índice visible
            dataGridRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "N",
                HeaderText = LanguageService.Current?.T("hdr_dgv_numero") ?? "N°",
                Width = 40,
                Name = "N"
            });

            // Id oculto
            dataGridRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdRol",
                HeaderText = "IdRol",
                Name = "IdRol",
                Visible = false
            });

            // Nombre visible
            dataGridRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Rol",
                HeaderText = LanguageService.Current?.T("hdr_dgv_rol") ?? "Rol",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Name = "Rol"
            });

            dataGridRoles.DataSource = items;
            dataGridRoles.ReadOnly = true;
            dataGridRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }



        private void buttonExitAE_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BuildRolesGridColumns()
        {
            dataGridRoles.AutoGenerateColumns = false;
            dataGridRoles.Columns.Clear();

            // Check: asignado
            var colCheck = new DataGridViewCheckBoxColumn
            {
                HeaderText = LanguageService.Current?.T("hdr_dgv_asignado") ?? "Asignado",
                DataPropertyName = "Asignado",
                Name = "Asignado",
                Width = 80
            };
            // Oculta el Id (lo necesitamos al guardar)
            var colId = new DataGridViewTextBoxColumn
            {
                HeaderText = "IdRol",
                DataPropertyName = "IdRol",
                Name = "IdRol",
                Visible = false
            };
            var colNombre = new DataGridViewTextBoxColumn
            {
                HeaderText = LanguageService.Current?.T("hdr_dgv_rol") ?? "Rol",
                DataPropertyName = "Nombre",
                Name = "Nombre",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridRoles.Columns.AddRange(colCheck, colId, colNombre);
        }





        private void CargarCombos()
        {
            comboBoxIdioma.DataSource = new[]
            {
                new { Id = "es", Nombre = "Español" },
                new { Id = "en", Nombre = "Inglés" }
            };
            comboBoxIdioma.DisplayMember = "Nombre";
            comboBoxIdioma.ValueMember = "Id";

            var roles = _rolService.ListarRoles();
            comboBoxPermisoEdit.DataSource = roles;
            comboBoxPermisoEdit.DisplayMember = "Nombre";
            comboBoxPermisoEdit.ValueMember = "Id";
        }

        private void CargarDatosUsuario()
        {
            textBoxMailEditUser.Text = _usuario.Mail;
            textBoxTelEditUser.Text = _usuario.Telefono.ToString();
            comboBoxIdioma.SelectedValue = _usuario.Idioma;
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarDatosUsuario();

        }

        private void buttonAgregarRol_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPermisoEdit.SelectedIndex < 0)
                {
                    LoggerLogic.Warn($"[EditUserForm] Validación: rol no seleccionado para asignar (Usuario={_usuario.IdUsuario}).");
                    MessageBox.Show(
                        LanguageService.Current?.T("val_rol_requerido") ?? "Elegí un rol de la lista.",
                        LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Guid rolId;
                var val = comboBoxPermisoEdit.SelectedValue;

                if (val is Guid) rolId = (Guid)val;
                else if (val is string && Guid.TryParse((string)val, out var g)) rolId = g;
                else
                {
                    var opt = comboBoxPermisoEdit.SelectedItem as RolOption;
                    if (opt == null) throw new InvalidOperationException("No se pudo obtener el rol seleccionado.");
                    rolId = opt.IdRol;
                }

                var usuarioId = _usuario.IdUsuario;
                _rolService.AsignarUsuarioARol(rolId, usuarioId);
                LoggerLogic.Info($"[EditUserForm] Rol asignado al usuario. Rol={rolId} Usuario={usuarioId}");

                CargarRolesGrid(usuarioId);
                CargarComboRoles(usuarioId);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[EditUserForm] Validación al asignar rol: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EditUserForm] Falla al asignar rol al usuario (Usuario={_usuario.IdUsuario}).", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_agregar_rol") ?? "Error al agregar el rol.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarComboRoles(Guid usuarioId)
        {
            // 1) Traigo todos y filtro los ya asignados
            var todos = _rolService.ListarRoles(); // List<RolCompuesto>
            var asignados = new HashSet<Guid>(_rolService.RolesDeUsuario(usuarioId).Select(r => r.IdRol));

            Guid GetId(Services.RoleService.RolCompuesto r)
                => (Guid)typeof(Services.RoleService.RolCompuesto).GetProperty("Id").GetValue(r);

            var opciones = todos
                .Select(r => new RolOption { IdRol = GetId(r), Nombre = r.Nombre })
                .Where(o => !asignados.Contains(o.IdRol))
                .OrderBy(o => o.Nombre)
                .ToList();

            // 2) Bindeo: limpiar DataSource, configurar Display/Value y recién después asignar la lista
            //    (si invertimos el orden, el combo lanza eventos con valores inconsistentes).
            comboBoxPermisoEdit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPermisoEdit.DataSource = null;
            comboBoxPermisoEdit.DisplayMember = nameof(RolOption.Nombre);
            comboBoxPermisoEdit.ValueMember = nameof(RolOption.IdRol);
            comboBoxPermisoEdit.DataSource = opciones;
            comboBoxPermisoEdit.SelectedIndex = opciones.Count > 0 ? 0 : -1;
        }


        private void dataGridRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridRoles.Rows[e.RowIndex];

            // Intentamos primero el DTO tipado; si el DataSource fue armado de otra forma, caemos al fallback por nombre de columna.
            var item = row.DataBoundItem as RolesServiceLogic.RolPlano;

            Guid rolId;
            string nombreRol;

            if (item != null)
            {
                rolId = item.IdRol;
                nombreRol = item.Nombre;
            }
            else
            {
                object idCell = row.Cells["IdRol"].Value;
                object nomCell = row.Cells["Rol"].Value;
                if (idCell == null) return;

                rolId = (Guid)idCell;
                nombreRol = nomCell == null
                    ? (LanguageService.Current?.T("txt_sin_nombre") ?? "(sin nombre)")
                    : nomCell.ToString();
            }

            var resp = MessageBox.Show(
                string.Format(LanguageService.Current?.T("msg_confirmar_quitar_rol_fmt") ?? "¿Quitar el rol \"{0}\" del usuario?", nombreRol),
                LanguageService.Current?.T("cap_confirmar") ?? "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                var usuarioId = _usuario.IdUsuario;
                _rolService.QuitarUsuarioDeRol(rolId, usuarioId);
                LoggerLogic.Info($"[EditUserForm] Rol quitado del usuario. Rol={rolId} Usuario={usuarioId}");

                CargarRolesGrid(usuarioId);
                CargarComboRoles(usuarioId);
            }
            catch (AppException ex)
            {
                LoggerLogic.Warn($"[EditUserForm] Validación al quitar rol: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[EditUserForm] Falla al quitar rol del usuario (Usuario={_usuario.IdUsuario}).", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_quitar_rol") ?? "No se pudo quitar el rol.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private bool TryGetRolSeleccionado(out Guid rolId)
        {
            rolId = Guid.Empty;

            // 1) El camino feliz: SelectedValue ya es Guid
            var val = comboBoxPermisoEdit.SelectedValue;
            if (val is Guid)
            {
                rolId = (Guid)val;
                return true;
            }

            // 2) A veces llega como string
            var s = val as string;
            Guid g;
            if (!string.IsNullOrEmpty(s) && Guid.TryParse(s, out g))
            {
                rolId = g;
                return true;
            }

            // 3) Por las dudas: DTO
            var opt = comboBoxPermisoEdit.SelectedItem as RolOption;
            if (opt != null)
            {
                rolId = opt.IdRol;
                return true;
            }

            // 4) DataRowView (si alguna vez bindeás un DataTable)
            var drv = comboBoxPermisoEdit.SelectedItem as System.Data.DataRowView;
            if (drv != null && drv.Row.Table.Columns.Contains("IdRol"))
            {
                var obj = drv["IdRol"];
                if (obj is Guid) { rolId = (Guid)obj; return true; }
                var str = obj as string;
                if (!string.IsNullOrEmpty(str) && Guid.TryParse(str, out g))
                { rolId = g; return true; }
            }

            return false;
        }

        private void buttonEditarUser_Click(object sender, EventArgs e)
        {
            var mail = (textBoxMailEditUser.Text ?? "").Trim();
            var pass = textBoxContraseñaEditUser.Text ?? "";
            var telTxt = (textBoxTelEditUser.Text ?? "").Trim();
            var idioma = comboBoxIdioma.SelectedValue?.ToString() ?? "es";

            if (string.IsNullOrWhiteSpace(mail))
            {
                LoggerLogic.Warn($"[EditUserForm] Validación: mail vacío (Usuario={_usuario.IdUsuario}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_mail_requerido") ?? "Ingresá un mail.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!Validaciones.EsMailValido(mail))
            {
                LoggerLogic.Warn($"[EditUserForm] Validación: mail con formato inválido (Usuario={_usuario.IdUsuario}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_mail_invalido") ?? "El mail no tiene un formato válido.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Contraseña vacía = mantener la actual (el campo no se precarga con el hash).

            if (!int.TryParse(telTxt, out var tel))
            {
                LoggerLogic.Warn($"[EditUserForm] Validación: teléfono inválido (Usuario={_usuario.IdUsuario}).");
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "Ingresá un teléfono válido.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // _usuario es el mismo objeto que cachea la grilla: si el guardado
            // falla hay que restaurar los valores para no mostrar datos no persistidos.
            var mailAnterior = _usuario.Mail;
            var telAnterior = _usuario.Telefono;
            var idiomaAnterior = _usuario.Idioma;

            _usuario.Mail = mail;
            _usuario.Telefono = tel;
            _usuario.Idioma = idioma;

            try
            {
                // El service hashea la contraseña nueva; si va vacía, conserva la actual.
                _usuarioService.ActualizarUsuario(_usuario, pass);
                LoggerLogic.Info($"[EditUserForm] Usuario actualizado. Id={_usuario.IdUsuario} Mail='{_usuario.Mail}'");

                var cultureCode = idioma == "en" ? "en-US" : "es-AR";

                if (_usuario.IdUsuario == _loggedUserId)
                {
                    Properties.Settings.Default.CultureCode = cultureCode;
                    Properties.Settings.Default.Save();
                    Application.Restart();
                    return;
                }

                MessageBox.Show(
                    LanguageService.Current?.T("msg_usuario_actualizado") ?? "Usuario actualizado.",
                    LanguageService.Current?.T("cap_ok") ?? "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (AppException ex)
            {
                _usuario.Mail = mailAnterior;
                _usuario.Telefono = telAnterior;
                _usuario.Idioma = idiomaAnterior;
                LoggerLogic.Warn($"[EditUserForm] Validación al actualizar usuario: {ex.MessageKey}");
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                _usuario.Mail = mailAnterior;
                _usuario.Telefono = telAnterior;
                _usuario.Idioma = idiomaAnterior;
                LoggerLogic.Error($"[EditUserForm] Falla al actualizar usuario. Id={_usuario.IdUsuario}", ex);
                MessageBox.Show(
                    LanguageService.Current?.T("err_actualizar_usuario") ?? "No se pudo actualizar el usuario.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
