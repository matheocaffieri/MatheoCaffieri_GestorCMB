namespace MatheoCaffieri_GestorCMB
{
    partial class EditProveedorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExitEP = new System.Windows.Forms.Button();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.labelDescripcion = new System.Windows.Forms.Label();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.labelTelefono = new System.Windows.Forms.Label();
            this.textBoxTelefono = new System.Windows.Forms.TextBox();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.FormPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // FormPanel
            //
            this.FormPanel.BackColor = System.Drawing.Color.DodgerBlue;
            this.FormPanel.Controls.Add(this.buttonExitEP);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(425, 32);
            this.FormPanel.TabIndex = 0;
            //
            // buttonExitEP
            //
            this.buttonExitEP.Location = new System.Drawing.Point(385, 2);
            this.buttonExitEP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExitEP.Name = "buttonExitEP";
            this.buttonExitEP.Size = new System.Drawing.Size(28, 23);
            this.buttonExitEP.TabIndex = 0;
            this.buttonExitEP.Text = "X";
            this.buttonExitEP.UseVisualStyleBackColor = true;
            //
            // labelTitulo
            //
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.labelTitulo.Location = new System.Drawing.Point(13, 52);
            this.labelTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(178, 27);
            this.labelTitulo.TabIndex = 1;
            this.labelTitulo.Text = "Editar proveedor";
            //
            // labelDescripcion
            //
            this.labelDescripcion.AutoSize = true;
            this.labelDescripcion.Location = new System.Drawing.Point(24, 110);
            this.labelDescripcion.Name = "labelDescripcion";
            this.labelDescripcion.Size = new System.Drawing.Size(71, 16);
            this.labelDescripcion.TabIndex = 2;
            this.labelDescripcion.Text = "Descripción";
            //
            // textBoxDescripcion
            //
            this.textBoxDescripcion.Location = new System.Drawing.Point(27, 136);
            this.textBoxDescripcion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(370, 22);
            this.textBoxDescripcion.TabIndex = 3;
            //
            // labelTelefono
            //
            this.labelTelefono.AutoSize = true;
            this.labelTelefono.Location = new System.Drawing.Point(24, 180);
            this.labelTelefono.Name = "labelTelefono";
            this.labelTelefono.Size = new System.Drawing.Size(60, 16);
            this.labelTelefono.TabIndex = 4;
            this.labelTelefono.Text = "Teléfono";
            //
            // textBoxTelefono
            //
            this.textBoxTelefono.Location = new System.Drawing.Point(27, 206);
            this.textBoxTelefono.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTelefono.Name = "textBoxTelefono";
            this.textBoxTelefono.Size = new System.Drawing.Size(370, 22);
            this.textBoxTelefono.TabIndex = 5;
            //
            // buttonGuardar
            //
            this.buttonGuardar.Location = new System.Drawing.Point(281, 260);
            this.buttonGuardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(116, 34);
            this.buttonGuardar.TabIndex = 6;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            //
            // EditProveedorForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(425, 312);
            this.Controls.Add(this.FormPanel);
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.labelDescripcion);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.labelTelefono);
            this.Controls.Add(this.textBoxTelefono);
            this.Controls.Add(this.buttonGuardar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditProveedorForm";
            this.Text = "EditProveedorForm";
            this.FormPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Button buttonExitEP;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelDescripcion;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.Label labelTelefono;
        private System.Windows.Forms.TextBox textBoxTelefono;
        private System.Windows.Forms.Button buttonGuardar;
    }
}
