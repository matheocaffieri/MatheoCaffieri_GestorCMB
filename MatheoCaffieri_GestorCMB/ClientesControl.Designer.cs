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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientesControl));
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
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxBuscar
            // 
            resources.ApplyResources(this.textBoxBuscar, "textBoxBuscar");
            this.textBoxBuscar.Name = "textBoxBuscar";
            this.textBoxBuscar.TextChanged += new System.EventHandler(this.textBoxBuscar_TextChanged);
            this.textBoxBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxBuscar_KeyDown);
            // 
            // buttonSearchClientes
            // 
            resources.ApplyResources(this.buttonSearchClientes, "buttonSearchClientes");
            this.buttonSearchClientes.Name = "buttonSearchClientes";
            this.buttonSearchClientes.UseVisualStyleBackColor = true;
            this.buttonSearchClientes.Click += new System.EventHandler(this.buttonSearchClientes_Click);
            // 
            // buttonBack
            // 
            resources.ApplyResources(this.buttonBack, "buttonBack");
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // textBoxRazonSocial
            // 
            resources.ApplyResources(this.textBoxRazonSocial, "textBoxRazonSocial");
            this.textBoxRazonSocial.Name = "textBoxRazonSocial";
            // 
            // textBoxTelefono
            // 
            resources.ApplyResources(this.textBoxTelefono, "textBoxTelefono");
            this.textBoxTelefono.Name = "textBoxTelefono";
            // 
            // textBoxMail
            // 
            resources.ApplyResources(this.textBoxMail, "textBoxMail");
            this.textBoxMail.Name = "textBoxMail";
            // 
            // textBoxNombreContacto
            // 
            resources.ApplyResources(this.textBoxNombreContacto, "textBoxNombreContacto");
            this.textBoxNombreContacto.Name = "textBoxNombreContacto";
            // 
            // buttonAddCliente
            // 
            resources.ApplyResources(this.buttonAddCliente, "buttonAddCliente");
            this.buttonAddCliente.Name = "buttonAddCliente";
            this.buttonAddCliente.UseVisualStyleBackColor = true;
            this.buttonAddCliente.Click += new System.EventHandler(this.buttonAddCliente_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // gestionarClientesLayoutPanel
            // 
            resources.ApplyResources(this.gestionarClientesLayoutPanel, "gestionarClientesLayoutPanel");
            this.gestionarClientesLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.gestionarClientesLayoutPanel.Name = "gestionarClientesLayoutPanel";
            // 
            // ClientesControl
            // 
            resources.ApplyResources(this, "$this");
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
            this.Name = "ClientesControl";
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
