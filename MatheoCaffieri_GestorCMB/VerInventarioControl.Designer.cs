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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerInventarioControl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MaterialesItemPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAgregarMaterial = new System.Windows.Forms.Button();
            this.buttonGestionarProveedores = new System.Windows.Forms.Button();
            this.buttonVerInformesCompra = new System.Windows.Forms.Button();
            this.buttonSearchMaterial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // MaterialesItemPanel
            // 
            resources.ApplyResources(this.MaterialesItemPanel, "MaterialesItemPanel");
            this.MaterialesItemPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.MaterialesItemPanel.Name = "MaterialesItemPanel";
            // 
            // buttonAgregarMaterial
            // 
            resources.ApplyResources(this.buttonAgregarMaterial, "buttonAgregarMaterial");
            this.buttonAgregarMaterial.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonAgregarMaterial.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAgregarMaterial.Name = "buttonAgregarMaterial";
            this.buttonAgregarMaterial.UseVisualStyleBackColor = false;
            this.buttonAgregarMaterial.Click += new System.EventHandler(this.buttonAgregarMaterial_Click);
            // 
            // buttonGestionarProveedores
            // 
            resources.ApplyResources(this.buttonGestionarProveedores, "buttonGestionarProveedores");
            this.buttonGestionarProveedores.BackColor = System.Drawing.Color.Khaki;
            this.buttonGestionarProveedores.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonGestionarProveedores.Name = "buttonGestionarProveedores";
            this.buttonGestionarProveedores.UseVisualStyleBackColor = false;
            this.buttonGestionarProveedores.Click += new System.EventHandler(this.buttonGestionarProveedores_Click);
            // 
            // buttonVerInformesCompra
            // 
            resources.ApplyResources(this.buttonVerInformesCompra, "buttonVerInformesCompra");
            this.buttonVerInformesCompra.BackColor = System.Drawing.Color.Khaki;
            this.buttonVerInformesCompra.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonVerInformesCompra.Name = "buttonVerInformesCompra";
            this.buttonVerInformesCompra.UseVisualStyleBackColor = false;
            this.buttonVerInformesCompra.Click += new System.EventHandler(this.buttonVerInformesCompra_Click);
            // 
            // buttonSearchMaterial
            // 
            resources.ApplyResources(this.buttonSearchMaterial, "buttonSearchMaterial");
            this.buttonSearchMaterial.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonSearchMaterial.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.search_logo;
            this.buttonSearchMaterial.Name = "buttonSearchMaterial";
            this.buttonSearchMaterial.UseVisualStyleBackColor = false;
            // 
            // VerInventarioControl
            // 
            resources.ApplyResources(this, "$this");
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
            this.Name = "VerInventarioControl";
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
