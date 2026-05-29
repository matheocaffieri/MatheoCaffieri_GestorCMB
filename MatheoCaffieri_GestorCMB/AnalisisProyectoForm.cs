using BL;
using DomainModel;
using Services.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MatheoCaffieri_GestorCMB
{
    public partial class AnalisisProyectoForm : Form
    {
        private readonly Guid   _idProyecto;
        private List<DetalleProyectoMaterial> _materiales;
        private List<DetalleProyectoEmpleado> _empleados;

        public AnalisisProyectoForm(Guid idProyecto, string nombreProyecto)
        {
            InitializeComponent();
            _idProyecto = idProyecto;
            AplicarTraducciones();
            Text        = string.Format(
                LanguageService.Current?.T("cap_analisis_fmt") ?? "Análisis — {0}",
                nombreProyecto);

            // Shown dispara después del primer paint, cuando Dock=Fill ya aplicó
            // el tamaño real — único momento seguro para setear SplitterDistance.
            this.Shown += (_, __) =>
            {
                if (splitCharts.Width > 0)
                    splitCharts.SplitterDistance = splitCharts.Width / 2;
            };
        }

        private void AplicarTraducciones()
        {
            labelFiltrar.Text = LanguageService.Current?.T("lbl_filtrar_por") ?? "Filtrar por:";
            radioDias.Text    = LanguageService.Current?.T("val_dias")        ?? "Días";
            radioMeses.Text   = LanguageService.Current?.T("val_meses")       ?? "Meses";
            radioAnios.Text   = LanguageService.Current?.T("val_anios")       ?? "Años";
        }

        // ── Load ────────────────────────────────────────────────────────────

        private void AnalisisProyectoForm_Load(object sender, EventArgs e)
        {
            try
            {
                _materiales = new DetalleMaterialBL().GetAll(_idProyecto);
                _empleados  = new DetalleEmpleadoBL().GetAll(_idProyecto);
            }
            catch
            {
                _materiales = new List<DetalleProyectoMaterial>();
                _empleados  = new List<DetalleProyectoEmpleado>();
            }

            radioMeses.Checked = true; // dispara ActualizarGraficos via CheckedChanged
        }

        // ── Filtro helpers ──────────────────────────────────────────────────

        private string Filtro =>
            radioDias.Checked  ? "Días"  :
            radioAnios.Checked ? "Años"  : "Meses";

        private DateTime GetPeriodDate(DateTime fecha)
        {
            switch (Filtro)
            {
                case "Días":  return fecha.Date;
                case "Años":  return new DateTime(fecha.Year, 1, 1);
                default:      return new DateTime(fecha.Year, fecha.Month, 1);
            }
        }

        private string GetPeriodLabel(DateTime d)
        {
            switch (Filtro)
            {
                case "Días":  return d.ToString("dd/MM/yy");
                case "Años":  return d.Year.ToString();
                default:      return d.ToString("MMM yyyy");
            }
        }

        // ── Update ──────────────────────────────────────────────────────────

        private void ActualizarGraficos()
        {
            if (_materiales == null) return;

            var matPeriods = _materiales
                .GroupBy(m => GetPeriodDate(m.FechaIngresoMaterial))
                .OrderBy(g => g.Key)
                .Select(g => new PeriodData(
                    g.Key,
                    GetPeriodLabel(g.Key),
                    (double)g.Sum(m => m.Material.CostoPorUnidad * m.Cantidad)))
                .ToList();

            var empPeriods = _empleados
                .GroupBy(e => GetPeriodDate(e.FechaIngresoEmpleado))
                .OrderBy(g => g.Key)
                .Select(g => new PeriodData(
                    g.Key,
                    GetPeriodLabel(g.Key),
                    (double)g.Sum(e => e.Empleado.Sueldo)))
                .ToList();

            var allLabels = matPeriods.Select(p => p.Label)
                .Union(empPeriods.Select(p => p.Label))
                .ToList(); // mantenemos orden ya que ambas están ordenadas por fecha

            // reconstruimos en orden cronológico real
            var allPeriods = matPeriods.Select(p => (p.Date, p.Label))
                .Union(empPeriods.Select(p => (p.Date, p.Label)))
                .OrderBy(p => p.Date)
                .ToList();

            BuildComprasChart(matPeriods);
            BuildCostosChart(allPeriods, matPeriods, empPeriods);
        }

        // ── Chart 1: Análisis de compras (columnas) ─────────────────────────

        private void BuildComprasChart(List<PeriodData> data)
        {
            chartCompras.Series.Clear();
            chartCompras.Titles.Clear();
            chartCompras.Legends.Clear();

            chartCompras.Titles.Add(new Title(LanguageService.Current?.T("hdr_analisis_compras") ?? "Análisis de compras")
            {
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Docking   = Docking.Top
            });

            var ca = chartCompras.ChartAreas[0];
            StyleChartArea(ca);

            var s = new Series("Materiales")
            {
                ChartType           = SeriesChartType.Column,
                Color               = Color.FromArgb(100, 160, 230),
                IsValueShownAsLabel = true,
                LabelFormat         = "N0",
                Font                = new Font("Segoe UI", 7.5f),
                BorderColor         = Color.FromArgb(70, 130, 200),
                BorderWidth         = 1
            };

            foreach (var p in data)
                s.Points.AddXY(p.Label, p.Total);

            chartCompras.Series.Add(s);
            ca.RecalculateAxesScale();
        }

        // ── Chart 2: Costo del proyecto (línea) ─────────────────────────────

        private void BuildCostosChart(
            List<(DateTime Date, string Label)> periods,
            List<PeriodData> mat,
            List<PeriodData> emp)
        {
            chartCostos.Series.Clear();
            chartCostos.Titles.Clear();
            chartCostos.Legends.Clear();

            chartCostos.Titles.Add(new Title(LanguageService.Current?.T("hdr_costo_proyecto") ?? "Costo del proyecto")
            {
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Docking   = Docking.Top
            });

            var ca = chartCostos.ChartAreas[0];
            StyleChartArea(ca);

            var s = new Series("Total")
            {
                ChartType           = SeriesChartType.Line,
                Color               = Color.FromArgb(65, 105, 225),
                BorderWidth         = 3,
                MarkerStyle         = MarkerStyle.Circle,
                MarkerSize          = 9,
                MarkerColor         = Color.FromArgb(65, 105, 225),
                IsValueShownAsLabel = true,
                LabelFormat         = "N0",
                Font                = new Font("Segoe UI", 7.5f),
                LabelBackColor      = Color.FromArgb(255, 255, 150),
                LabelBorderColor    = Color.FromArgb(180, 180, 0),
                LabelBorderWidth    = 1
            };

            foreach (var p in periods)
            {
                double matVal = mat.FirstOrDefault(m => m.Label == p.Label)?.Total ?? 0;
                double empVal = emp.FirstOrDefault(e => e.Label == p.Label)?.Total ?? 0;
                s.Points.AddXY(p.Label, matVal + empVal);
            }

            chartCostos.Series.Add(s);
            ca.RecalculateAxesScale();
        }

        // ── Shared chart area styling ────────────────────────────────────────

        private static void StyleChartArea(ChartArea ca)
        {
            ca.BackColor                     = Color.White;
            ca.AxisX.MajorGrid.LineColor     = Color.FromArgb(220, 220, 220);
            ca.AxisY.MajorGrid.LineColor     = Color.FromArgb(220, 220, 220);
            ca.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            ca.AxisX.LineColor               = Color.FromArgb(180, 180, 180);
            ca.AxisY.LineColor               = Color.FromArgb(180, 180, 180);
            ca.AxisX.LabelStyle.Font         = new Font("Segoe UI", 8f);
            ca.AxisY.LabelStyle.Font         = new Font("Segoe UI", 8f);
            ca.AxisY.LabelStyle.Format       = "N0";
            ca.AxisX.LabelStyle.IsEndLabelVisible = true;
        }

        // ── Events ──────────────────────────────────────────────────────────

        private void OnFiltroChanged(object sender, EventArgs e)
        {
            if (_materiales != null)
                ActualizarGraficos();
        }

        // ── Helper DTO ──────────────────────────────────────────────────────

        private class PeriodData
        {
            public DateTime Date  { get; }
            public string   Label { get; }
            public double   Total { get; }
            public PeriodData(DateTime date, string label, double total)
            { Date = date; Label = label; Total = total; }
        }
    }
}
