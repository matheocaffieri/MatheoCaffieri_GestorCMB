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
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonExitAP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAP);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(632, 32);
            this.FormPanel.TabIndex = 1;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
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
            this.label2.Location = new System.Drawing.Point(197, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 32);
            this.label2.TabIndex = 24;
            this.label2.Text = "Agregar proyectos";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(27, 176);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(275, 22);
            this.textBox1.TabIndex = 25;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(330, 176);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(275, 24);
            this.comboBox1.TabIndex = 26;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(27, 285);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(275, 22);
            this.textBox2.TabIndex = 27;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(27, 379);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(275, 22);
            this.dateTimePicker1.TabIndex = 28;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(473, 455);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 35);
            this.button1.TabIndex = 29;
            this.button1.Text = "Agregar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonExitAP
            // 
            this.buttonExitAP.Location = new System.Drawing.Point(601, 6);
            this.buttonExitAP.Name = "buttonExitAP";
            this.buttonExitAP.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAP.TabIndex = 31;
            this.buttonExitAP.Text = "X";
            this.buttonExitAP.UseVisualStyleBackColor = true;
            this.buttonExitAP.Click += new System.EventHandler(this.buttonExitAP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 16);
            this.label1.TabIndex = 31;
            this.label1.Text = "Descripción del proyecto";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(327, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 32;
            this.label3.Text = "Cliente";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Ubicación";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 16);
            this.label5.TabIndex = 34;
            this.label5.Text = "Fecha de inicio";
            // 
            // AddProyectosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(632, 502);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddProyectosForm";
            this.Text = "AddProyectosForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonExitAP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}