namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class UsuarioItemControl
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
            this.labelInfoMailUsuario = new System.Windows.Forms.Label();
            this.buttonEditarUsuario = new System.Windows.Forms.Button();
            this.labelNumero = new System.Windows.Forms.Label();
            this.SwitchHabilitarUsuario = new MatheoCaffieri_GestorCMB.ItemControls.ToggleSwitch();
            this.SuspendLayout();
            // 
            // labelInfoMailUsuario
            // 
            this.labelInfoMailUsuario.AutoSize = true;
            this.labelInfoMailUsuario.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoMailUsuario.Location = new System.Drawing.Point(50, 13);
            this.labelInfoMailUsuario.Name = "labelInfoMailUsuario";
            this.labelInfoMailUsuario.Size = new System.Drawing.Size(166, 16);
            this.labelInfoMailUsuario.TabIndex = 64;
            this.labelInfoMailUsuario.Text = "nombreusuario@gmail.com.ar";
            // 
            // buttonEditarUsuario
            // 
            this.buttonEditarUsuario.BackColor = System.Drawing.Color.Transparent;
            this.buttonEditarUsuario.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.edit_logo;
            this.buttonEditarUsuario.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonEditarUsuario.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonEditarUsuario.FlatAppearance.BorderSize = 0;
            this.buttonEditarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditarUsuario.Location = new System.Drawing.Point(258, 7);
            this.buttonEditarUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEditarUsuario.Name = "buttonEditarUsuario";
            this.buttonEditarUsuario.Size = new System.Drawing.Size(27, 26);
            this.buttonEditarUsuario.TabIndex = 67;
            this.buttonEditarUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEditarUsuario.UseVisualStyleBackColor = false;
            this.buttonEditarUsuario.Click += new System.EventHandler(this.buttonEditarUsuario_Click);
            // 
            // labelNumero
            // 
            this.labelNumero.AutoSize = true;
            this.labelNumero.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumero.ForeColor = System.Drawing.Color.DimGray;
            this.labelNumero.Location = new System.Drawing.Point(12, 13);
            this.labelNumero.Name = "labelNumero";
            this.labelNumero.Size = new System.Drawing.Size(23, 16);
            this.labelNumero.TabIndex = 68;
            this.labelNumero.Text = "#N";
            // 
            // SwitchHabilitarUsuario
            // 
            this.SwitchHabilitarUsuario.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.SwitchHabilitarUsuario.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.SwitchHabilitarUsuario.IsOn = false;
            this.SwitchHabilitarUsuario.Location = new System.Drawing.Point(301, 7);
            this.SwitchHabilitarUsuario.Margin = new System.Windows.Forms.Padding(2);
            this.SwitchHabilitarUsuario.Name = "SwitchHabilitarUsuario";
            this.SwitchHabilitarUsuario.OffColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.SwitchHabilitarUsuario.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(205)))), ((int)(((byte)(50)))));
            this.SwitchHabilitarUsuario.Size = new System.Drawing.Size(45, 27);
            this.SwitchHabilitarUsuario.SwitchColor = System.Drawing.Color.White;
            this.SwitchHabilitarUsuario.TabIndex = 66;
            // 
            // UsuarioItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.labelNumero);
            this.Controls.Add(this.buttonEditarUsuario);
            this.Controls.Add(this.SwitchHabilitarUsuario);
            this.Controls.Add(this.labelInfoMailUsuario);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UsuarioItemControl";
            this.Size = new System.Drawing.Size(358, 43);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfoMailUsuario;
        private ToggleSwitch SwitchHabilitarUsuario;
        private System.Windows.Forms.Button buttonEditarUsuario;
        private System.Windows.Forms.Label labelNumero;
    }
}
