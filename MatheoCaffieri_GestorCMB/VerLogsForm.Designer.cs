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
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(536, 32);
            this.FormPanel.TabIndex = 47;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown_1);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove_1);
            // 
            // buttonExitAM
            // 
            this.buttonExitAM.Location = new System.Drawing.Point(499, 3);
            this.buttonExitAM.Name = "buttonExitAM";
            this.buttonExitAM.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAM.TabIndex = 31;
            this.buttonExitAM.Text = "X";
            this.buttonExitAM.UseVisualStyleBackColor = true;
            this.buttonExitAM.Click += new System.EventHandler(this.buttonExitAM_Click);
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
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(51, 68);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(434, 342);
            this.listView1.TabIndex = 48;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // VerLogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(536, 498);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VerLogsForm";
            this.Text = "VerLogsForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAM;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ListView listView1;
    }
}