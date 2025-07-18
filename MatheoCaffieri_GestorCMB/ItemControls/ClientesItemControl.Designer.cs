namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class ClientesItemControl
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
            this.labelRazonSocial = new System.Windows.Forms.Label();
            this.labelTelefonoCliente = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelNombreContacto = new System.Windows.Forms.Label();
            this.labelMailCliente = new System.Windows.Forms.Label();
            this.buttonModificarCliente = new System.Windows.Forms.Button();
            this.buttonHabDesCliente = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRazonSocial
            // 
            this.labelRazonSocial.AutoSize = true;
            this.labelRazonSocial.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRazonSocial.Location = new System.Drawing.Point(13, 9);
            this.labelRazonSocial.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRazonSocial.Name = "labelRazonSocial";
            this.labelRazonSocial.Size = new System.Drawing.Size(93, 20);
            this.labelRazonSocial.TabIndex = 36;
            this.labelRazonSocial.Text = "Razon social";
            // 
            // labelTelefonoCliente
            // 
            this.labelTelefonoCliente.AutoSize = true;
            this.labelTelefonoCliente.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTelefonoCliente.Location = new System.Drawing.Point(13, 49);
            this.labelTelefonoCliente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTelefonoCliente.Name = "labelTelefonoCliente";
            this.labelTelefonoCliente.Size = new System.Drawing.Size(89, 20);
            this.labelTelefonoCliente.TabIndex = 37;
            this.labelTelefonoCliente.Text = "1112345678";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(110, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 20);
            this.label1.TabIndex = 38;
            this.label1.Text = "-";
            // 
            // labelNombreContacto
            // 
            this.labelNombreContacto.AutoSize = true;
            this.labelNombreContacto.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNombreContacto.Location = new System.Drawing.Point(133, 49);
            this.labelNombreContacto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNombreContacto.Name = "labelNombreContacto";
            this.labelNombreContacto.Size = new System.Drawing.Size(125, 20);
            this.labelNombreContacto.TabIndex = 39;
            this.labelNombreContacto.Text = "nombre contacto";
            // 
            // labelMailCliente
            // 
            this.labelMailCliente.AutoSize = true;
            this.labelMailCliente.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMailCliente.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMailCliente.Location = new System.Drawing.Point(114, 9);
            this.labelMailCliente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMailCliente.Name = "labelMailCliente";
            this.labelMailCliente.Size = new System.Drawing.Size(129, 20);
            this.labelMailCliente.TabIndex = 40;
            this.labelMailCliente.Text = "mail@mail.com.ar";
            // 
            // buttonModificarCliente
            // 
            this.buttonModificarCliente.Location = new System.Drawing.Point(362, 19);
            this.buttonModificarCliente.Name = "buttonModificarCliente";
            this.buttonModificarCliente.Size = new System.Drawing.Size(56, 37);
            this.buttonModificarCliente.TabIndex = 41;
            this.buttonModificarCliente.Text = "Modificar";
            this.buttonModificarCliente.UseVisualStyleBackColor = true;
            // 
            // buttonHabDesCliente
            // 
            this.buttonHabDesCliente.Location = new System.Drawing.Point(435, 19);
            this.buttonHabDesCliente.Name = "buttonHabDesCliente";
            this.buttonHabDesCliente.Size = new System.Drawing.Size(56, 37);
            this.buttonHabDesCliente.TabIndex = 42;
            this.buttonHabDesCliente.Text = "Habilitar";
            this.buttonHabDesCliente.UseVisualStyleBackColor = true;
            // 
            // ClientesItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.buttonHabDesCliente);
            this.Controls.Add(this.buttonModificarCliente);
            this.Controls.Add(this.labelMailCliente);
            this.Controls.Add(this.labelNombreContacto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTelefonoCliente);
            this.Controls.Add(this.labelRazonSocial);
            this.Name = "ClientesItemControl";
            this.Size = new System.Drawing.Size(518, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRazonSocial;
        private System.Windows.Forms.Label labelTelefonoCliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelNombreContacto;
        private System.Windows.Forms.Label labelMailCliente;
        private System.Windows.Forms.Button buttonModificarCliente;
        private System.Windows.Forms.Button buttonHabDesCliente;
    }
}
