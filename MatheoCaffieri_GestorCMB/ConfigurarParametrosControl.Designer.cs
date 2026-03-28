namespace MatheoCaffieri_GestorCMB
{
    partial class ConfigurarParametrosControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurarParametrosControl));
            this.labelTitulo = new System.Windows.Forms.Label();
            this.groupBoxParametros = new System.Windows.Forms.GroupBox();
            this.labelEmpleados = new System.Windows.Forms.Label();
            this.numMargenEmpleados = new System.Windows.Forms.NumericUpDown();
            this.labelPorcentEmpleados = new System.Windows.Forms.Label();
            this.labelMateriales = new System.Windows.Forms.Label();
            this.numMargenMateriales = new System.Windows.Forms.NumericUpDown();
            this.labelPorcentMateriales = new System.Windows.Forms.Label();
            this.labelUtilidad = new System.Windows.Forms.Label();
            this.numUtilidadEmpresa = new System.Windows.Forms.NumericUpDown();
            this.labelPorcentUtilidad = new System.Windows.Forms.Label();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBoxParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMargenEmpleados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMargenMateriales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUtilidadEmpresa)).BeginInit();
            this.SuspendLayout();
            //
            // labelTitulo
            //
            resources.ApplyResources(this.labelTitulo, "labelTitulo");
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(30, 20);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(400, 35);
            //
            // groupBoxParametros
            //
            this.groupBoxParametros.Controls.Add(this.labelEmpleados);
            this.groupBoxParametros.Controls.Add(this.numMargenEmpleados);
            this.groupBoxParametros.Controls.Add(this.labelPorcentEmpleados);
            this.groupBoxParametros.Controls.Add(this.labelMateriales);
            this.groupBoxParametros.Controls.Add(this.numMargenMateriales);
            this.groupBoxParametros.Controls.Add(this.labelPorcentMateriales);
            this.groupBoxParametros.Controls.Add(this.labelUtilidad);
            this.groupBoxParametros.Controls.Add(this.numUtilidadEmpresa);
            this.groupBoxParametros.Controls.Add(this.labelPorcentUtilidad);
            resources.ApplyResources(this.groupBoxParametros, "groupBoxParametros");
            this.groupBoxParametros.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxParametros.Location = new System.Drawing.Point(30, 70);
            this.groupBoxParametros.Name = "groupBoxParametros";
            this.groupBoxParametros.Size = new System.Drawing.Size(500, 200);
            this.groupBoxParametros.TabStop = false;
            //
            // labelEmpleados
            //
            resources.ApplyResources(this.labelEmpleados, "labelEmpleados");
            this.labelEmpleados.Location = new System.Drawing.Point(20, 35);
            this.labelEmpleados.Name = "labelEmpleados";
            this.labelEmpleados.Size = new System.Drawing.Size(230, 25);
            this.labelEmpleados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // numMargenEmpleados
            //
            this.numMargenEmpleados.DecimalPlaces = 2;
            this.numMargenEmpleados.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMargenEmpleados.Location = new System.Drawing.Point(260, 35);
            this.numMargenEmpleados.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numMargenEmpleados.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numMargenEmpleados.Name = "numMargenEmpleados";
            this.numMargenEmpleados.Size = new System.Drawing.Size(80, 23);
            //
            // labelPorcentEmpleados
            //
            this.labelPorcentEmpleados.Location = new System.Drawing.Point(345, 35);
            this.labelPorcentEmpleados.Name = "labelPorcentEmpleados";
            this.labelPorcentEmpleados.Size = new System.Drawing.Size(20, 23);
            this.labelPorcentEmpleados.Text = "%";
            this.labelPorcentEmpleados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // labelMateriales
            //
            resources.ApplyResources(this.labelMateriales, "labelMateriales");
            this.labelMateriales.Location = new System.Drawing.Point(20, 80);
            this.labelMateriales.Name = "labelMateriales";
            this.labelMateriales.Size = new System.Drawing.Size(230, 25);
            this.labelMateriales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // numMargenMateriales
            //
            this.numMargenMateriales.DecimalPlaces = 2;
            this.numMargenMateriales.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMargenMateriales.Location = new System.Drawing.Point(260, 80);
            this.numMargenMateriales.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numMargenMateriales.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numMargenMateriales.Name = "numMargenMateriales";
            this.numMargenMateriales.Size = new System.Drawing.Size(80, 23);
            //
            // labelPorcentMateriales
            //
            this.labelPorcentMateriales.Location = new System.Drawing.Point(345, 80);
            this.labelPorcentMateriales.Name = "labelPorcentMateriales";
            this.labelPorcentMateriales.Size = new System.Drawing.Size(20, 23);
            this.labelPorcentMateriales.Text = "%";
            this.labelPorcentMateriales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // labelUtilidad
            //
            resources.ApplyResources(this.labelUtilidad, "labelUtilidad");
            this.labelUtilidad.Location = new System.Drawing.Point(20, 125);
            this.labelUtilidad.Name = "labelUtilidad";
            this.labelUtilidad.Size = new System.Drawing.Size(230, 25);
            this.labelUtilidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // numUtilidadEmpresa
            //
            this.numUtilidadEmpresa.DecimalPlaces = 2;
            this.numUtilidadEmpresa.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            this.numUtilidadEmpresa.Location = new System.Drawing.Point(260, 125);
            this.numUtilidadEmpresa.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numUtilidadEmpresa.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numUtilidadEmpresa.Name = "numUtilidadEmpresa";
            this.numUtilidadEmpresa.Size = new System.Drawing.Size(80, 23);
            //
            // labelPorcentUtilidad
            //
            this.labelPorcentUtilidad.Location = new System.Drawing.Point(345, 125);
            this.labelPorcentUtilidad.Name = "labelPorcentUtilidad";
            this.labelPorcentUtilidad.Size = new System.Drawing.Size(20, 23);
            this.labelPorcentUtilidad.Text = "%";
            this.labelPorcentUtilidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // buttonGuardar
            //
            resources.ApplyResources(this.buttonGuardar, "buttonGuardar");
            this.buttonGuardar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonGuardar.Location = new System.Drawing.Point(30, 290);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(120, 35);
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);
            //
            // labelStatus
            //
            this.labelStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.labelStatus.Location = new System.Drawing.Point(165, 298);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(350, 23);
            this.labelStatus.Text = "";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // ConfigurarParametrosControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.groupBoxParametros);
            this.Controls.Add(this.buttonGuardar);
            this.Controls.Add(this.labelStatus);
            this.Name = "ConfigurarParametrosControl";
            this.Size = new System.Drawing.Size(700, 400);
            this.groupBoxParametros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numMargenEmpleados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMargenMateriales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUtilidadEmpresa)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.GroupBox groupBoxParametros;
        private System.Windows.Forms.Label labelEmpleados;
        private System.Windows.Forms.NumericUpDown numMargenEmpleados;
        private System.Windows.Forms.Label labelPorcentEmpleados;
        private System.Windows.Forms.Label labelMateriales;
        private System.Windows.Forms.NumericUpDown numMargenMateriales;
        private System.Windows.Forms.Label labelPorcentMateriales;
        private System.Windows.Forms.Label labelUtilidad;
        private System.Windows.Forms.NumericUpDown numUtilidadEmpresa;
        private System.Windows.Forms.Label labelPorcentUtilidad;
        private System.Windows.Forms.Button buttonGuardar;
        private System.Windows.Forms.Label labelStatus;
    }
}
