namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class AddEmpleadoProyectoItemControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEmpleadoProyectoItemControl));
            this.buttonAgregarEmpleado = new System.Windows.Forms.Button();
            this.labelInfoEmpleado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonAgregarEmpleado
            // 
            resources.ApplyResources(this.buttonAgregarEmpleado, "buttonAgregarEmpleado");
            this.buttonAgregarEmpleado.Name = "buttonAgregarEmpleado";
            this.buttonAgregarEmpleado.UseVisualStyleBackColor = true;
            this.buttonAgregarEmpleado.Click += new System.EventHandler(this.buttonAgregarEmpleado_Click);
            // 
            // labelInfoEmpleado
            // 
            resources.ApplyResources(this.labelInfoEmpleado, "labelInfoEmpleado");
            this.labelInfoEmpleado.Name = "labelInfoEmpleado";
            // 
            // AddEmpleadoProyectoItemControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.labelInfoEmpleado);
            this.Controls.Add(this.buttonAgregarEmpleado);
            this.Name = "AddEmpleadoProyectoItemControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAgregarEmpleado;
        private System.Windows.Forms.Label labelInfoEmpleado;
    }
}
