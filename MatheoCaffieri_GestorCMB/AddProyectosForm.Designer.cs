namespace MatheoCaffieri_GestorCMB
{
    partial class AddProyectosForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProyectosForm));
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAP = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDescProyecto = new System.Windows.Forms.TextBox();
            this.comboBoxCliente = new System.Windows.Forms.ComboBox();
            this.textBoxUbicacion = new System.Windows.Forms.TextBox();
            this.dateTimePickerProyecto = new System.Windows.Forms.DateTimePicker();
            this.buttonAddProyecto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            resources.ApplyResources(this.FormPanel, "FormPanel");
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAP);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            // 
            // buttonExitAP
            // 
            resources.ApplyResources(this.buttonExitAP, "buttonExitAP");
            this.buttonExitAP.Name = "buttonExitAP";
            this.buttonExitAP.UseVisualStyleBackColor = true;
            this.buttonExitAP.Click += new System.EventHandler(this.buttonExitAP_Click);
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
            // textBoxDescProyecto
            // 
            resources.ApplyResources(this.textBoxDescProyecto, "textBoxDescProyecto");
            this.textBoxDescProyecto.Name = "textBoxDescProyecto";
            // 
            // comboBoxCliente
            // 
            resources.ApplyResources(this.comboBoxCliente, "comboBoxCliente");
            this.comboBoxCliente.FormattingEnabled = true;
            this.comboBoxCliente.Name = "comboBoxCliente";
            // 
            // textBoxUbicacion
            // 
            resources.ApplyResources(this.textBoxUbicacion, "textBoxUbicacion");
            this.textBoxUbicacion.Name = "textBoxUbicacion";
            // 
            // dateTimePickerProyecto
            // 
            resources.ApplyResources(this.dateTimePickerProyecto, "dateTimePickerProyecto");
            this.dateTimePickerProyecto.Name = "dateTimePickerProyecto";
            // 
            // buttonAddProyecto
            // 
            resources.ApplyResources(this.buttonAddProyecto, "buttonAddProyecto");
            this.buttonAddProyecto.Name = "buttonAddProyecto";
            this.buttonAddProyecto.UseVisualStyleBackColor = true;
            this.buttonAddProyecto.Click += new System.EventHandler(this.buttonAddProyecto_Click);
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
            // AddProyectosForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAddProyecto);
            this.Controls.Add(this.dateTimePickerProyecto);
            this.Controls.Add(this.textBoxUbicacion);
            this.Controls.Add(this.comboBoxCliente);
            this.Controls.Add(this.textBoxDescProyecto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddProyectosForm";
            this.Load += new System.EventHandler(this.AddProyectosForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDescProyecto;
        private System.Windows.Forms.ComboBox comboBoxCliente;
        private System.Windows.Forms.TextBox textBoxUbicacion;
        private System.Windows.Forms.DateTimePicker dateTimePickerProyecto;
        private System.Windows.Forms.Button buttonAddProyecto;
        private System.Windows.Forms.Button buttonExitAP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}