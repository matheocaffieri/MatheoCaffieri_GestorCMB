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
    public partial class HomeControl : UserControl
    {
        private MainForm mainForm;

        public HomeControl(MainForm mainForm)
        {
            InitializeComponent();
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
    }
}
