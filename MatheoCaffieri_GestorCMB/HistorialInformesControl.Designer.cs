namespace MatheoCaffieri_GestorCMB
{
    partial class HistorialInformesControl
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
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.textBoxBuscar = new System.Windows.Forms.TextBox();
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.historialLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            // buttonBack
            this.buttonBack.Location = new System.Drawing.Point(29, 26);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(48, 42);
            this.buttonBack.TabIndex = 37;
            this.buttonBack.Text = "«";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);

            // labelTitulo
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Location = new System.Drawing.Point(84, 26);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(244, 31);
            this.labelTitulo.TabIndex = 34;
            this.labelTitulo.Text = "Historial de informes";

            // textBoxBuscar
            this.textBoxBuscar.Location = new System.Drawing.Point(409, 34);
            this.textBoxBuscar.Name = "textBoxBuscar";
            this.textBoxBuscar.Size = new System.Drawing.Size(540, 26);
            this.textBoxBuscar.TabIndex = 35;

            // buttonBuscar
            this.buttonBuscar.Location = new System.Drawing.Point(967, 31);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(69, 42);
            this.buttonBuscar.TabIndex = 36;
            this.buttonBuscar.Text = "›";
            this.buttonBuscar.UseVisualStyleBackColor = true;

            // historialLayoutPanel
            this.historialLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.historialLayoutPanel.AutoScroll = true;
            this.historialLayoutPanel.Location = new System.Drawing.Point(91, 100);
            this.historialLayoutPanel.Name = "historialLayoutPanel";
            this.historialLayoutPanel.Size = new System.Drawing.Size(973, 338);
            this.historialLayoutPanel.TabIndex = 39;

            // HistorialInformesControl
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.historialLayoutPanel);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonBuscar);
            this.Controls.Add(this.textBoxBuscar);
            this.Controls.Add(this.labelTitulo);
            this.Name = "HistorialInformesControl";
            this.Size = new System.Drawing.Size(1067, 441);
            this.Load += new System.EventHandler(this.HistorialInformesControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.TextBox textBoxBuscar;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.FlowLayoutPanel historialLayoutPanel;
    }
}
