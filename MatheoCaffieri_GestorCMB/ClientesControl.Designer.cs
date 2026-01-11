namespace MatheoCaffieri_GestorCMB
{
    partial class ClientesControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBuscar = new System.Windows.Forms.TextBox();
            this.buttonSearchClientes = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.textBoxRazonSocial = new System.Windows.Forms.TextBox();
            this.textBoxTelefono = new System.Windows.Forms.TextBox();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.textBoxNombreContacto = new System.Windows.Forms.TextBox();
            this.buttonAddCliente = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gestionarClientesLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 31);
            this.label2.TabIndex = 30;
            this.label2.Text = "Gestionar clientes";
            // 
            // textBoxBuscar
            // 
            this.textBoxBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBuscar.Location = new System.Drawing.Point(305, 37);
            this.textBoxBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxBuscar.Name = "textBoxBuscar";
            this.textBoxBuscar.Size = new System.Drawing.Size(406, 26);
            this.textBoxBuscar.TabIndex = 31;
            this.textBoxBuscar.TextChanged += new System.EventHandler(this.textBoxBuscar_TextChanged);
            this.textBoxBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBuscar_KeyDown);
            // 
            // buttonSearchClientes
            // 
            this.buttonSearchClientes.Location = new System.Drawing.Point(724, 34);
            this.buttonSearchClientes.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSearchClientes.Name = "buttonSearchClientes";
            this.buttonSearchClientes.Size = new System.Drawing.Size(59, 34);
            this.buttonSearchClientes.TabIndex = 32;
            this.buttonSearchClientes.Text = "Search";
            this.buttonSearchClientes.UseVisualStyleBackColor = true;
            this.buttonSearchClientes.Click += new System.EventHandler(this.buttonSearchClientes_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(20, 30);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(36, 34);
            this.buttonBack.TabIndex = 33;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // textBoxRazonSocial
            // 
            this.textBoxRazonSocial.Location = new System.Drawing.Point(20, 101);
            this.textBoxRazonSocial.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRazonSocial.Name = "textBoxRazonSocial";
            this.textBoxRazonSocial.Size = new System.Drawing.Size(207, 20);
            this.textBoxRazonSocial.TabIndex = 34;
            // 
            // textBoxTelefono
            // 
            this.textBoxTelefono.Location = new System.Drawing.Point(20, 160);
            this.textBoxTelefono.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTelefono.Name = "textBoxTelefono";
            this.textBoxTelefono.Size = new System.Drawing.Size(207, 20);
            this.textBoxTelefono.TabIndex = 35;
            // 
            // textBoxMail
            // 
            this.textBoxMail.Location = new System.Drawing.Point(20, 219);
            this.textBoxMail.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(207, 20);
            this.textBoxMail.TabIndex = 36;
            // 
            // textBoxNombreContacto
            // 
            this.textBoxNombreContacto.Location = new System.Drawing.Point(20, 280);
            this.textBoxNombreContacto.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNombreContacto.Name = "textBoxNombreContacto";
            this.textBoxNombreContacto.Size = new System.Drawing.Size(207, 20);
            this.textBoxNombreContacto.TabIndex = 37;
            // 
            // buttonAddCliente
            // 
            this.buttonAddCliente.Location = new System.Drawing.Point(20, 310);
            this.buttonAddCliente.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddCliente.Name = "buttonAddCliente";
            this.buttonAddCliente.Size = new System.Drawing.Size(86, 29);
            this.buttonAddCliente.TabIndex = 38;
            this.buttonAddCliente.Text = "Agregar";
            this.buttonAddCliente.UseVisualStyleBackColor = true;
            this.buttonAddCliente.Click += new System.EventHandler(this.buttonAddCliente_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(287, 76);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 244);
            this.panel1.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 135);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Teléfono";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Razón social";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 195);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Mail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 254);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "Nombre de contacto";
            // 
            // gestionarClientesLayoutPanel
            // 
            this.gestionarClientesLayoutPanel.AutoScroll = true;
            this.gestionarClientesLayoutPanel.Location = new System.Drawing.Point(314, 76);
            this.gestionarClientesLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.gestionarClientesLayoutPanel.Name = "gestionarClientesLayoutPanel";
            this.gestionarClientesLayoutPanel.Size = new System.Drawing.Size(411, 280);
            this.gestionarClientesLayoutPanel.TabIndex = 47;
            // 
            // ClientesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gestionarClientesLayoutPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonAddCliente);
            this.Controls.Add(this.textBoxNombreContacto);
            this.Controls.Add(this.textBoxMail);
            this.Controls.Add(this.textBoxTelefono);
            this.Controls.Add(this.textBoxRazonSocial);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSearchClientes);
            this.Controls.Add(this.textBoxBuscar);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ClientesControl";
            this.Size = new System.Drawing.Size(800, 358);
            this.Load += new System.EventHandler(this.ClientesControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBuscar;
        private System.Windows.Forms.Button buttonSearchClientes;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.TextBox textBoxRazonSocial;
        private System.Windows.Forms.TextBox textBoxTelefono;
        private System.Windows.Forms.TextBox textBoxMail;
        private System.Windows.Forms.TextBox textBoxNombreContacto;
        private System.Windows.Forms.Button buttonAddCliente;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel gestionarClientesLayoutPanel;
    }
}
