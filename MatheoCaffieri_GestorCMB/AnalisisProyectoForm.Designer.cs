using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MatheoCaffieri_GestorCMB
{
    partial class AnalisisProyectoForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelFiltro  = new System.Windows.Forms.Panel();
            this.labelFiltrar = new System.Windows.Forms.Label();
            this.radioDias    = new System.Windows.Forms.RadioButton();
            this.radioMeses   = new System.Windows.Forms.RadioButton();
            this.radioAnios   = new System.Windows.Forms.RadioButton();
            this.splitCharts  = new System.Windows.Forms.SplitContainer();
            this.chartCompras = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCostos  = new System.Windows.Forms.DataVisualization.Charting.Chart();

            // Solo los Charts necesitan ISupportInitialize
            ((System.ComponentModel.ISupportInitialize)(this.chartCompras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCostos)).BeginInit();
            this.SuspendLayout();

            // ── panelFiltro ────────────────────────────────────────────────
            this.panelFiltro.Dock      = System.Windows.Forms.DockStyle.Top;
            this.panelFiltro.Height    = 46;
            this.panelFiltro.BackColor = System.Drawing.Color.FromArgb(247, 248, 250);
            this.panelFiltro.Controls.Add(this.labelFiltrar);
            this.panelFiltro.Controls.Add(this.radioDias);
            this.panelFiltro.Controls.Add(this.radioMeses);
            this.panelFiltro.Controls.Add(this.radioAnios);

            // ── labelFiltrar ───────────────────────────────────────────────
            this.labelFiltrar.AutoSize  = true;
            this.labelFiltrar.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.labelFiltrar.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.labelFiltrar.Location  = new System.Drawing.Point(18, 14);
            this.labelFiltrar.Text      = "Filtrar por:";

            // ── radioDias ──────────────────────────────────────────────────
            this.radioDias.AutoSize  = true;
            this.radioDias.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.radioDias.Location  = new System.Drawing.Point(108, 13);
            this.radioDias.Text      = "Días";
            this.radioDias.CheckedChanged += new System.EventHandler(this.OnFiltroChanged);

            // ── radioMeses ─────────────────────────────────────────────────
            this.radioMeses.AutoSize  = true;
            this.radioMeses.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.radioMeses.Location  = new System.Drawing.Point(175, 13);
            this.radioMeses.Text      = "Meses";
            this.radioMeses.CheckedChanged += new System.EventHandler(this.OnFiltroChanged);

            // ── radioAnios ─────────────────────────────────────────────────
            this.radioAnios.AutoSize  = true;
            this.radioAnios.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.radioAnios.Location  = new System.Drawing.Point(249, 13);
            this.radioAnios.Text      = "Años";
            this.radioAnios.CheckedChanged += new System.EventHandler(this.OnFiltroChanged);

            // ── splitCharts — sin ISupportInitialize, sin Panel1/2MinSize ──
            // (Panel1MinSize, Panel2MinSize y SplitterDistance se asignan en Load,
            //  cuando el control ya tiene tamaño real y la validación no falla.)
            this.splitCharts.Dock        = System.Windows.Forms.DockStyle.Fill;
            this.splitCharts.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitCharts.SplitterWidth = 6;
            this.splitCharts.BackColor   = System.Drawing.Color.FromArgb(225, 228, 232);

            this.splitCharts.Panel1.Padding = new System.Windows.Forms.Padding(8, 8, 4, 8);
            this.splitCharts.Panel1.Controls.Add(this.chartCompras);

            this.splitCharts.Panel2.Padding = new System.Windows.Forms.Padding(4, 8, 8, 8);
            this.splitCharts.Panel2.Controls.Add(this.chartCostos);

            // ── chartCompras ───────────────────────────────────────────────
            var caCompras = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            caCompras.Name      = "Default";
            caCompras.BackColor = System.Drawing.Color.White;
            this.chartCompras.ChartAreas.Add(caCompras);
            this.chartCompras.Dock            = System.Windows.Forms.DockStyle.Fill;
            this.chartCompras.BackColor       = System.Drawing.Color.White;
            this.chartCompras.BorderlineColor = System.Drawing.Color.FromArgb(210, 212, 216);
            this.chartCompras.BorderlineDashStyle =
                System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            // ── chartCostos ────────────────────────────────────────────────
            var caCostos = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            caCostos.Name      = "Default";
            caCostos.BackColor = System.Drawing.Color.White;
            this.chartCostos.ChartAreas.Add(caCostos);
            this.chartCostos.Dock            = System.Windows.Forms.DockStyle.Fill;
            this.chartCostos.BackColor       = System.Drawing.Color.White;
            this.chartCostos.BorderlineColor = System.Drawing.Color.FromArgb(210, 212, 216);
            this.chartCostos.BorderlineDashStyle =
                System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            // ── Form ───────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.ClientSize          = new System.Drawing.Size(1020, 560);
            this.Font                = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize         = new System.Drawing.Size(700, 420);
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text                = "Análisis del proyecto";

            // splitCharts (Fill) primero, panelFiltro (Top) segundo
            this.Controls.Add(this.splitCharts);
            this.Controls.Add(this.panelFiltro);

            this.Load += new System.EventHandler(this.AnalisisProyectoForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.chartCompras)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCostos)).EndInit();
            this.ResumeLayout(false);
        }

        // ── Fields ─────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel           panelFiltro;
        private System.Windows.Forms.Label           labelFiltrar;
        private System.Windows.Forms.RadioButton     radioDias;
        private System.Windows.Forms.RadioButton     radioMeses;
        private System.Windows.Forms.RadioButton     radioAnios;
        private System.Windows.Forms.SplitContainer  splitCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCompras;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCostos;
    }
}
