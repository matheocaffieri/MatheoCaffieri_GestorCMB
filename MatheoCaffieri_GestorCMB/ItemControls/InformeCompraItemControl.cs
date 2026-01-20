using DomainModel;
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
    public partial class InformeCompraItemControl : UserControl
    {
        private string _numeroProyecto;
        private string _nombreProyecto;

        [Browsable(false)]
        public Guid IdProyecto { get; private set; }

        // Guarda el último informe generado o el informe “activo”
        [Browsable(false)]
        public Guid? IdInformeCompra { get; private set; }

        public event EventHandler<Guid> AgregarCompraClicked;
        public event EventHandler<Guid> EliminarClicked;

        public InformeCompraItemControl()
        {
            InitializeComponent();

            // Recomendado para que se vea bien el listado
            textBoxItemsFaltantes.Multiline = true;
            textBoxItemsFaltantes.ScrollBars = ScrollBars.Vertical;
            textBoxItemsFaltantes.ReadOnly = true;

            buttonAgregarCompra.Click += (s, e) => AgregarCompraClicked?.Invoke(this, IdProyecto);
            buttonEliminar.Click += (s, e) => EliminarClicked?.Invoke(this, IdProyecto);
        }

        [Category("Custom Props")]
        public string NumeroProyecto
        {
            get => _numeroProyecto;
            set
            {
                _numeroProyecto = value;
                labelNumeroProyecto.Text = value;
            }
        }

        [Category("Custom Props")]
        public string NombreProyecto
        {
            get => _nombreProyecto;
            set
            {
                _nombreProyecto = value;
                labelNombreProyecto.Text = value;
            }
        }

        public void BindProyecto(Guid idProyecto, string numero, string nombre)
        {
            IdProyecto = idProyecto;
            NumeroProyecto = numero;
            NombreProyecto = nombre;
        }

        public void SetInforme(Guid? idInformeCompra)
        {
            IdInformeCompra = idInformeCompra;
        }

        public void SetFaltantes(List<MaterialFaltante> faltantes)
        {
            if (faltantes == null || faltantes.Count == 0)
            {
                textBoxItemsFaltantes.Text = "Sin materiales faltantes.";
                buttonAgregarCompra.Enabled = false;
                return;
            }

            buttonAgregarCompra.Enabled = true;

            // Orden opcional para que quede prolijo
            var ordered = faltantes
                .OrderBy(x => x.TipoMaterialFaltante)
                .ThenBy(x => x.DescripcionArticuloFaltante)
                .ToList();

            var sb = new StringBuilder();
            foreach (var f in ordered)
            {
                var desc = string.IsNullOrWhiteSpace(f.DescripcionArticuloFaltante) ? "(sin descripción)" : f.DescripcionArticuloFaltante;
                var tipo = string.IsNullOrWhiteSpace(f.TipoMaterialFaltante) ? "-" : f.TipoMaterialFaltante;
                var unidad = string.IsNullOrWhiteSpace(f.TipoUnidadMaterialFaltante) ? "" : f.TipoUnidadMaterialFaltante;

                sb.AppendLine($"• {f.CantidadFaltante} {unidad} - {desc} ({tipo})");
            }

            textBoxItemsFaltantes.Text = sb.ToString();
        }
    }
}
