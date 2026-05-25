namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class DetalleMaterialHeaderControl
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
            this.labelDescripcion = new System.Windows.Forms.Label();
            this.labelCategoria   = new System.Windows.Forms.Label();
            this.labelUnidad      = new System.Windows.Forms.Label();
            this.labelCantidad    = new System.Windows.Forms.Label();
            this.labelPrecio      = new System.Windows.Forms.Label();
            this.labelGanancia    = new System.Windows.Forms.Label();
            this.labelSep1        = new System.Windows.Forms.Label();
            this.labelSep2        = new System.Windows.Forms.Label();
            this.labelSep3        = new System.Windows.Forms.Label();
            this.labelSep4        = new System.Windows.Forms.Label();
            this.labelSep5        = new System.Windows.Forms.Label();
            this.SuspendLayout();

            var font  = new System.Drawing.Font("Microsoft YaHei UI", 7F);
            var color = System.Drawing.Color.Gray;

            this.labelDescripcion.Font         = font;
            this.labelDescripcion.ForeColor    = color;
            this.labelDescripcion.AutoEllipsis = true;
            this.labelDescripcion.Text         = "Descripción";
            this.labelDescripcion.TextAlign    = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDescripcion.BackColor    = System.Drawing.Color.Transparent;

            this.labelCategoria.Font         = font;
            this.labelCategoria.ForeColor    = color;
            this.labelCategoria.AutoEllipsis = true;
            this.labelCategoria.Text         = "Categoría";
            this.labelCategoria.TextAlign    = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelCategoria.BackColor    = System.Drawing.Color.Transparent;

            this.labelUnidad.Font         = font;
            this.labelUnidad.ForeColor    = color;
            this.labelUnidad.AutoEllipsis = true;
            this.labelUnidad.Text         = "Unidad";
            this.labelUnidad.TextAlign    = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelUnidad.BackColor    = System.Drawing.Color.Transparent;

            this.labelCantidad.Font         = font;
            this.labelCantidad.ForeColor    = color;
            this.labelCantidad.AutoEllipsis = true;
            this.labelCantidad.Text         = "Cant.";
            this.labelCantidad.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelCantidad.BackColor    = System.Drawing.Color.Transparent;

            this.labelPrecio.Font         = font;
            this.labelPrecio.ForeColor    = color;
            this.labelPrecio.AutoEllipsis = true;
            this.labelPrecio.Text         = "Precio/U";
            this.labelPrecio.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelPrecio.BackColor    = System.Drawing.Color.Transparent;

            this.labelGanancia.Font         = font;
            this.labelGanancia.ForeColor    = color;
            this.labelGanancia.AutoEllipsis = true;
            this.labelGanancia.Text         = "Ganancia/U";
            this.labelGanancia.TextAlign    = System.Drawing.ContentAlignment.MiddleRight;
            this.labelGanancia.BackColor    = System.Drawing.Color.Transparent;

            ConfigurarSeparador(this.labelSep1, font, color);
            ConfigurarSeparador(this.labelSep2, font, color);
            ConfigurarSeparador(this.labelSep3, font, color);
            ConfigurarSeparador(this.labelSep4, font, color);
            ConfigurarSeparador(this.labelSep5, font, color);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Margin    = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Controls.Add(this.labelSep5);
            this.Controls.Add(this.labelSep4);
            this.Controls.Add(this.labelSep3);
            this.Controls.Add(this.labelSep2);
            this.Controls.Add(this.labelSep1);
            this.Controls.Add(this.labelGanancia);
            this.Controls.Add(this.labelPrecio);
            this.Controls.Add(this.labelCantidad);
            this.Controls.Add(this.labelUnidad);
            this.Controls.Add(this.labelCategoria);
            this.Controls.Add(this.labelDescripcion);
            this.Name = "DetalleMaterialHeaderControl";
            this.Size = new System.Drawing.Size(420, 16);
            this.ResumeLayout(false);
        }

        private static void ConfigurarSeparador(System.Windows.Forms.Label lbl, System.Drawing.Font font, System.Drawing.Color color)
        {
            lbl.Font      = font;
            lbl.ForeColor = color;
            lbl.Text      = "|";
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.BackColor = System.Drawing.Color.Transparent;
        }

        #endregion

        private System.Windows.Forms.Label labelDescripcion;
        private System.Windows.Forms.Label labelCategoria;
        private System.Windows.Forms.Label labelUnidad;
        private System.Windows.Forms.Label labelCantidad;
        private System.Windows.Forms.Label labelPrecio;
        private System.Windows.Forms.Label labelGanancia;
        private System.Windows.Forms.Label labelSep1;
        private System.Windows.Forms.Label labelSep2;
        private System.Windows.Forms.Label labelSep3;
        private System.Windows.Forms.Label labelSep4;
        private System.Windows.Forms.Label labelSep5;
    }
}
