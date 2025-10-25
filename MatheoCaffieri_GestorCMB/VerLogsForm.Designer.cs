namespace MatheoCaffieri_GestorCMB
{
    partial class VerLogsForm
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
            this.buttonExitAM = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.buttonArchivo = new System.Windows.Forms.Button();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAM);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(620, 26);
            this.FormPanel.TabIndex = 47;
            this.FormPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.FormPanel_Paint);
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown_1);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove_1);
            // 
            // buttonExitAM
            // 
            this.buttonExitAM.Location = new System.Drawing.Point(597, 2);
            this.buttonExitAM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonExitAM.Name = "buttonExitAM";
            this.buttonExitAM.Size = new System.Drawing.Size(21, 19);
            this.buttonExitAM.TabIndex = 31;
            this.buttonExitAM.Text = "X";
            this.buttonExitAM.UseVisualStyleBackColor = true;
            this.buttonExitAM.Click += new System.EventHandler(this.buttonExitAM_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(783, 2);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(22, 19);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "X";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(38, 55);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(540, 279);
            this.listView1.TabIndex = 48;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // buttonArchivo
            // 
            this.buttonArchivo.Location = new System.Drawing.Point(500, 351);
            this.buttonArchivo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonArchivo.Name = "buttonArchivo";
            this.buttonArchivo.Size = new System.Drawing.Size(78, 28);
            this.buttonArchivo.TabIndex = 49;
            this.buttonArchivo.Text = "Ver archivo";
            this.buttonArchivo.UseVisualStyleBackColor = true;
            this.buttonArchivo.Click += new System.EventHandler(this.buttonArchivo_Click);
            // 
            // VerLogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(620, 405);
            this.Controls.Add(this.buttonArchivo);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "VerLogsForm";
            this.Text = "VerLogsForm";
            this.Load += new System.EventHandler(this.VerLogsForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAM;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonArchivo;
    }
}