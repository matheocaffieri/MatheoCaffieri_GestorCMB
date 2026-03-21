namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class ProyectoItemControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProyectoItemControl));
            this.labelNumProyecto = new System.Windows.Forms.Label();
            this.labelDescripcionProyecto = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelEstadoProyecto = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabelVerDetalles = new System.Windows.Forms.LinkLabel();
            this.labelNombreCliente = new System.Windows.Forms.Label();
            this.labelFechaInicio = new System.Windows.Forms.Label();
            this.labelUbicacionProyecto = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNumProyecto
            // 
            resources.ApplyResources(this.labelNumProyecto, "labelNumProyecto");
            this.labelNumProyecto.Name = "labelNumProyecto";
            // 
            // labelDescripcionProyecto
            // 
            resources.ApplyResources(this.labelDescripcionProyecto, "labelDescripcionProyecto");
            this.labelDescripcionProyecto.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDescripcionProyecto.Name = "labelDescripcionProyecto";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labelEstadoProyecto
            // 
            resources.ApplyResources(this.labelEstadoProyecto, "labelEstadoProyecto");
            this.labelEstadoProyecto.ForeColor = System.Drawing.Color.Green;
            this.labelEstadoProyecto.Name = "labelEstadoProyecto";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // linkLabelVerDetalles
            // 
            resources.ApplyResources(this.linkLabelVerDetalles, "linkLabelVerDetalles");
            this.linkLabelVerDetalles.Name = "linkLabelVerDetalles";
            this.linkLabelVerDetalles.TabStop = true;
            this.linkLabelVerDetalles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelVerDetalles_LinkClicked);
            // 
            // labelNombreCliente
            // 
            resources.ApplyResources(this.labelNombreCliente, "labelNombreCliente");
            this.labelNombreCliente.Name = "labelNombreCliente";
            // 
            // labelFechaInicio
            // 
            resources.ApplyResources(this.labelFechaInicio, "labelFechaInicio");
            this.labelFechaInicio.Name = "labelFechaInicio";
            // 
            // labelUbicacionProyecto
            // 
            resources.ApplyResources(this.labelUbicacionProyecto, "labelUbicacionProyecto");
            this.labelUbicacionProyecto.Name = "labelUbicacionProyecto";
            // 
            // ProyectoItemControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.labelUbicacionProyecto);
            this.Controls.Add(this.labelFechaInicio);
            this.Controls.Add(this.labelNombreCliente);
            this.Controls.Add(this.linkLabelVerDetalles);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelEstadoProyecto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelDescripcionProyecto);
            this.Controls.Add(this.labelNumProyecto);
            this.Name = "ProyectoItemControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNumProyecto;
        private System.Windows.Forms.Label labelDescripcionProyecto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelEstadoProyecto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabelVerDetalles;
        private System.Windows.Forms.Label labelNombreCliente;
        private System.Windows.Forms.Label labelFechaInicio;
        private System.Windows.Forms.Label labelUbicacionProyecto;
    }
}
