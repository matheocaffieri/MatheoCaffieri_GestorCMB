namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class HistorialInformeItemControl
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
            this.labelFecha = new System.Windows.Forms.Label();
            this.labelNombre = new System.Windows.Forms.Label();
            this.labelEstado = new System.Windows.Forms.Label();
            this.textBoxMateriales = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            // labelFecha
            this.labelFecha.AutoSize = true;
            this.labelFecha.Location = new System.Drawing.Point(10, 10);
            this.labelFecha.Size = new System.Drawing.Size(120, 20);
            this.labelFecha.Name = "labelFecha";

            // labelNombre
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(10, 34);
            this.labelNombre.Size = new System.Drawing.Size(200, 20);
            this.labelNombre.Name = "labelNombre";

            // labelEstado
            this.labelEstado.AutoSize = false;
            this.labelEstado.Location = new System.Drawing.Point(10, 60);
            this.labelEstado.Size = new System.Drawing.Size(170, 24);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.ForeColor = System.Drawing.Color.White;

            // textBoxMateriales
            this.textBoxMateriales.Location = new System.Drawing.Point(10, 94);
            this.textBoxMateriales.Size = new System.Drawing.Size(300, 90);
            this.textBoxMateriales.Name = "textBoxMateriales";

            // HistorialInformeItemControl
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.textBoxMateriales);
            this.Controls.Add(this.labelEstado);
            this.Controls.Add(this.labelNombre);
            this.Controls.Add(this.labelFecha);
            this.Name = "HistorialInformeItemControl";
            this.Size = new System.Drawing.Size(330, 195);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelFecha;
        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.TextBox textBoxMateriales;
    }
}
