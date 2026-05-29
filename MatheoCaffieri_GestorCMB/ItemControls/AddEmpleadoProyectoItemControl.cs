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

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class AddEmpleadoProyectoItemControl : UserControl
    {
        private const int MAX_PROYECTOS_ACTIVOS = 3;
        private decimal _sueldo;

        public Guid IdEmpleado { get; private set; }
        public int ProyectosActivos { get; private set; } = 0;

        public event EventHandler<AgregarEmpleadoEventArgs> AgregarClick;

        public AddEmpleadoProyectoItemControl()
        {
            InitializeComponent();
            RefrescarUI();
        }

        public string InfoEmpleado
        {
            get => labelInfoEmpleado.Text;
            set => labelInfoEmpleado.Text = value ?? string.Empty;
        }

        public void Bind(Guid idEmpleado, string nombre, string apellido, int dni, decimal sueldo, int proyectosActivos)
        {
            IdEmpleado = idEmpleado;
            _sueldo = sueldo;
            ProyectosActivos = Math.Max(0, proyectosActivos);

            InfoEmpleado =
                $"{nombre} {apellido} | {dni} | ${sueldo:N0} | Proyectos activos: {ProyectosActivos}";

            RefrescarUI();
        }

        private void RefrescarUI()
        {
            var sinCupo = ProyectosActivos >= MAX_PROYECTOS_ACTIVOS;
            buttonAgregarEmpleado.Enabled = !sinCupo;
            buttonAgregarEmpleado.Text = sinCupo
                ? (LanguageService.Current?.T("val_sin_cupo")      ?? "Sin cupo")
                : (LanguageService.Current?.T("btn_agregar_simple") ?? "Agregar");
        }

        private void buttonAgregarEmpleado_Click(object sender, EventArgs e)
        {
            if (IdEmpleado == Guid.Empty) return;

            if (ProyectosActivos >= MAX_PROYECTOS_ACTIVOS)
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_empleado_max_proyectos") ?? "El empleado ya tiene 3 proyectos activos. No se puede agregar.",
                    LanguageService.Current?.T("cap_limite_alcanzado") ?? "Límite alcanzado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AgregarClick?.Invoke(this, new AgregarEmpleadoEventArgs(IdEmpleado, _sueldo));
        }
    }


    public sealed class AgregarEmpleadoEventArgs : EventArgs
    {
        public Guid IdEmpleado { get; }
        public decimal Sueldo { get; }

        public AgregarEmpleadoEventArgs(Guid idEmpleado, decimal sueldo)
        {
            IdEmpleado = idEmpleado;
            Sueldo = sueldo;
        }
    }
}
