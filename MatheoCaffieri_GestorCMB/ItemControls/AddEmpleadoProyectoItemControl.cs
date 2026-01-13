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
    public partial class AddEmpleadoProyectoItemControl : UserControl
    {
        private const int MAX_PROYECTOS_ACTIVOS = 3;

        // === datos clave ===
        public Guid IdEmpleado { get; private set; }
        public int ProyectosActivos { get; private set; } = 0;

        // evento para que el FORM haga la operación real
        public event EventHandler<AgregarEmpleadoEventArgs> AgregarClick;

        public AddEmpleadoProyectoItemControl()
        {
            InitializeComponent();
            RefrescarUI();
        }

        // ÚNICO LABEL (como aclaraste)
        public string InfoEmpleado
        {
            get => labelInfoEmpleado.Text;
            set => labelInfoEmpleado.Text = value ?? string.Empty;
        }

        // Bind como en materiales
        public void Bind(Guid idEmpleado, string nombre, string apellido, int dni, decimal sueldo, int proyectosActivos)
        {
            IdEmpleado = idEmpleado;
            ProyectosActivos = Math.Max(0, proyectosActivos);

            InfoEmpleado =
                $"{nombre} {apellido} | {dni} | ${sueldo:N0} | Proyectos activos: {ProyectosActivos}";

            RefrescarUI();
        }

        private void RefrescarUI()
        {
            var sinCupo = ProyectosActivos >= MAX_PROYECTOS_ACTIVOS;
            buttonAgregarEmpleado.Enabled = !sinCupo;
            buttonAgregarEmpleado.Text = sinCupo ? "Sin cupo" : "Agregar";
        }

        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {
            if (IdEmpleado == Guid.Empty) return;

            if (ProyectosActivos >= MAX_PROYECTOS_ACTIVOS)
            {
                MessageBox.Show("El empleado ya tiene 3 proyectos activos. No se puede agregar.",
                    "Límite alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AgregarClick?.Invoke(this, new AgregarEmpleadoEventArgs(IdEmpleado));


        }
    }


    public sealed class AgregarEmpleadoEventArgs : EventArgs
    {
        public Guid IdEmpleado { get; }
        public AgregarEmpleadoEventArgs(Guid idEmpleado) => IdEmpleado = idEmpleado;
    }
}
