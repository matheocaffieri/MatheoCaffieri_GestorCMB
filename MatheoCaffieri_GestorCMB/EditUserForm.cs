using BL.AccessBL;
using BL.LoginBL;
using DomainModel.Exceptions;
using DomainModel.Login;
using Services.Language;
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


        public EditUserForm(Usuario usuario, RolesServiceLogic rolService, UsuarioService usuarioService, Guid loggedUserId)
        {
            InitializeComponent();
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _loggedUserId = loggedUserId;

            textBoxMailEditUser.Text = _usuario.Mail;
            textBoxContraseñaEditUser.Text = _usuario.Contraseña;
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
                HeaderText = "N°",
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
                HeaderText = "Rol",
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
                HeaderText = "Asignado",
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
                HeaderText = "Rol",
                DataPropertyName = "Nombre",
                Name = "Nombre",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            dataGridRoles.Columns.AddRange(colCheck, colId, colNombre);
        }





        private void CargarCombos()
        {
            // ejemplo: cargar idioma
            comboBoxIdioma.DataSource = new[]
            {
            new { Id = "es", Nombre = "Español" },
            new { Id = "en", Nombre = "Inglés" }
        };
            comboBoxIdioma.DisplayMember = "Nombre";
            comboBoxIdioma.ValueMember = "Id";

            // ejemplo: cargar roles
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
            // comboBoxPermisoEdit.SelectedValue = _usuario.;
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

                var usuarioId = _usuario.IdUsuario; // usa el nombre real de tu propiedad
                _rolService.AsignarUsuarioARol(rolId, usuarioId);

                // Refrescos
                CargarRolesGrid(usuarioId);
                CargarComboRoles(usuarioId);
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
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

            // 2) ORDEN correcto de bindeo
            comboBoxPermisoEdit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPermisoEdit.DataSource = null;                        // <- limpia
            comboBoxPermisoEdit.DisplayMember = nameof(RolOption.Nombre);    // <- set antes
            comboBoxPermisoEdit.ValueMember = nameof(RolOption.IdRol);     // <- set antes
            comboBoxPermisoEdit.DataSource = opciones;                    // <- ahora sí
            comboBoxPermisoEdit.SelectedIndex = opciones.Count > 0 ? 0 : -1; // o -1 si querés
        }


        private void dataGridRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignorar encabezados
            if (e.RowIndex < 0) return;

            var row = dataGridRoles.Rows[e.RowIndex];

            // Intentamos obtener el item tipado (DTO que usás para el grid)
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
                // Fallback por si el DataSource no es RolPlano (usa nombres de columnas)
                object idCell = row.Cells["IdRol"].Value;
                object nomCell = row.Cells["Rol"].Value;
                if (idCell == null) return;

                rolId = (Guid)idCell;
                nombreRol = nomCell == null ? "(sin nombre)" : nomCell.ToString();
            }

            var resp = MessageBox.Show(
                string.Format("¿Quitar el rol \"{0}\" del usuario?", nombreRol),
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp != DialogResult.Yes) return;

            try
            {
                var usuarioId = _usuario.IdUsuario; // ajustá el nombre si difiere
                _rolService.QuitarUsuarioDeRol(rolId, usuarioId);

                // Refrescos
                CargarRolesGrid(usuarioId);
                CargarComboRoles(usuarioId); // si usás combo con “no asignados”
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
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
                MessageBox.Show(
                    LanguageService.Current?.T("val_mail_requerido") ?? "Ingresá un mail.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_contrasena_requerida") ?? "Ingresá una contraseña.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(telTxt, out var tel))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("val_telefono_invalido") ?? "Ingresá un teléfono válido.",
                    LanguageService.Current?.T("cap_aviso") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _usuario.Mail = mail;
            _usuario.Contraseña = pass;
            _usuario.Telefono = tel;
            _usuario.Idioma = idioma;

            try
            {
                _usuarioService.ActualizarUsuario(_usuario);

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
                Close();
            }
            catch (AppException ex)
            {
                var msg = LanguageService.Current?.T(ex.MessageKey) ?? ex.Message;
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_actualizar_usuario") ?? "No se pudo actualizar el usuario.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
