namespace MatheoCaffieri_GestorCMB
{
    partial class ProveedorControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonSearchClientes = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.proveedorLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 57;
            this.label3.Text = "Descripción";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 56;
            this.label1.Text = "Teléfono";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(387, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 300);
            this.panel1.TabIndex = 55;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 253);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 36);
            this.button1.TabIndex = 54;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(31, 190);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(275, 22);
            this.textBox3.TabIndex = 51;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(31, 117);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(275, 22);
            this.textBox2.TabIndex = 50;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(31, 30);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(48, 42);
            this.buttonBack.TabIndex = 49;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            // 
            // buttonSearchClientes
            // 
            this.buttonSearchClientes.Location = new System.Drawing.Point(969, 35);
            this.buttonSearchClientes.Name = "buttonSearchClientes";
            this.buttonSearchClientes.Size = new System.Drawing.Size(70, 42);
            this.buttonSearchClientes.TabIndex = 48;
            this.buttonSearchClientes.Text = "Search";
            this.buttonSearchClientes.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(469, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(482, 30);
            this.textBox1.TabIndex = 47;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(86, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(342, 39);
            this.label2.TabIndex = 46;
            this.label2.Text = "Gestionar proveedores";
            // 
            // proveedorLayoutPanel
            // 
            this.proveedorLayoutPanel.AutoScroll = true;
            this.proveedorLayoutPanel.Location = new System.Drawing.Point(432, 117);
            this.proveedorLayoutPanel.Name = "proveedorLayoutPanel";
            this.proveedorLayoutPanel.Size = new System.Drawing.Size(544, 321);
            this.proveedorLayoutPanel.TabIndex = 59;
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // ProveedorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.proveedorLayoutPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSearchClientes);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Name = "ProveedorControl";
            this.Size = new System.Drawing.Size(1067, 441);
            this.Load += new System.EventHandler(this.ProveedorControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonSearchClientes;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel proveedorLayoutPanel;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}
