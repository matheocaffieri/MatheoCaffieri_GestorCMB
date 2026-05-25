namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class DetalleEmpleadoHeaderControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.labelNombre    = new System.Windows.Forms.Label();
            this.labelDocumento = new System.Windows.Forms.Label();
            this.labelSueldo    = new System.Windows.Forms.Label();
            this.labelGanancia  = new System.Windows.Forms.Label();
            this.labelSep1      = new System.Windows.Forms.Label();
            this.labelSep2      = new System.Windows.Forms.Label();
            this.labelSep3      = new System.Windows.Forms.Label();
            this.SuspendLayout();

            var font  = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            var color = System.Drawing.Color.Gray;

            this.labelNombre.Font         = font;
            this.labelNombre.ForeColor    = color;
            this.labelNombre.AutoEllipsis = true;
            this.labelNombre.Text         = "Nombre";
            this.labelNombre.TextAlign    = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelNombre.BackColor    = System.Drawing.Color.Transparent;

            this.labelDocumento.Font         = font;
            this.labelDocumento.ForeColor    = color;
            this.labelDocumento.AutoEllipsis = true;
            this.labelDocumento.Text         = "Documento";
            this.labelDocumento.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDocumento.BackColor    = System.Drawing.Color.Transparent;

            this.labelSueldo.Font         = font;
            this.labelSueldo.ForeColor    = color;
            this.labelSueldo.AutoEllipsis = true;
            this.labelSueldo.Text         = "Sueldo";
            this.labelSueldo.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelSueldo.BackColor    = System.Drawing.Color.Transparent;

            this.labelGanancia.Font         = font;
            this.labelGanancia.ForeColor    = color;
            this.labelGanancia.AutoEllipsis = true;
            this.labelGanancia.Text         = "Ganancia";
            this.labelGanancia.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelGanancia.BackColor    = System.Drawing.Color.Transparent;

            this.labelSep1.Font      = font;
            this.labelSep1.ForeColor = color;
            this.labelSep1.Text      = "|";
            this.labelSep1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSep1.BackColor = System.Drawing.Color.Transparent;

            this.labelSep2.Font      = font;
            this.labelSep2.ForeColor = color;
            this.labelSep2.Text      = "|";
            this.labelSep2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSep2.BackColor = System.Drawing.Color.Transparent;

            this.labelSep3.Font      = font;
            this.labelSep3.ForeColor = color;
            this.labelSep3.Text      = "|";
            this.labelSep3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSep3.BackColor = System.Drawing.Color.Transparent;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Margin    = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Controls.Add(this.labelSep3);
            this.Controls.Add(this.labelSep2);
            this.Controls.Add(this.labelSep1);
            this.Controls.Add(this.labelGanancia);
            this.Controls.Add(this.labelSueldo);
            this.Controls.Add(this.labelDocumento);
            this.Controls.Add(this.labelNombre);
            this.Name = "DetalleEmpleadoHeaderControl";
            this.Size = new System.Drawing.Size(415, 16);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.Label labelDocumento;
        private System.Windows.Forms.Label labelSueldo;
        private System.Windows.Forms.Label labelGanancia;
        private System.Windows.Forms.Label labelSep1;
        private System.Windows.Forms.Label labelSep2;
        private System.Windows.Forms.Label labelSep3;
    }
}
