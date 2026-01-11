namespace MatheoCaffieri_GestorCMB
{
    partial class LoginForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textMail = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonForgotPassword = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(170, 221);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.c_m_b;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(31, 54);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(105, 111);
            this.panel2.TabIndex = 5;
            // 
            // textMail
            // 
            this.textMail.Location = new System.Drawing.Point(202, 54);
            this.textMail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textMail.Name = "textMail";
            this.textMail.Size = new System.Drawing.Size(277, 20);
            this.textMail.TabIndex = 1;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(202, 124);
            this.textPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(277, 20);
            this.textPassword.TabIndex = 2;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(419, 169);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(83, 21);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Iniciar sesión";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonForgotPassword
            // 
            this.buttonForgotPassword.Location = new System.Drawing.Point(203, 169);
            this.buttonForgotPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonForgotPassword.Name = "buttonForgotPassword";
            this.buttonForgotPassword.Size = new System.Drawing.Size(140, 21);
            this.buttonForgotPassword.TabIndex = 4;
            this.buttonForgotPassword.Text = "¿Olvidó su contraseña?";
            this.buttonForgotPassword.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mail";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(200, 99);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(61, 13);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Contraseña";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 221);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonForgotPassword);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textMail);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textMail;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonForgotPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPassword;
    }
}