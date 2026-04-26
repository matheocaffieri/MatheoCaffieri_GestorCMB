namespace MatheoCaffieri_GestorCMB
{
    partial class ProveedorControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxDescripcion   = new System.Windows.Forms.TextBox();
            this.textBoxTelefono      = new System.Windows.Forms.TextBox();
            this.buttonAddProveedor   = new System.Windows.Forms.Button();
            this.buttonSearchClientes = new System.Windows.Forms.Button();
            this.textBox1             = new System.Windows.Forms.TextBox();
            this.proveedorLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.Name = "ProveedorControl";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.ProveedorControl_Load);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox         textBoxDescripcion;
        private System.Windows.Forms.TextBox         textBoxTelefono;
        private System.Windows.Forms.Button          buttonAddProveedor;
        private System.Windows.Forms.Button          buttonSearchClientes;
        private System.Windows.Forms.TextBox         textBox1;
        private System.Windows.Forms.FlowLayoutPanel proveedorLayoutPanel;
    }
}
