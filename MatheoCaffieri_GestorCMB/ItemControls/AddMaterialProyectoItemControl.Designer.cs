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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMaterialProyectoItemControl));
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
            resources.ApplyResources(this.labelInfoGeneralArticulo, "labelInfoGeneralArticulo");
            this.labelInfoGeneralArticulo.Name = "labelInfoGeneralArticulo";
            // 
            // labelInfoTipoMat
            // 
            resources.ApplyResources(this.labelInfoTipoMat, "labelInfoTipoMat");
            this.labelInfoTipoMat.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelInfoTipoMat.Name = "labelInfoTipoMat";
            // 
            // labelInfoDescArt
            // 
            resources.ApplyResources(this.labelInfoDescArt, "labelInfoDescArt");
            this.labelInfoDescArt.Name = "labelInfoDescArt";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.buttonAgregarMaterial);
            this.panel1.Controls.Add(this.buttonDecreaseQ);
            this.panel1.Controls.Add(this.buttonIncreaseQ);
            this.panel1.Controls.Add(this.labelInfoCantidadInventario);
            this.panel1.Name = "panel1";
            // 
            // buttonAgregarMaterial
            // 
            resources.ApplyResources(this.buttonAgregarMaterial, "buttonAgregarMaterial");
            this.buttonAgregarMaterial.Name = "buttonAgregarMaterial";
            this.buttonAgregarMaterial.UseVisualStyleBackColor = true;
            this.buttonAgregarMaterial.Click += new System.EventHandler(this.buttonAgregarMaterial_Click);
            // 
            // buttonDecreaseQ
            // 
            resources.ApplyResources(this.buttonDecreaseQ, "buttonDecreaseQ");
            this.buttonDecreaseQ.BackColor = System.Drawing.Color.Transparent;
            this.buttonDecreaseQ.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.down_logo;
            this.buttonDecreaseQ.FlatAppearance.BorderSize = 0;
            this.buttonDecreaseQ.Name = "buttonDecreaseQ";
            this.buttonDecreaseQ.UseVisualStyleBackColor = false;
            this.buttonDecreaseQ.Click += new System.EventHandler(this.buttonDecreaseQ_Click);
            // 
            // buttonIncreaseQ
            // 
            resources.ApplyResources(this.buttonIncreaseQ, "buttonIncreaseQ");
            this.buttonIncreaseQ.BackColor = System.Drawing.Color.Transparent;
            this.buttonIncreaseQ.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.up_logo;
            this.buttonIncreaseQ.FlatAppearance.BorderSize = 0;
            this.buttonIncreaseQ.Name = "buttonIncreaseQ";
            this.buttonIncreaseQ.UseVisualStyleBackColor = false;
            this.buttonIncreaseQ.Click += new System.EventHandler(this.buttonIncreaseQ_Click);
            // 
            // labelInfoCantidadInventario
            // 
            resources.ApplyResources(this.labelInfoCantidadInventario, "labelInfoCantidadInventario");
            this.labelInfoCantidadInventario.Name = "labelInfoCantidadInventario";
            // 
            // AddMaterialProyectoItemControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelInfoGeneralArticulo);
            this.Controls.Add(this.labelInfoTipoMat);
            this.Controls.Add(this.labelInfoDescArt);
            this.Name = "AddMaterialProyectoItemControl";
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
