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
    public partial class ProyectoItemControl : UserControl
    {


        private MainForm mainForm;
        private Proyecto _proyecto;

        public string NumProyecto
        {
            get => labelNumProyecto.Text;
            set => labelNumProyecto.Text = value ?? "Proyecto #N/A";
        }

        public string DescripcionProyecto
        {
            get => labelDescripcionProyecto.Text;
            set => labelDescripcionProyecto.Text = value ?? "Sin descripción";
        }

        public string NombreCliente
        {
            get => labelNombreCliente.Text;
            set => labelNombreCliente.Text = value ?? "Desconocido";
        }

        public string FechaInicio
        {
            get => labelFechaInicio.Text;
            set => labelFechaInicio.Text = value ?? "Fecha no disponible";
        }

        public string EstadoProyecto
        {
            get => labelEstadoProyecto.Text;
            set => labelEstadoProyecto.Text = value ?? "Estado desconocido";
        }

        public string UbicacionProyecto
        {
            get => labelUbicacionProyecto.Text;
            set => labelUbicacionProyecto.Text = value ?? "Ubicación no especificada";
        }

        public Proyecto ProyectoData
        {
            get => _proyecto;
            set => _proyecto = value;
        }

        public ProyectoItemControl(MainForm mainForm, Proyecto proyecto)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this._proyecto = proyecto;


            // Cargar datos del proyecto en la interfaz
            NumProyecto = "Proyecto #" + proyecto.IdProyecto;
            DescripcionProyecto = proyecto.Descripcion;
            NombreCliente = proyecto.Cliente?.NombreContacto ?? "Cliente desconocido";
            FechaInicio = proyecto.FechaInicio.ToString("dd/MM/yyyy") ?? "Sin fecha";
            EstadoProyecto = proyecto.Estado.ToString() ?? "Estado no disponible";
            UbicacionProyecto = proyecto.Ubicacion ?? "Ubicación desconocida";
        }

        private void linkLabelVerDetalles_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_proyecto == null)
            {
                MessageBox.Show("No se ha asignado información del proyecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cambiar la vista en el MainForm
            mainForm.addUserControl(new DetalleProyectoControl(mainForm, _proyecto));
        }
    }

}

