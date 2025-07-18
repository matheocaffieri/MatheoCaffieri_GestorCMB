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
            this.comboBoxLang = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textMail = new System.Windows.Forms.TextBox();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonForgotPassword = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonVerContraseña = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.comboBoxLang);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 272);
            this.panel1.TabIndex = 0;
            // 
            // comboBoxLang
            // 
            this.comboBoxLang.FormattingEnabled = true;
            this.comboBoxLang.Location = new System.Drawing.Point(144, 209);
            this.comboBoxLang.Name = "comboBoxLang";
            this.comboBoxLang.Size = new System.Drawing.Size(58, 24);
            this.comboBoxLang.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.c_m_b;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(41, 66);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(140, 137);
            this.panel2.TabIndex = 5;
            // 
            // textMail
            // 
            this.textMail.Location = new System.Drawing.Point(270, 66);
            this.textMail.Name = "textMail";
            this.textMail.Size = new System.Drawing.Size(368, 22);
            this.textMail.TabIndex = 1;
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(270, 153);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(323, 22);
            this.textPassword.TabIndex = 2;
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(270, 207);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(111, 26);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Iniciar sesión";
            this.buttonLogin.UseVisualStyleBackColor = true;
            // 
            // buttonForgotPassword
            // 
            this.buttonForgotPassword.Location = new System.Drawing.Point(483, 207);
            this.buttonForgotPassword.Name = "buttonForgotPassword";
            this.buttonForgotPassword.Size = new System.Drawing.Size(155, 26);
            this.buttonForgotPassword.TabIndex = 4;
            this.buttonForgotPassword.Text = "¿Olvidó su contraseña?";
            this.buttonForgotPassword.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(267, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Contraseña";
            // 
            // buttonVerContraseña
            // 
            this.buttonVerContraseña.Location = new System.Drawing.Point(599, 151);
            this.buttonVerContraseña.Name = "buttonVerContraseña";
            this.buttonVerContraseña.Size = new System.Drawing.Size(39, 26);
            this.buttonVerContraseña.TabIndex = 7;
            this.buttonVerContraseña.Text = "Iniciar sesión";
            this.buttonVerContraseña.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 272);
            this.Controls.Add(this.buttonVerContraseña);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonForgotPassword);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.textMail);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.Button buttonVerContraseña;
    }
}