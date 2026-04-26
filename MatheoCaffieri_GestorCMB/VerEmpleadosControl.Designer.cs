namespace MatheoCaffieri_GestorCMB
{
    partial class VerEmpleadosControl
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
            this.textBox1              = new System.Windows.Forms.TextBox();
            this.buttonAgregarEmpleado = new System.Windows.Forms.Button();
            this.empleadosLayoutPanel  = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.Name = "VerEmpleadosControl";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.VerEmpleadosControl_Load);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox        textBox1;
        private System.Windows.Forms.Button         buttonAgregarEmpleado;
        private System.Windows.Forms.FlowLayoutPanel empleadosLayoutPanel;
    }
}
