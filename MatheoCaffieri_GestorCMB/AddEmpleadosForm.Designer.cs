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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitAE = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(27, 243);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(236, 22);
            this.textBox2.TabIndex = 35;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(27, 164);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 22);
            this.textBox1.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(251, 32);
            this.label2.TabIndex = 32;
            this.label2.Text = "Agregar empleados";
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAE);
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(425, 32);
            this.FormPanel.TabIndex = 31;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            // 
            // buttonExitAE
            // 
            this.buttonExitAE.Location = new System.Drawing.Point(385, 3);
            this.buttonExitAE.Name = "buttonExitAE";
            this.buttonExitAE.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAE.TabIndex = 31;
            this.buttonExitAE.Text = "X";
            this.buttonExitAE.UseVisualStyleBackColor = true;
            this.buttonExitAE.Click += new System.EventHandler(this.buttonExitAE_Click);
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
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(224, 164);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(172, 22);
            this.textBox3.TabIndex = 39;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(27, 320);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(236, 22);
            this.textBox4.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "Nombre";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 16);
            this.label3.TabIndex = 42;
            this.label3.Text = "Apellido";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 16);
            this.label4.TabIndex = 43;
            this.label4.Text = "Número de documento";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "Sueldo";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(264, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 35);
            this.button1.TabIndex = 45;
            this.button1.Text = "Agregar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // AddEmpleadosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(425, 483);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddEmpleadosForm";
            this.Text = "AddEmpleadosForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAE;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}