namespace MatheoCaffieri_GestorCMB
{
    partial class AddEmpleadosForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEmpleadosForm));
            this.textBoxDocumento = new System.Windows.Forms.TextBox();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAE = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.textBoxApellido = new System.Windows.Forms.TextBox();
            this.textBoxSueldo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxDocumento
            // 
            resources.ApplyResources(this.textBoxDocumento, "textBoxDocumento");
            this.textBoxDocumento.Name = "textBoxDocumento";
            // 
            // textBoxNombre
            // 
            resources.ApplyResources(this.textBoxNombre, "textBoxNombre");
            this.textBoxNombre.Name = "textBoxNombre";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FormPanel
            // 
            resources.ApplyResources(this.FormPanel, "FormPanel");
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAE);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
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
            // textBoxApellido
            // 
            resources.ApplyResources(this.textBoxApellido, "textBoxApellido");
            this.textBoxApellido.Name = "textBoxApellido";
            // 
            // textBoxSueldo
            // 
            resources.ApplyResources(this.textBoxSueldo, "textBoxSueldo");
            this.textBoxSueldo.Name = "textBoxSueldo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // buttonAgregar
            // 
            resources.ApplyResources(this.buttonAgregar, "buttonAgregar");
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.UseVisualStyleBackColor = true;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);
            // 
            // AddEmpleadosForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.buttonAgregar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSueldo);
            this.Controls.Add(this.textBoxApellido);
            this.Controls.Add(this.textBoxDocumento);
            this.Controls.Add(this.textBoxNombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddEmpleadosForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxDocumento;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAE;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.TextBox textBoxApellido;
        private System.Windows.Forms.TextBox textBoxSueldo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonAgregar;
    }
}