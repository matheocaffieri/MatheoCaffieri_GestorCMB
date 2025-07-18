namespace MatheoCaffieri_GestorCMB
{
    partial class AccesosForm
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
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAE = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNombrePermiso = new System.Windows.Forms.TextBox();
            this.comboBoxAcceso = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSavePermiso = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxRol = new System.Windows.Forms.ComboBox();
            this.buttonAsignarPermiso = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPermiso = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FormPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAE);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(435, 32);
            this.FormPanel.TabIndex = 32;
            // 
            // buttonExitAE
            // 
            this.buttonExitAE.Location = new System.Drawing.Point(404, 6);
            this.buttonExitAE.Name = "buttonExitAE";
            this.buttonExitAE.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAE.TabIndex = 31;
            this.buttonExitAE.Text = "X";
            this.buttonExitAE.UseVisualStyleBackColor = true;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(1044, 3);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(30, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "X";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(299, 40);
            this.label2.TabIndex = 48;
            this.label2.Text = "Registrar permisos";
            // 
            // textBoxNombrePermiso
            // 
            this.textBoxNombrePermiso.Location = new System.Drawing.Point(23, 52);
            this.textBoxNombrePermiso.Name = "textBoxNombrePermiso";
            this.textBoxNombrePermiso.Size = new System.Drawing.Size(344, 22);
            this.textBoxNombrePermiso.TabIndex = 49;
            // 
            // comboBoxAcceso
            // 
            this.comboBoxAcceso.FormattingEnabled = true;
            this.comboBoxAcceso.Location = new System.Drawing.Point(23, 109);
            this.comboBoxAcceso.Name = "comboBoxAcceso";
            this.comboBoxAcceso.Size = new System.Drawing.Size(245, 24);
            this.comboBoxAcceso.TabIndex = 50;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonSavePermiso);
            this.groupBox1.Controls.Add(this.textBoxNombrePermiso);
            this.groupBox1.Controls.Add(this.comboBoxAcceso);
            this.groupBox1.Location = new System.Drawing.Point(20, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 151);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Crear permiso";
            // 
            // buttonSavePermiso
            // 
            this.buttonSavePermiso.Location = new System.Drawing.Point(274, 103);
            this.buttonSavePermiso.Name = "buttonSavePermiso";
            this.buttonSavePermiso.Size = new System.Drawing.Size(93, 35);
            this.buttonSavePermiso.TabIndex = 51;
            this.buttonSavePermiso.Text = "Guardar";
            this.buttonSavePermiso.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxPermiso);
            this.groupBox2.Controls.Add(this.buttonAsignarPermiso);
            this.groupBox2.Controls.Add(this.comboBoxRol);
            this.groupBox2.Location = new System.Drawing.Point(20, 284);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(393, 151);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Asignar permiso";
            // 
            // comboBoxRol
            // 
            this.comboBoxRol.FormattingEnabled = true;
            this.comboBoxRol.Location = new System.Drawing.Point(23, 109);
            this.comboBoxRol.Name = "comboBoxRol";
            this.comboBoxRol.Size = new System.Drawing.Size(245, 24);
            this.comboBoxRol.TabIndex = 50;
            // 
            // buttonAsignarPermiso
            // 
            this.buttonAsignarPermiso.Location = new System.Drawing.Point(274, 103);
            this.buttonAsignarPermiso.Name = "buttonAsignarPermiso";
            this.buttonAsignarPermiso.Size = new System.Drawing.Size(93, 35);
            this.buttonAsignarPermiso.TabIndex = 52;
            this.buttonAsignarPermiso.Text = "Asignar";
            this.buttonAsignarPermiso.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 53;
            this.label1.Text = "Nombre";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "Acceso";
            // 
            // comboBoxPermiso
            // 
            this.comboBoxPermiso.FormattingEnabled = true;
            this.comboBoxPermiso.Location = new System.Drawing.Point(23, 52);
            this.comboBoxPermiso.Name = "comboBoxPermiso";
            this.comboBoxPermiso.Size = new System.Drawing.Size(344, 24);
            this.comboBoxPermiso.TabIndex = 53;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 16);
            this.label4.TabIndex = 55;
            this.label4.Text = "Seleccionar permiso";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Seleccionar rol";
            // 
            // AccesosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 487);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AccesosForm";
            this.Text = "AccesosForm";
            this.FormPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAE;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNombrePermiso;
        private System.Windows.Forms.ComboBox comboBoxAcceso;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSavePermiso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonAsignarPermiso;
        private System.Windows.Forms.ComboBox comboBoxRol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPermiso;
    }
}