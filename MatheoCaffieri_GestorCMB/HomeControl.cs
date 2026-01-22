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
using BL.LoginBL;
using Interfaces.LoginInterfaces;


namespace MatheoCaffieri_GestorCMB
{
    public partial class HomeControl : UserControl
    {
        private MainForm mainForm;

        public HomeControl(MainForm mainForm)
        {
            InitializeComponent();
            this.Load -= HomeControl_Load;
            this.Load += HomeControl_Load;

            AplicarAccesosHome();

            this.mainForm = mainForm;

            // Mostrar el nombre del usuario en el linkLabel
            linkLabelUser.Text = string.IsNullOrWhiteSpace(UserSession.UserDisplayName)
                ? "User"
                : UserSession.UserDisplayName;

            // Aseguro el handler por si no lo agregaste desde el diseñador
            linkLabelUser.DoubleClick -= linkLabelUser_DoubleClick;
            linkLabelUser.DoubleClick += linkLabelUser_DoubleClick;
        }


        private void linkLabelUser_DoubleClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
            "¿Querés cerrar sesión?",
            "Cerrar sesión",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Abrir el login en un nuevo hilo de interfaz
                    System.Threading.Thread t = new System.Threading.Thread(() =>
                    {
                        Application.Run(new LoginForm());
                    });
                    t.SetApartmentState(System.Threading.ApartmentState.STA);
                    t.Start();

                    // Cerrar la ventana actual (MainForm)
                    var host = mainForm ?? this.FindForm();
                    if (host != null)
                        host.Invoke(new Action(() => host.Close()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo cerrar sesión: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void butMainProyectos_Click(object sender, EventArgs e)
        {
            VerProyectosControl verProyectosControl = new VerProyectosControl(mainForm);
            mainForm.addUserControl(verProyectosControl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VerInventarioControl verInventarioControl = new VerInventarioControl();
            mainForm.addUserControl(verInventarioControl);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VerEmpleadosControl verEmpleadosControl = new VerEmpleadosControl();
            mainForm.addUserControl(verEmpleadosControl);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClientesControl clientesControl = new ClientesControl();
            mainForm.addUserControl(clientesControl);
        }

        private void HomeControl_Load(object sender, EventArgs e)
        {
            AplicarAccesosHome();
        }

        private void AplicarAccesosHome()
        {
            BloquearSiNoTienePermiso(butMainProyectos, TipoPermiso.VER_PROYECTOS);
            BloquearSiNoTienePermiso(button2, TipoPermiso.VER_INVENTARIO);
            BloquearSiNoTienePermiso(button3, TipoPermiso.VER_EMPLEADOS);
            BloquearSiNoTienePermiso(button4, TipoPermiso.GESTIONAR_CLIENTES);
        }

        private struct ButtonStyleSnapshot
        {
            public Color Back;
            public Color Fore;
            public FlatStyle Flat;
            public bool UseVisual;
        }

        private void GuardarEstiloOriginal(Button btn)
        {
            if (btn.Tag is ButtonStyleSnapshot) return;

            btn.Tag = new ButtonStyleSnapshot
            {
                Back = btn.BackColor,
                Fore = btn.ForeColor,
                Flat = btn.FlatStyle,
                UseVisual = btn.UseVisualStyleBackColor
            };
        }

        private void RestaurarEstiloOriginal(Button btn)
        {
            if (btn.Tag is ButtonStyleSnapshot s)
            {
                btn.BackColor = s.Back;
                btn.ForeColor = s.Fore;
                btn.FlatStyle = s.Flat;
                btn.UseVisualStyleBackColor = s.UseVisual;
            }
        }

        private static Color Oscurecer(Color c, float factor = 0.88f) // 0.85–0.90 queda bien
        {
            int r = (int)(c.R * factor);
            int g = (int)(c.G * factor);
            int b = (int)(c.B * factor);

            r = r < 0 ? 0 : (r > 255 ? 255 : r);
            g = g < 0 ? 0 : (g > 255 ? 255 : g);
            b = b < 0 ? 0 : (b > 255 ? 255 : b);

            return Color.FromArgb(c.A, r, g, b);
        }

        private void BloquearSiNoTienePermiso(Button btn, TipoPermiso permiso)
        {
            GuardarEstiloOriginal(btn);

            bool allowed = SessionManager.Instance.TienePermiso(permiso);

            if (allowed)
            {
                btn.Enabled = true;
                btn.Cursor = Cursors.Hand;
                RestaurarEstiloOriginal(btn);
                return;
            }

            // Sin permiso: “disabled” pero manteniendo el color original, apenas más oscuro
            btn.Enabled = false;
            btn.Cursor = Cursors.No;

            // Importante para que WinForms no te lo pinte gris/blanco por Visual Styles
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;

            if (btn.Tag is ButtonStyleSnapshot s)
            {
                btn.BackColor = Oscurecer(s.Back, 0.88f);
                btn.ForeColor = SystemColors.GrayText;
                btn.FlatAppearance.BorderColor = Oscurecer(s.Back, 0.75f);
                btn.FlatAppearance.BorderSize = 1;
            }
        }


    }
}
