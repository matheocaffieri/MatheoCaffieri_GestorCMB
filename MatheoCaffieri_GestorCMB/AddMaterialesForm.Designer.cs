namespace MatheoCaffieri_GestorCMB
{
    partial class AddMaterialesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPrecio = new System.Windows.Forms.TextBox();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAM = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.comboBoxMaterial = new System.Windows.Forms.ComboBox();
            this.comboBoxProveedor = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxUnidad = new System.Windows.Forms.ComboBox();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Location = new System.Drawing.Point(264, 476);
            this.buttonAgregar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(132, 34);
            this.buttonAgregar.TabIndex = 56;
            this.buttonAgregar.Text = "Agregar";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 16);
            this.label5.TabIndex = 55;
            this.label5.Text = "Proveedor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 16);
            this.label4.TabIndex = 54;
            this.label4.Text = "Descripción";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 53;
            this.label3.Text = "Precio";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 16);
            this.label1.TabIndex = 52;
            this.label1.Text = "Tipo de material";
            // 
            // textBoxPrecio
            // 
            this.textBoxPrecio.Location = new System.Drawing.Point(224, 154);
            this.textBoxPrecio.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxPrecio.Name = "textBoxPrecio";
            this.textBoxPrecio.Size = new System.Drawing.Size(172, 22);
            this.textBoxPrecio.TabIndex = 50;
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(27, 233);
            this.textBoxDescripcion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(236, 22);
            this.textBoxDescripcion.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 32);
            this.label2.TabIndex = 47;
            this.label2.Text = "Agregar materiales";
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAM);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(425, 32);
            this.FormPanel.TabIndex = 46;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            // 
            // buttonExitAM
            // 
            this.buttonExitAM.Location = new System.Drawing.Point(385, 2);
            this.buttonExitAM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExitAM.Name = "buttonExitAM";
            this.buttonExitAM.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAM.TabIndex = 31;
            this.buttonExitAM.Text = "X";
            this.buttonExitAM.UseVisualStyleBackColor = true;
            this.buttonExitAM.Click += new System.EventHandler(this.buttonExitAM_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(1044, 2);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(29, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "X";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // comboBoxMaterial
            // 
            this.comboBoxMaterial.FormattingEnabled = true;
            this.comboBoxMaterial.Location = new System.Drawing.Point(19, 154);
            this.comboBoxMaterial.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxMaterial.Name = "comboBoxMaterial";
            this.comboBoxMaterial.Size = new System.Drawing.Size(169, 24);
            this.comboBoxMaterial.TabIndex = 57;
            // 
            // comboBoxProveedor
            // 
            this.comboBoxProveedor.FormattingEnabled = true;
            this.comboBoxProveedor.Location = new System.Drawing.Point(27, 318);
            this.comboBoxProveedor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxProveedor.Name = "comboBoxProveedor";
            this.comboBoxProveedor.Size = new System.Drawing.Size(236, 24);
            this.comboBoxProveedor.TabIndex = 58;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 16);
            this.label6.TabIndex = 59;
            this.label6.Text = "Tipo de unidad";
            // 
            // comboBoxUnidad
            // 
            this.comboBoxUnidad.FormattingEnabled = true;
            this.comboBoxUnidad.Location = new System.Drawing.Point(27, 412);
            this.comboBoxUnidad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxUnidad.Name = "comboBoxUnidad";
            this.comboBoxUnidad.Size = new System.Drawing.Size(169, 24);
            this.comboBoxUnidad.TabIndex = 60;
            // 
            // AddMaterialesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 534);
            this.Controls.Add(this.comboBoxUnidad);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxProveedor);
            this.Controls.Add(this.comboBoxMaterial);
            this.Controls.Add(this.buttonAgregar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPrecio);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AddMaterialesForm";
            this.Text = "AddMaterialesForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPrecio;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAM;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ComboBox comboBoxMaterial;
        private System.Windows.Forms.ComboBox comboBoxProveedor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxUnidad;
    }
}