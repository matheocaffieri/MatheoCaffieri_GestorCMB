namespace MatheoCaffieri_GestorCMB
{
    partial class VerEmpleadosControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonSearchEmpleado = new System.Windows.Forms.Button();
            this.buttonAgregarEmpleado = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.empleadosLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 70);
            this.label1.TabIndex = 23;
            this.label1.Text = "Personal";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(348, 37);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(611, 30);
            this.textBox1.TabIndex = 26;
            // 
            // buttonSearchEmpleado
            // 
            this.buttonSearchEmpleado.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.buttonSearchEmpleado.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.search_logo;
            this.buttonSearchEmpleado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSearchEmpleado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearchEmpleado.Location = new System.Drawing.Point(965, 37);
            this.buttonSearchEmpleado.Name = "buttonSearchEmpleado";
            this.buttonSearchEmpleado.Size = new System.Drawing.Size(36, 36);
            this.buttonSearchEmpleado.TabIndex = 27;
            this.buttonSearchEmpleado.UseVisualStyleBackColor = false;
            // 
            // buttonAgregarEmpleado
            // 
            this.buttonAgregarEmpleado.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonAgregarEmpleado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAgregarEmpleado.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAgregarEmpleado.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonAgregarEmpleado.Location = new System.Drawing.Point(792, 116);
            this.buttonAgregarEmpleado.Name = "buttonAgregarEmpleado";
            this.buttonAgregarEmpleado.Size = new System.Drawing.Size(239, 52);
            this.buttonAgregarEmpleado.TabIndex = 28;
            this.buttonAgregarEmpleado.Text = "Agregar empleado";
            this.buttonAgregarEmpleado.UseVisualStyleBackColor = false;
            this.buttonAgregarEmpleado.Click += new System.EventHandler(this.buttonAgregarEmpleado_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 39);
            this.label2.TabIndex = 29;
            this.label2.Text = "Empleados";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(246, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(472, 39);
            this.label3.TabIndex = 30;
            this.label3.Text = "___________________________________";
            // 
            // empleadosLayoutPanel
            // 
            this.empleadosLayoutPanel.AutoScroll = true;
            this.empleadosLayoutPanel.Location = new System.Drawing.Point(39, 192);
            this.empleadosLayoutPanel.Name = "empleadosLayoutPanel";
            this.empleadosLayoutPanel.Size = new System.Drawing.Size(959, 246);
            this.empleadosLayoutPanel.TabIndex = 32;
            // 
            // VerEmpleadosControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.empleadosLayoutPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAgregarEmpleado);
            this.Controls.Add(this.buttonSearchEmpleado);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "VerEmpleadosControl";
            this.Size = new System.Drawing.Size(853, 352);
            this.Load += new System.EventHandler(this.VerEmpleadosControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonSearchEmpleado;
        private System.Windows.Forms.Button buttonAgregarEmpleado;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel empleadosLayoutPanel;
    }
}
