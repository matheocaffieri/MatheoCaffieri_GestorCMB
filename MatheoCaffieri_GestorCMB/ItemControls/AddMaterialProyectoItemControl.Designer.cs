namespace MatheoCaffieri_GestorCMB.ItemControls
{
    partial class AddMaterialProyectoItemControl
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
            this.labelInfoGeneralArticulo = new System.Windows.Forms.Label();
            this.labelInfoTipoMat = new System.Windows.Forms.Label();
            this.labelInfoDescArt = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAgregarMaterial = new System.Windows.Forms.Button();
            this.buttonDecreaseQ = new System.Windows.Forms.Button();
            this.buttonIncreaseQ = new System.Windows.Forms.Button();
            this.labelInfoCantidadInventario = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelInfoGeneralArticulo
            // 
            this.labelInfoGeneralArticulo.AutoSize = true;
            this.labelInfoGeneralArticulo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoGeneralArticulo.Location = new System.Drawing.Point(166, 16);
            this.labelInfoGeneralArticulo.Name = "labelInfoGeneralArticulo";
            this.labelInfoGeneralArticulo.Size = new System.Drawing.Size(153, 20);
            this.labelInfoGeneralArticulo.TabIndex = 29;
            this.labelInfoGeneralArticulo.Text = "| Luz y Deco | $10.000";
            // 
            // labelInfoTipoMat
            // 
            this.labelInfoTipoMat.AutoSize = true;
            this.labelInfoTipoMat.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoTipoMat.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelInfoTipoMat.Location = new System.Drawing.Point(102, 16);
            this.labelInfoTipoMat.Name = "labelInfoTipoMat";
            this.labelInfoTipoMat.Size = new System.Drawing.Size(67, 20);
            this.labelInfoTipoMat.TabIndex = 28;
            this.labelInfoTipoMat.Text = "Lampara";
            // 
            // labelInfoDescArt
            // 
            this.labelInfoDescArt.AutoSize = true;
            this.labelInfoDescArt.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoDescArt.Location = new System.Drawing.Point(13, 16);
            this.labelInfoDescArt.Name = "labelInfoDescArt";
            this.labelInfoDescArt.Size = new System.Drawing.Size(93, 20);
            this.labelInfoDescArt.TabIndex = 27;
            this.labelInfoDescArt.Text = "Lampara fria";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.buttonAgregarMaterial);
            this.panel1.Controls.Add(this.buttonDecreaseQ);
            this.panel1.Controls.Add(this.buttonIncreaseQ);
            this.panel1.Controls.Add(this.labelInfoCantidadInventario);
            this.panel1.Location = new System.Drawing.Point(348, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(160, 60);
            this.panel1.TabIndex = 30;
            // 
            // buttonAgregarMaterial
            // 
            this.buttonAgregarMaterial.Location = new System.Drawing.Point(6, 13);
            this.buttonAgregarMaterial.Name = "buttonAgregarMaterial";
            this.buttonAgregarMaterial.Size = new System.Drawing.Size(54, 31);
            this.buttonAgregarMaterial.TabIndex = 31;
            this.buttonAgregarMaterial.Text = "Agregar";
            this.buttonAgregarMaterial.UseVisualStyleBackColor = true;
            this.buttonAgregarMaterial.Click += new System.EventHandler(this.buttonAgregarMaterial_Click);
            // 
            // buttonDecreaseQ
            // 
            this.buttonDecreaseQ.BackColor = System.Drawing.Color.Transparent;
            this.buttonDecreaseQ.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.down_logo;
            this.buttonDecreaseQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonDecreaseQ.FlatAppearance.BorderSize = 0;
            this.buttonDecreaseQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDecreaseQ.Location = new System.Drawing.Point(101, 29);
            this.buttonDecreaseQ.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDecreaseQ.Name = "buttonDecreaseQ";
            this.buttonDecreaseQ.Size = new System.Drawing.Size(56, 29);
            this.buttonDecreaseQ.TabIndex = 30;
            this.buttonDecreaseQ.UseVisualStyleBackColor = false;
            this.buttonDecreaseQ.Click += new System.EventHandler(this.buttonDecreaseQ_Click);
            // 
            // buttonIncreaseQ
            // 
            this.buttonIncreaseQ.BackColor = System.Drawing.Color.Transparent;
            this.buttonIncreaseQ.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.up_logo;
            this.buttonIncreaseQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonIncreaseQ.FlatAppearance.BorderSize = 0;
            this.buttonIncreaseQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIncreaseQ.Location = new System.Drawing.Point(101, 2);
            this.buttonIncreaseQ.Margin = new System.Windows.Forms.Padding(2);
            this.buttonIncreaseQ.Name = "buttonIncreaseQ";
            this.buttonIncreaseQ.Size = new System.Drawing.Size(56, 29);
            this.buttonIncreaseQ.TabIndex = 29;
            this.buttonIncreaseQ.UseVisualStyleBackColor = false;
            this.buttonIncreaseQ.Click += new System.EventHandler(this.buttonIncreaseQ_Click);
            // 
            // labelInfoCantidadInventario
            // 
            this.labelInfoCantidadInventario.AutoSize = true;
            this.labelInfoCantidadInventario.Font = new System.Drawing.Font("Microsoft YaHei UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoCantidadInventario.Location = new System.Drawing.Point(67, 8);
            this.labelInfoCantidadInventario.Name = "labelInfoCantidadInventario";
            this.labelInfoCantidadInventario.Size = new System.Drawing.Size(32, 36);
            this.labelInfoCantidadInventario.TabIndex = 28;
            this.labelInfoCantidadInventario.Text = "3";
            // 
            // AddMaterialProyectoItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelInfoGeneralArticulo);
            this.Controls.Add(this.labelInfoTipoMat);
            this.Controls.Add(this.labelInfoDescArt);
            this.Name = "AddMaterialProyectoItemControl";
            this.Size = new System.Drawing.Size(509, 60);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfoGeneralArticulo;
        private System.Windows.Forms.Label labelInfoTipoMat;
        private System.Windows.Forms.Label labelInfoDescArt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonDecreaseQ;
        private System.Windows.Forms.Button buttonIncreaseQ;
        private System.Windows.Forms.Label labelInfoCantidadInventario;
        private System.Windows.Forms.Button buttonAgregarMaterial;
    }
}
