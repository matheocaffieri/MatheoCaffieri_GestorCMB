namespace MatheoCaffieri_GestorCMB
{
    partial class InformesDeCompraControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformesDeCompraControl));
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonSearchClientes = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.informeLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            resources.ApplyResources(this.buttonBack, "buttonBack");
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonSearchClientes
            // 
            resources.ApplyResources(this.buttonSearchClientes, "buttonSearchClientes");
            this.buttonSearchClientes.Name = "buttonSearchClientes";
            this.buttonSearchClientes.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // informeLayoutPanel
            // 
            resources.ApplyResources(this.informeLayoutPanel, "informeLayoutPanel");
            this.informeLayoutPanel.Name = "informeLayoutPanel";
            // 
            // InformesDeCompraControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.informeLayoutPanel);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSearchClientes);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Name = "InformesDeCompraControl";
            this.Load += new System.EventHandler(this.InformesDeCompraControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonSearchClientes;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel informeLayoutPanel;
    }
}
