namespace MatheoCaffieri_GestorCMB
{
    partial class AgregarEmpleadoProyectoForm
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
            this.buttonExitAP = new System.Windows.Forms.Button();
            this.gestionarEmpleadosDetalleLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExitAP
            // 
            this.buttonExitAP.Location = new System.Drawing.Point(543, 2);
            this.buttonExitAP.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExitAP.Name = "buttonExitAP";
            this.buttonExitAP.Size = new System.Drawing.Size(21, 19);
            this.buttonExitAP.TabIndex = 31;
            this.buttonExitAP.Text = "X";
            this.buttonExitAP.UseVisualStyleBackColor = true;
            this.buttonExitAP.Click += new System.EventHandler(this.buttonExitAP_Click);
            // 
            // gestionarEmpleadosDetalleLayoutPanel
            // 
            this.gestionarEmpleadosDetalleLayoutPanel.AutoScroll = true;
            this.gestionarEmpleadosDetalleLayoutPanel.Location = new System.Drawing.Point(11, 68);
            this.gestionarEmpleadosDetalleLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.gestionarEmpleadosDetalleLayoutPanel.Name = "gestionarEmpleadosDetalleLayoutPanel";
            this.gestionarEmpleadosDetalleLayoutPanel.Size = new System.Drawing.Size(544, 247);
            this.gestionarEmpleadosDetalleLayoutPanel.TabIndex = 50;
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAP);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(2);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(566, 26);
            this.FormPanel.TabIndex = 49;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(783, 2);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(22, 19);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "X";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // AgregarEmpleadoProyectoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 320);
            this.Controls.Add(this.gestionarEmpleadosDetalleLayoutPanel);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AgregarEmpleadoProyectoForm";
            this.Text = "AgregarEmpleadoProyectoForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExitAP;
        private System.Windows.Forms.FlowLayoutPanel gestionarEmpleadosDetalleLayoutPanel;
        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExit;
    }
}