namespace MatheoCaffieri_GestorCMB
{
    partial class VerEmpleadosControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerEmpleadosControl));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonAgregarEmpleado = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.empleadosLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSearchEmpleado = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // buttonAgregarEmpleado
            // 
            resources.ApplyResources(this.buttonAgregarEmpleado, "buttonAgregarEmpleado");
            this.buttonAgregarEmpleado.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonAgregarEmpleado.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAgregarEmpleado.Name = "buttonAgregarEmpleado";
            this.buttonAgregarEmpleado.UseVisualStyleBackColor = false;
            this.buttonAgregarEmpleado.Click += new System.EventHandler(this.buttonAgregarEmpleado_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // empleadosLayoutPanel
            // 
            resources.ApplyResources(this.empleadosLayoutPanel, "empleadosLayoutPanel");
            this.empleadosLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.empleadosLayoutPanel.Name = "empleadosLayoutPanel";
            // 
            // buttonSearchEmpleado
            // 
            resources.ApplyResources(this.buttonSearchEmpleado, "buttonSearchEmpleado");
            this.buttonSearchEmpleado.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonSearchEmpleado.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.search_logo;
            this.buttonSearchEmpleado.Name = "buttonSearchEmpleado";
            this.buttonSearchEmpleado.UseVisualStyleBackColor = false;
            // 
            // VerEmpleadosControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.empleadosLayoutPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAgregarEmpleado);
            this.Controls.Add(this.buttonSearchEmpleado);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "VerEmpleadosControl";
            this.Load += new System.EventHandler(this.VerEmpleadosControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSearchEmpleado;
        private System.Windows.Forms.Button buttonAgregarEmpleado;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel empleadosLayoutPanel;
    }
}
