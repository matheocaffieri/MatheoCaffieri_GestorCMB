namespace MatheoCaffieri_GestorCMB
{
    partial class EditUserForm
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
            this.buttonExitAE = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonAgregarRol = new System.Windows.Forms.Button();
            this.dataGridRoles = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxPermisoEdit = new System.Windows.Forms.ComboBox();
            this.buttonEditarUser = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxIdioma = new System.Windows.Forms.ComboBox();
            this.textBoxContraseñaEditUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTelEditUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMailEditUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FormPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRoles)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitAE);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(436, 32);
            this.FormPanel.TabIndex = 33;
            // 
            // buttonExitAE
            // 
            this.buttonExitAE.Location = new System.Drawing.Point(404, 6);
            this.buttonExitAE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExitAE.Name = "buttonExitAE";
            this.buttonExitAE.Size = new System.Drawing.Size(28, 23);
            this.buttonExitAE.TabIndex = 31;
            this.buttonExitAE.Text = "X";
            this.buttonExitAE.UseVisualStyleBackColor = true;
            this.buttonExitAE.Click += new System.EventHandler(this.buttonExitAE_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonAgregarRol);
            this.groupBox2.Controls.Add(this.dataGridRoles);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBoxPermisoEdit);
            this.groupBox2.Location = new System.Drawing.Point(23, 398);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(393, 261);
            this.groupBox2.TabIndex = 55;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Asignar rol";
            // 
            // buttonAgregarRol
            // 
            this.buttonAgregarRol.Location = new System.Drawing.Point(275, 217);
            this.buttonAgregarRol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAgregarRol.Name = "buttonAgregarRol";
            this.buttonAgregarRol.Size = new System.Drawing.Size(93, 34);
            this.buttonAgregarRol.TabIndex = 57;
            this.buttonAgregarRol.Text = "Agregar";
            this.buttonAgregarRol.UseVisualStyleBackColor = true;
            this.buttonAgregarRol.Click += new System.EventHandler(this.buttonAgregarRol_Click);
            // 
            // dataGridRoles
            // 
            this.dataGridRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridRoles.Location = new System.Drawing.Point(24, 96);
            this.dataGridRoles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridRoles.Name = "dataGridRoles";
            this.dataGridRoles.RowHeadersWidth = 51;
            this.dataGridRoles.Size = new System.Drawing.Size(344, 114);
            this.dataGridRoles.TabIndex = 56;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 16);
            this.label4.TabIndex = 55;
            this.label4.Text = "Seleccionar rol";
            // 
            // comboBoxPermisoEdit
            // 
            this.comboBoxPermisoEdit.FormattingEnabled = true;
            this.comboBoxPermisoEdit.Location = new System.Drawing.Point(23, 52);
            this.comboBoxPermisoEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxPermisoEdit.Name = "comboBoxPermisoEdit";
            this.comboBoxPermisoEdit.Size = new System.Drawing.Size(344, 24);
            this.comboBoxPermisoEdit.TabIndex = 53;
            // 
            // buttonEditarUser
            // 
            this.buttonEditarUser.Location = new System.Drawing.Point(323, 69);
            this.buttonEditarUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonEditarUser.Name = "buttonEditarUser";
            this.buttonEditarUser.Size = new System.Drawing.Size(93, 34);
            this.buttonEditarUser.TabIndex = 52;
            this.buttonEditarUser.Text = "Editar";
            this.buttonEditarUser.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBoxIdioma);
            this.groupBox1.Controls.Add(this.textBoxContraseñaEditUser);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxTelEditUser);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxMailEditUser);
            this.groupBox1.Location = new System.Drawing.Point(23, 128);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(393, 265);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Editar información";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(171, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 59;
            this.label6.Text = "Idioma";
            // 
            // comboBoxIdioma
            // 
            this.comboBoxIdioma.FormattingEnabled = true;
            this.comboBoxIdioma.Location = new System.Drawing.Point(225, 224);
            this.comboBoxIdioma.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxIdioma.Name = "comboBoxIdioma";
            this.comboBoxIdioma.Size = new System.Drawing.Size(141, 24);
            this.comboBoxIdioma.TabIndex = 58;
            // 
            // textBoxContraseñaEditUser
            // 
            this.textBoxContraseñaEditUser.Location = new System.Drawing.Point(23, 183);
            this.textBoxContraseñaEditUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxContraseñaEditUser.Name = "textBoxContraseñaEditUser";
            this.textBoxContraseñaEditUser.Size = new System.Drawing.Size(343, 22);
            this.textBoxContraseñaEditUser.TabIndex = 57;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Nueva contraseña";
            // 
            // textBoxTelEditUser
            // 
            this.textBoxTelEditUser.Location = new System.Drawing.Point(24, 110);
            this.textBoxTelEditUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTelEditUser.Name = "textBoxTelEditUser";
            this.textBoxTelEditUser.Size = new System.Drawing.Size(343, 22);
            this.textBoxTelEditUser.TabIndex = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "Teléfono";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "Mail";
            // 
            // textBoxMailEditUser
            // 
            this.textBoxMailEditUser.Location = new System.Drawing.Point(23, 52);
            this.textBoxMailEditUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxMailEditUser.Name = "textBoxMailEditUser";
            this.textBoxMailEditUser.Size = new System.Drawing.Size(344, 22);
            this.textBoxMailEditUser.TabIndex = 49;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 32);
            this.label2.TabIndex = 53;
            this.label2.Text = "Editar usuario";
            // 
            // EditUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 677);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonEditarUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditUserForm";
            this.Text = "EditUserForm";
            this.Load += new System.EventHandler(this.EditUserForm_Load);
            this.FormPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridRoles)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitAE;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPermisoEdit;
        private System.Windows.Forms.Button buttonEditarUser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxContraseñaEditUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTelEditUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMailEditUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxIdioma;
        private System.Windows.Forms.DataGridView dataGridRoles;
        private System.Windows.Forms.Button buttonAgregarRol;
    }
}