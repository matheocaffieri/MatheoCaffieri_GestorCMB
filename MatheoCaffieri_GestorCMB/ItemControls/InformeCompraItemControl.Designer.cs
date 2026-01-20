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
            this.labelNumeroProyecto = new System.Windows.Forms.Label();
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.buttonAgregarCompra = new System.Windows.Forms.Button();
            this.buttonEliminar = new System.Windows.Forms.Button();
            this.textBoxItemsFaltantes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelNumeroProyecto
            // 
            this.labelNumeroProyecto.AutoSize = true;
            this.labelNumeroProyecto.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumeroProyecto.Location = new System.Drawing.Point(22, 12);
            this.labelNumeroProyecto.Name = "labelNumeroProyecto";
            this.labelNumeroProyecto.Size = new System.Drawing.Size(105, 21);
            this.labelNumeroProyecto.TabIndex = 35;
            this.labelNumeroProyecto.Text = "Proyecto #N";
            // 
            // labelNombreProyecto
            // 
            this.labelNombreProyecto.AutoSize = true;
            this.labelNombreProyecto.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNombreProyecto.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelNombreProyecto.Location = new System.Drawing.Point(127, 12);
            this.labelNombreProyecto.Name = "labelNombreProyecto";
            this.labelNombreProyecto.Size = new System.Drawing.Size(169, 21);
            this.labelNombreProyecto.TabIndex = 36;
            this.labelNombreProyecto.Text = "Nombre de proyecto";
            // 
            // buttonAgregarCompra
            // 
            this.buttonAgregarCompra.Location = new System.Drawing.Point(26, 47);
            this.buttonAgregarCompra.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonAgregarCompra.Name = "buttonAgregarCompra";
            this.buttonAgregarCompra.Size = new System.Drawing.Size(95, 24);
            this.buttonAgregarCompra.TabIndex = 37;
            this.buttonAgregarCompra.Text = "Agregar compra";
            this.buttonAgregarCompra.UseVisualStyleBackColor = true;
            // 
            // buttonEliminar
            // 
            this.buttonEliminar.Location = new System.Drawing.Point(133, 47);
            this.buttonEliminar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonEliminar.Name = "buttonEliminar";
            this.buttonEliminar.Size = new System.Drawing.Size(95, 24);
            this.buttonEliminar.TabIndex = 38;
            this.buttonEliminar.Text = "Eliminar";
            this.buttonEliminar.UseVisualStyleBackColor = true;
            // 
            // textBoxItemsFaltantes
            // 
            this.textBoxItemsFaltantes.Enabled = false;
            this.textBoxItemsFaltantes.Location = new System.Drawing.Point(323, 3);
            this.textBoxItemsFaltantes.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxItemsFaltantes.Multiline = true;
            this.textBoxItemsFaltantes.Name = "textBoxItemsFaltantes";
            this.textBoxItemsFaltantes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxItemsFaltantes.Size = new System.Drawing.Size(319, 76);
            this.textBoxItemsFaltantes.TabIndex = 0;
            // 
            // InformeCompraItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.textBoxItemsFaltantes);
            this.Controls.Add(this.buttonEliminar);
            this.Controls.Add(this.buttonAgregarCompra);
            this.Controls.Add(this.labelNombreProyecto);
            this.Controls.Add(this.labelNumeroProyecto);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "InformeCompraItemControl";
            this.Size = new System.Drawing.Size(644, 81);
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
