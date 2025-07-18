namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class ProveedorItemControl
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
            this.labelDescripcionProveedor = new System.Windows.Forms.Label();
            this.labelTelefonoProveedor = new System.Windows.Forms.Label();
            this.SwitchHabilitarProveedor = new MatheoCaffieri_GestorCMB.ItemControls.ToggleSwitch();
            this.buttonEditarProveedor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDescripcionProveedor
            // 
            this.labelDescripcionProveedor.AutoEllipsis = true;
            this.labelDescripcionProveedor.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescripcionProveedor.Location = new System.Drawing.Point(13, 17);
            this.labelDescripcionProveedor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDescripcionProveedor.Name = "labelDescripcionProveedor";
            this.labelDescripcionProveedor.Size = new System.Drawing.Size(162, 23);
            this.labelDescripcionProveedor.TabIndex = 47;
            this.labelDescripcionProveedor.Text = "Descripcion";
            // 
            // labelTelefonoProveedor
            // 
            this.labelTelefonoProveedor.AutoSize = true;
            this.labelTelefonoProveedor.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTelefonoProveedor.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTelefonoProveedor.Location = new System.Drawing.Point(183, 17);
            this.labelTelefonoProveedor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTelefonoProveedor.Name = "labelTelefonoProveedor";
            this.labelTelefonoProveedor.Size = new System.Drawing.Size(110, 23);
            this.labelTelefonoProveedor.TabIndex = 48;
            this.labelTelefonoProveedor.Text = "1112345678";
            // 
            // SwitchHabilitarProveedor
            // 
            this.SwitchHabilitarProveedor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.SwitchHabilitarProveedor.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.SwitchHabilitarProveedor.IsOn = false;
            this.SwitchHabilitarProveedor.Location = new System.Drawing.Point(436, 12);
            this.SwitchHabilitarProveedor.Name = "SwitchHabilitarProveedor";
            this.SwitchHabilitarProveedor.OffColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.SwitchHabilitarProveedor.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(50)))));
            this.SwitchHabilitarProveedor.Size = new System.Drawing.Size(62, 36);
            this.SwitchHabilitarProveedor.SwitchColor = System.Drawing.Color.White;
            this.SwitchHabilitarProveedor.TabIndex = 54;
            // 
            // buttonEditarProveedor
            // 
            this.buttonEditarProveedor.BackColor = System.Drawing.Color.Transparent;
            this.buttonEditarProveedor.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.edit_logo;
            this.buttonEditarProveedor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonEditarProveedor.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonEditarProveedor.FlatAppearance.BorderSize = 0;
            this.buttonEditarProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditarProveedor.Location = new System.Drawing.Point(375, 12);
            this.buttonEditarProveedor.Name = "buttonEditarProveedor";
            this.buttonEditarProveedor.Size = new System.Drawing.Size(40, 37);
            this.buttonEditarProveedor.TabIndex = 53;
            this.buttonEditarProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEditarProveedor.UseVisualStyleBackColor = false;
            // 
            // ProveedorItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.SwitchHabilitarProveedor);
            this.Controls.Add(this.buttonEditarProveedor);
            this.Controls.Add(this.labelTelefonoProveedor);
            this.Controls.Add(this.labelDescripcionProveedor);
            this.Name = "ProveedorItemControl";
            this.Size = new System.Drawing.Size(512, 59);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDescripcionProveedor;
        private System.Windows.Forms.Label labelTelefonoProveedor;
        private ToggleSwitch SwitchHabilitarProveedor;
        private System.Windows.Forms.Button buttonEditarProveedor;
    }
}
