namespace MatheoCaffieri_GestorCMB
{
    partial class BackupForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelTitulo = new System.Windows.Forms.Label();
            this.labelSeleccione = new System.Windows.Forms.Label();
            this.checkedListBoxBases = new System.Windows.Forms.CheckedListBox();
            this.labelDestino = new System.Windows.Forms.Label();
            this.textBoxDestino = new System.Windows.Forms.TextBox();
            this.buttonExaminar = new System.Windows.Forms.Button();
            this.textBoxResultado = new System.Windows.Forms.TextBox();
            this.buttonGenerar = new System.Windows.Forms.Button();
            this.buttonCerrar = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // labelTitulo
            //
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(18, 15);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(240, 21);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Copia de seguridad";
            //
            // labelSeleccione
            //
            this.labelSeleccione.AutoSize = true;
            this.labelSeleccione.Location = new System.Drawing.Point(20, 50);
            this.labelSeleccione.Name = "labelSeleccione";
            this.labelSeleccione.Size = new System.Drawing.Size(230, 13);
            this.labelSeleccione.TabIndex = 1;
            this.labelSeleccione.Text = "Seleccioná las bases para copiar:";
            //
            // checkedListBoxBases
            //
            this.checkedListBoxBases.CheckOnClick = true;
            this.checkedListBoxBases.FormattingEnabled = true;
            this.checkedListBoxBases.Location = new System.Drawing.Point(20, 68);
            this.checkedListBoxBases.Name = "checkedListBoxBases";
            this.checkedListBoxBases.Size = new System.Drawing.Size(410, 94);
            this.checkedListBoxBases.TabIndex = 2;
            //
            // labelDestino
            //
            this.labelDestino.AutoSize = true;
            this.labelDestino.Location = new System.Drawing.Point(20, 178);
            this.labelDestino.Name = "labelDestino";
            this.labelDestino.Size = new System.Drawing.Size(80, 13);
            this.labelDestino.TabIndex = 3;
            this.labelDestino.Text = "Carpeta destino:";
            //
            // textBoxDestino
            //
            this.textBoxDestino.Location = new System.Drawing.Point(20, 196);
            this.textBoxDestino.Name = "textBoxDestino";
            this.textBoxDestino.ReadOnly = true;
            this.textBoxDestino.Size = new System.Drawing.Size(300, 20);
            this.textBoxDestino.TabIndex = 4;
            //
            // buttonExaminar
            //
            this.buttonExaminar.Location = new System.Drawing.Point(330, 194);
            this.buttonExaminar.Name = "buttonExaminar";
            this.buttonExaminar.Size = new System.Drawing.Size(100, 24);
            this.buttonExaminar.TabIndex = 5;
            this.buttonExaminar.Text = "Examinar...";
            this.buttonExaminar.UseVisualStyleBackColor = true;
            this.buttonExaminar.Click += new System.EventHandler(this.buttonExaminar_Click);
            //
            // textBoxResultado
            //
            this.textBoxResultado.Location = new System.Drawing.Point(20, 230);
            this.textBoxResultado.Multiline = true;
            this.textBoxResultado.Name = "textBoxResultado";
            this.textBoxResultado.ReadOnly = true;
            this.textBoxResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResultado.Size = new System.Drawing.Size(410, 70);
            this.textBoxResultado.TabIndex = 6;
            //
            // buttonGenerar
            //
            this.buttonGenerar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonGenerar.Location = new System.Drawing.Point(20, 315);
            this.buttonGenerar.Name = "buttonGenerar";
            this.buttonGenerar.Size = new System.Drawing.Size(130, 30);
            this.buttonGenerar.TabIndex = 7;
            this.buttonGenerar.Text = "Generar";
            this.buttonGenerar.UseVisualStyleBackColor = true;
            this.buttonGenerar.Click += new System.EventHandler(this.buttonGenerar_Click);
            //
            // buttonCerrar
            //
            this.buttonCerrar.Location = new System.Drawing.Point(340, 315);
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.Size = new System.Drawing.Size(90, 30);
            this.buttonCerrar.TabIndex = 8;
            this.buttonCerrar.Text = "Cerrar";
            this.buttonCerrar.UseVisualStyleBackColor = true;
            this.buttonCerrar.Click += new System.EventHandler(this.buttonCerrar_Click);
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(160, 323);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 13);
            this.labelStatus.TabIndex = 9;
            //
            // BackupForm
            //
            this.AcceptButton = this.buttonGenerar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCerrar;
            this.ClientSize = new System.Drawing.Size(452, 360);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.buttonGenerar);
            this.Controls.Add(this.textBoxResultado);
            this.Controls.Add(this.buttonExaminar);
            this.Controls.Add(this.textBoxDestino);
            this.Controls.Add(this.labelDestino);
            this.Controls.Add(this.checkedListBoxBases);
            this.Controls.Add(this.labelSeleccione);
            this.Controls.Add(this.labelTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copia de seguridad";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelSeleccione;
        private System.Windows.Forms.CheckedListBox checkedListBoxBases;
        private System.Windows.Forms.Label labelDestino;
        private System.Windows.Forms.TextBox textBoxDestino;
        private System.Windows.Forms.Button buttonExaminar;
        private System.Windows.Forms.TextBox textBoxResultado;
        private System.Windows.Forms.Button buttonGenerar;
        private System.Windows.Forms.Button buttonCerrar;
        private System.Windows.Forms.Label labelStatus;
    }
}
