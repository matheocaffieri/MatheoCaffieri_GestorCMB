namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class EmpleadosItemControl
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
            this.labelInfoEmpleado = new System.Windows.Forms.Label();
            this.buttonEditarEmpleado = new System.Windows.Forms.Button();
            this.SwitchHabilitarEmpleado = new MatheoCaffieri_GestorCMB.ItemControls.ToggleSwitch();
            this.SuspendLayout();
            // 
            // labelInfoEmpleado
            // 
            this.labelInfoEmpleado.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoEmpleado.Location = new System.Drawing.Point(20, 24);
            this.labelInfoEmpleado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelInfoEmpleado.Name = "labelInfoEmpleado";
            this.labelInfoEmpleado.Size = new System.Drawing.Size(680, 27);
            this.labelInfoEmpleado.TabIndex = 30;
            this.labelInfoEmpleado.Text = "Nombre y apellido | 12345678 | $500.000 | Proyectos activos: N";
            // 
            // buttonEditarEmpleado
            // 
            this.buttonEditarEmpleado.BackColor = System.Drawing.Color.Transparent;
            this.buttonEditarEmpleado.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.edit_logo;
            this.buttonEditarEmpleado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonEditarEmpleado.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonEditarEmpleado.FlatAppearance.BorderSize = 0;
            this.buttonEditarEmpleado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditarEmpleado.Location = new System.Drawing.Point(769, 20);
            this.buttonEditarEmpleado.Name = "buttonEditarEmpleado";
            this.buttonEditarEmpleado.Size = new System.Drawing.Size(40, 37);
            this.buttonEditarEmpleado.TabIndex = 50;
            this.buttonEditarEmpleado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEditarEmpleado.UseVisualStyleBackColor = false;
            // 
            // SwitchHabilitarEmpleado
            // 
            this.SwitchHabilitarEmpleado.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.SwitchHabilitarEmpleado.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.SwitchHabilitarEmpleado.IsOn = false;
            this.SwitchHabilitarEmpleado.Location = new System.Drawing.Point(830, 20);
            this.SwitchHabilitarEmpleado.Name = "SwitchHabilitarEmpleado";
            this.SwitchHabilitarEmpleado.OffColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.SwitchHabilitarEmpleado.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(50)))));
            this.SwitchHabilitarEmpleado.Size = new System.Drawing.Size(62, 36);
            this.SwitchHabilitarEmpleado.SwitchColor = System.Drawing.Color.White;
            this.SwitchHabilitarEmpleado.TabIndex = 52;
            // 
            // EmpleadosItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.SwitchHabilitarEmpleado);
            this.Controls.Add(this.buttonEditarEmpleado);
            this.Controls.Add(this.labelInfoEmpleado);
            this.Name = "EmpleadosItemControl";
            this.Size = new System.Drawing.Size(910, 75);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInfoEmpleado;
        private System.Windows.Forms.Button buttonEditarEmpleado;
        private ToggleSwitch SwitchHabilitarEmpleado;
    }
}
