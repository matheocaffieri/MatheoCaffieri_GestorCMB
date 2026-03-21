using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainModel;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    public partial class EmpleadosItemControl : UserControl
    {
        public Empleado Empleado { get; private set; }

        public event Action<Empleado, bool> ActiveChanged;
        public event Action<Empleado> EditRequested;   // <-- NUEVO

        public EmpleadosItemControl()
        {
            InitializeComponent();

            // Switch
            SwitchHabilitarEmpleado.ToggleChanged += (_, __) =>
            {
                if (Empleado == null) return;

                bool nuevoEstado = SwitchHabilitarEmpleado.IsOn;
                Empleado.IsActive = nuevoEstado;
                ActiveChanged?.Invoke(Empleado, nuevoEstado);
            };

            // Lápiz (editar)
            buttonEditarEmpleado.Click += (_, __) =>
            {
                if (Empleado == null) return;
                EditRequested?.Invoke(Empleado);
            };
        }

        public void Bind(Empleado e)
        {
            Empleado = e;

            labelInfoEmpleado.Text =
                $"{e.Nombre} {e.Apellido} | DNI: {e.NroDocumento} | ${e.Sueldo} | Proyectos activos: {e.CantidadProyectosActivos}";

            SwitchHabilitarEmpleado.IsOn = e.IsActive;
        }
    }
}
