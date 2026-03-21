namespace MatheoCaffieri_GestorCMB
{
    partial class EditProyectoForm
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
            this.labelCliente = new System.Windows.Forms.Label();
            this.comboBoxCliente = new System.Windows.Forms.ComboBox();
            this.labelUbicacion = new System.Windows.Forms.Label();
            this.textBoxUbicacion = new System.Windows.Forms.TextBox();
            this.labelFechaInicio = new System.Windows.Forms.Label();
            this.dateTimePickerFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.labelEstado = new System.Windows.Forms.Label();
            this.comboBoxEstado = new System.Windows.Forms.ComboBox();
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
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(520, 32);
            this.FormPanel.TabIndex = 0;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            //
            // buttonExitEP
            //
            this.buttonExitEP.Location = new System.Drawing.Point(480, 2);
            this.buttonExitEP.Name = "buttonExitEP";
            this.buttonExitEP.Size = new System.Drawing.Size(28, 23);
            this.buttonExitEP.TabIndex = 0;
            this.buttonExitEP.Text = "X";
            this.buttonExitEP.UseVisualStyleBackColor = true;
            this.buttonExitEP.Click += new System.EventHandler(this.buttonExitEP_Click);
            //
            // labelTitulo
            //
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            this.labelTitulo.Location = new System.Drawing.Point(13, 48);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.TabIndex = 1;
            this.labelTitulo.Text = "Editar proyecto";
            //
            // labelDescripcion
            //
            this.labelDescripcion.AutoSize = true;
            this.labelDescripcion.Location = new System.Drawing.Point(24, 110);
            this.labelDescripcion.Name = "labelDescripcion";
            this.labelDescripcion.TabIndex = 2;
            this.labelDescripcion.Text = "Descripción del proyecto";
            //
            // textBoxDescripcion
            //
            this.textBoxDescripcion.Location = new System.Drawing.Point(27, 134);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(220, 22);
            this.textBoxDescripcion.TabIndex = 3;
            //
            // labelCliente
            //
            this.labelCliente.AutoSize = true;
            this.labelCliente.Location = new System.Drawing.Point(270, 110);
            this.labelCliente.Name = "labelCliente";
            this.labelCliente.TabIndex = 4;
            this.labelCliente.Text = "Cliente";
            //
            // comboBoxCliente
            //
            this.comboBoxCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCliente.FormattingEnabled = true;
            this.comboBoxCliente.Location = new System.Drawing.Point(273, 134);
            this.comboBoxCliente.Name = "comboBoxCliente";
            this.comboBoxCliente.Size = new System.Drawing.Size(220, 24);
            this.comboBoxCliente.TabIndex = 5;
            //
            // labelUbicacion
            //
            this.labelUbicacion.AutoSize = true;
            this.labelUbicacion.Location = new System.Drawing.Point(24, 180);
            this.labelUbicacion.Name = "labelUbicacion";
            this.labelUbicacion.TabIndex = 6;
            this.labelUbicacion.Text = "Ubicación";
            //
            // textBoxUbicacion
            //
            this.textBoxUbicacion.Location = new System.Drawing.Point(27, 204);
            this.textBoxUbicacion.Name = "textBoxUbicacion";
            this.textBoxUbicacion.Size = new System.Drawing.Size(466, 22);
            this.textBoxUbicacion.TabIndex = 7;
            //
            // labelFechaInicio
            //
            this.labelFechaInicio.AutoSize = true;
            this.labelFechaInicio.Location = new System.Drawing.Point(24, 250);
            this.labelFechaInicio.Name = "labelFechaInicio";
            this.labelFechaInicio.TabIndex = 8;
            this.labelFechaInicio.Text = "Fecha de inicio";
            //
            // dateTimePickerFechaInicio
            //
            this.dateTimePickerFechaInicio.Location = new System.Drawing.Point(27, 274);
            this.dateTimePickerFechaInicio.Name = "dateTimePickerFechaInicio";
            this.dateTimePickerFechaInicio.Size = new System.Drawing.Size(220, 22);
            this.dateTimePickerFechaInicio.TabIndex = 9;
            //
            // labelEstado
            //
            this.labelEstado.AutoSize = true;
            this.labelEstado.Location = new System.Drawing.Point(270, 250);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.TabIndex = 10;
            this.labelEstado.Text = "Estado";
            //
            // comboBoxEstado
            //
            this.comboBoxEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEstado.FormattingEnabled = true;
            this.comboBoxEstado.Location = new System.Drawing.Point(273, 274);
            this.comboBoxEstado.Name = "comboBoxEstado";
            this.comboBoxEstado.Size = new System.Drawing.Size(220, 24);
            this.comboBoxEstado.TabIndex = 11;
            //
            // buttonGuardar
            //
            this.buttonGuardar.Location = new System.Drawing.Point(381, 340);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(112, 34);
            this.buttonGuardar.TabIndex = 12;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.UseVisualStyleBackColor = true;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);
            //
            // EditProyectoForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(520, 395);
            this.Controls.Add(this.FormPanel);
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.labelDescripcion);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.labelCliente);
            this.Controls.Add(this.comboBoxCliente);
            this.Controls.Add(this.labelUbicacion);
            this.Controls.Add(this.textBoxUbicacion);
            this.Controls.Add(this.labelFechaInicio);
            this.Controls.Add(this.dateTimePickerFechaInicio);
            this.Controls.Add(this.labelEstado);
            this.Controls.Add(this.comboBoxEstado);
            this.Controls.Add(this.buttonGuardar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditProyectoForm";
            this.Text = "EditProyectoForm";
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
        private System.Windows.Forms.Label labelCliente;
        private System.Windows.Forms.ComboBox comboBoxCliente;
        private System.Windows.Forms.Label labelUbicacion;
        private System.Windows.Forms.TextBox textBoxUbicacion;
        private System.Windows.Forms.Label labelFechaInicio;
        private System.Windows.Forms.DateTimePicker dateTimePickerFechaInicio;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.ComboBox comboBoxEstado;
        private System.Windows.Forms.Button buttonGuardar;
    }
}
