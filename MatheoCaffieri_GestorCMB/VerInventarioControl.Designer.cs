namespace MatheoCaffieri_GestorCMB
{
    partial class VerInventarioControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MaterialesItemPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSearchMaterial = new System.Windows.Forms.Button();
            this.buttonAgregarMaterial = new System.Windows.Forms.Button();
            this.buttonGestionarProveedores = new System.Windows.Forms.Button();
            this.buttonVerInformesCompra = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 57);
            this.label1.TabIndex = 22;
            this.label1.Text = "Inventario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 31);
            this.label2.TabIndex = 23;
            this.label2.Text = "Materiales";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(164, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(399, 31);
            this.label3.TabIndex = 24;
            this.label3.Text = "___________________________________";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(39, 133);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(459, 26);
            this.textBox1.TabIndex = 25;
            // 
            // MaterialesItemPanel
            // 
            this.MaterialesItemPanel.AutoScroll = true;
            this.MaterialesItemPanel.Location = new System.Drawing.Point(28, 185);
            this.MaterialesItemPanel.Margin = new System.Windows.Forms.Padding(2);
            this.MaterialesItemPanel.Name = "MaterialesItemPanel";
            this.MaterialesItemPanel.Size = new System.Drawing.Size(764, 171);
            this.MaterialesItemPanel.TabIndex = 27;
            // 
            // buttonSearchMaterial
            // 
            this.buttonSearchMaterial.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonSearchMaterial.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.search_logo;
            this.buttonSearchMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSearchMaterial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearchMaterial.Location = new System.Drawing.Point(502, 133);
            this.buttonSearchMaterial.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSearchMaterial.Name = "buttonSearchMaterial";
            this.buttonSearchMaterial.Size = new System.Drawing.Size(27, 29);
            this.buttonSearchMaterial.TabIndex = 28;
            this.buttonSearchMaterial.UseVisualStyleBackColor = false;
            // 
            // buttonAgregarMaterial
            // 
            this.buttonAgregarMaterial.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonAgregarMaterial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAgregarMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAgregarMaterial.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAgregarMaterial.Location = new System.Drawing.Point(563, 87);
            this.buttonAgregarMaterial.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAgregarMaterial.Name = "buttonAgregarMaterial";
            this.buttonAgregarMaterial.Size = new System.Drawing.Size(179, 42);
            this.buttonAgregarMaterial.TabIndex = 29;
            this.buttonAgregarMaterial.Text = "Agregar material";
            this.buttonAgregarMaterial.UseVisualStyleBackColor = false;
            this.buttonAgregarMaterial.Click += new System.EventHandler(this.buttonAgregarMaterial_Click);
            // 
            // buttonGestionarProveedores
            // 
            this.buttonGestionarProveedores.BackColor = System.Drawing.Color.Khaki;
            this.buttonGestionarProveedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGestionarProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGestionarProveedores.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonGestionarProveedores.Location = new System.Drawing.Point(563, 20);
            this.buttonGestionarProveedores.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGestionarProveedores.Name = "buttonGestionarProveedores";
            this.buttonGestionarProveedores.Size = new System.Drawing.Size(179, 42);
            this.buttonGestionarProveedores.TabIndex = 30;
            this.buttonGestionarProveedores.Text = "Gestionar proveedores";
            this.buttonGestionarProveedores.UseVisualStyleBackColor = false;
            this.buttonGestionarProveedores.Click += new System.EventHandler(this.buttonGestionarProveedores_Click);
            // 
            // buttonVerInformesCompra
            // 
            this.buttonVerInformesCompra.BackColor = System.Drawing.Color.Khaki;
            this.buttonVerInformesCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVerInformesCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVerInformesCompra.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonVerInformesCompra.Location = new System.Drawing.Point(350, 20);
            this.buttonVerInformesCompra.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVerInformesCompra.Name = "buttonVerInformesCompra";
            this.buttonVerInformesCompra.Size = new System.Drawing.Size(179, 42);
            this.buttonVerInformesCompra.TabIndex = 31;
            this.buttonVerInformesCompra.Text = "Ver informes de compra";
            this.buttonVerInformesCompra.UseVisualStyleBackColor = false;
            this.buttonVerInformesCompra.Click += new System.EventHandler(this.buttonVerInformesCompra_Click);
            // 
            // VerInventarioControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonVerInformesCompra);
            this.Controls.Add(this.buttonGestionarProveedores);
            this.Controls.Add(this.buttonAgregarMaterial);
            this.Controls.Add(this.buttonSearchMaterial);
            this.Controls.Add(this.MaterialesItemPanel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VerInventarioControl";
            this.Size = new System.Drawing.Size(640, 286);
            this.Load += new System.EventHandler(this.VerInventarioControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.FlowLayoutPanel MaterialesItemPanel;
        private System.Windows.Forms.Button buttonSearchMaterial;
        private System.Windows.Forms.Button buttonAgregarMaterial;
        private System.Windows.Forms.Button buttonGestionarProveedores;
        private System.Windows.Forms.Button buttonVerInformesCompra;
    }
}
