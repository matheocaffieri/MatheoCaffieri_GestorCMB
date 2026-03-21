namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class InformeCompraItemControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformeCompraItemControl));
            this.labelNumeroProyecto = new System.Windows.Forms.Label();
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.buttonAgregarCompra = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.textBoxItemsFaltantes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelNumeroProyecto
            // 
            resources.ApplyResources(this.labelNumeroProyecto, "labelNumeroProyecto");
            this.labelNumeroProyecto.Name = "labelNumeroProyecto";
            // 
            // labelNombreProyecto
            // 
            resources.ApplyResources(this.labelNombreProyecto, "labelNombreProyecto");
            this.labelNombreProyecto.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelNombreProyecto.Name = "labelNombreProyecto";
            // 
            // buttonAgregarCompra
            // 
            resources.ApplyResources(this.buttonAgregarCompra, "buttonAgregarCompra");
            this.buttonAgregarCompra.Name = "buttonAgregarCompra";
            this.buttonAgregarCompra.UseVisualStyleBackColor = true;
            // 
            // buttonEliminar
            // 
            resources.ApplyResources(this.buttonEliminar, "buttonEliminar");
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            // 
            // textBoxItemsFaltantes
            // 
            resources.ApplyResources(this.textBoxItemsFaltantes, "textBoxItemsFaltantes");
            this.textBoxItemsFaltantes.Name = "textBoxItemsFaltantes";
            // 
            // InformeCompraItemControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.textBoxItemsFaltantes);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.buttonAgregarCompra);
            this.Controls.Add(this.labelNombreProyecto);
            this.Controls.Add(this.labelNumeroProyecto);
            this.Name = "InformeCompraItemControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNumeroProyecto;
        private System.Windows.Forms.Label labelNombreProyecto;
        private System.Windows.Forms.Button buttonAgregarCompra;
        private System.Windows.Forms.Button buttonEliminar;
        private System.Windows.Forms.TextBox textBoxItemsFaltantes;
    }
}
