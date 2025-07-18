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
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonSearchClientes = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.informeLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(29, 26);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(48, 42);
            this.buttonBack.TabIndex = 37;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            // 
            // buttonSearchClientes
            // 
            this.buttonSearchClientes.Location = new System.Drawing.Point(967, 31);
            this.buttonSearchClientes.Name = "buttonSearchClientes";
            this.buttonSearchClientes.Size = new System.Drawing.Size(70, 42);
            this.buttonSearchClientes.TabIndex = 36;
            this.buttonSearchClientes.Text = "Search";
            this.buttonSearchClientes.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(409, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(540, 30);
            this.textBox1.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(84, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(306, 39);
            this.label2.TabIndex = 34;
            this.label2.Text = "Informes de compra";
            // 
            // informeLayoutPanel
            // 
            this.informeLayoutPanel.AutoScroll = true;
            this.informeLayoutPanel.Location = new System.Drawing.Point(91, 100);
            this.informeLayoutPanel.Name = "informeLayoutPanel";
            this.informeLayoutPanel.Size = new System.Drawing.Size(973, 338);
            this.informeLayoutPanel.TabIndex = 39;
            // 
            // InformesDeCompraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.informeLayoutPanel);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSearchClientes);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Name = "InformesDeCompraControl";
            this.Size = new System.Drawing.Size(1067, 441);
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
