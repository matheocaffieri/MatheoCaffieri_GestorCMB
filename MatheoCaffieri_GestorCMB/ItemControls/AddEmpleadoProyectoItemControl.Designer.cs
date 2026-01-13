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
            this.buttonAgregarEmpleado = new System.Windows.Forms.Button();
            this.labelInfoEmpleado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonAgregarEmpleado
            // 
            this.buttonAgregarEmpleado.Location = new System.Drawing.Point(433, 14);
            this.buttonAgregarEmpleado.Name = "buttonAgregarEmpleado";
            this.buttonAgregarEmpleado.Size = new System.Drawing.Size(54, 31);
            this.buttonAgregarEmpleado.TabIndex = 32;
            this.buttonAgregarEmpleado.Text = "Agregar";
            this.buttonAgregarEmpleado.UseVisualStyleBackColor = true;
            this.buttonAgregarEmpleado.Click += new System.EventHandler(this.buttonAgregarEmpleado_Click);
            // 
            // labelInfoEmpleado
            // 
            this.labelInfoEmpleado.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoEmpleado.Location = new System.Drawing.Point(7, 19);
            this.labelInfoEmpleado.Name = "labelInfoEmpleado";
            this.labelInfoEmpleado.Size = new System.Drawing.Size(420, 22);
            this.labelInfoEmpleado.TabIndex = 33;
            this.labelInfoEmpleado.Text = "Nombre y apellido | 12345678 | $500.000 | Proyectos activos: N";
            // 
            // AddEmpleadoProyectoItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.labelInfoEmpleado);
            this.Controls.Add(this.buttonAgregarEmpleado);
            this.Name = "AddEmpleadoProyectoItemControl";
            this.Size = new System.Drawing.Size(509, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAgregarEmpleado;
        private System.Windows.Forms.Label labelInfoEmpleado;
    }
}
