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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccesosForm));
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAE = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNombrePermiso = new System.Windows.Forms.TextBox();
            this.comboBoxAcceso = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSavePermiso = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPermiso = new System.Windows.Forms.ComboBox();
            this.buttonAsignarPermiso = new System.Windows.Forms.Button();
            this.comboBoxRol = new System.Windows.Forms.ComboBox();
            this.FormPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            resources.ApplyResources(this.FormPanel, "FormPanel");
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAE);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Name = "FormPanel";
            // 
            // buttonExitAE
            // 
            resources.ApplyResources(this.buttonExitAE, "buttonExitAE");
            this.buttonExitAE.Name = "buttonExitAE";
            this.buttonExitAE.UseVisualStyleBackColor = true;
            this.buttonExitAE.Click += new System.EventHandler(this.buttonExitAE_Click);
            // 
            // buttonExit
            // 
            resources.ApplyResources(this.buttonExit, "buttonExit");
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxNombrePermiso
            // 
            resources.ApplyResources(this.textBoxNombrePermiso, "textBoxNombrePermiso");
            this.textBoxNombrePermiso.Name = "textBoxNombrePermiso";
            // 
            // comboBoxAcceso
            // 
            resources.ApplyResources(this.comboBoxAcceso, "comboBoxAcceso");
            this.comboBoxAcceso.FormattingEnabled = true;
            this.comboBoxAcceso.Name = "comboBoxAcceso";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonSavePermiso);
            this.groupBox1.Controls.Add(this.textBoxNombrePermiso);
            this.groupBox1.Controls.Add(this.comboBoxAcceso);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // buttonSavePermiso
            // 
            resources.ApplyResources(this.buttonSavePermiso, "buttonSavePermiso");
            this.buttonSavePermiso.Name = "buttonSavePermiso";
            this.buttonSavePermiso.UseVisualStyleBackColor = true;
            this.buttonSavePermiso.Click += new System.EventHandler(this.buttonSavePermiso_Click_1);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxPermiso);
            this.groupBox2.Controls.Add(this.buttonAsignarPermiso);
            this.groupBox2.Controls.Add(this.comboBoxRol);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // comboBoxPermiso
            // 
            resources.ApplyResources(this.comboBoxPermiso, "comboBoxPermiso");
            this.comboBoxPermiso.FormattingEnabled = true;
            this.comboBoxPermiso.Name = "comboBoxPermiso";
            // 
            // buttonAsignarPermiso
            // 
            resources.ApplyResources(this.buttonAsignarPermiso, "buttonAsignarPermiso");
            this.buttonAsignarPermiso.Name = "buttonAsignarPermiso";
            this.buttonAsignarPermiso.UseVisualStyleBackColor = true;
            this.buttonAsignarPermiso.Click += new System.EventHandler(this.buttonAsignarPermiso_Click_1);
            // 
            // comboBoxRol
            // 
            resources.ApplyResources(this.comboBoxRol, "comboBoxRol");
            this.comboBoxRol.FormattingEnabled = true;
            this.comboBoxRol.Name = "comboBoxRol";
            // 
            // AccesosForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AccesosForm";
            this.Load += new System.EventHandler(this.AccesosForm_Load);
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