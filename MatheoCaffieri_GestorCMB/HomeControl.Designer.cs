namespace MatheoCaffieri_GestorCMB
{
    partial class HomeControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeControl));
            this.labelClientes = new System.Windows.Forms.Label();
            this.labelPersonal = new System.Windows.Forms.Label();
            this.labelInventario = new System.Windows.Forms.Label();
            this.labelProyectos = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.butMainProyectos = new System.Windows.Forms.Button();
            this.linkLabelUser = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelClientes
            // 
            resources.ApplyResources(this.labelClientes, "labelClientes");
            this.labelClientes.Name = "labelClientes";
            // 
            // labelPersonal
            // 
            resources.ApplyResources(this.labelPersonal, "labelPersonal");
            this.labelPersonal.Name = "labelPersonal";
            // 
            // labelInventario
            // 
            resources.ApplyResources(this.labelInventario, "labelInventario");
            this.labelInventario.Name = "labelInventario";
            // 
            // labelProyectos
            // 
            resources.ApplyResources(this.labelProyectos, "labelProyectos");
            this.labelProyectos.Name = "labelProyectos";
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.BackColor = System.Drawing.Color.LightCoral;
            this.button4.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.cliente;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button3.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.personal;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackColor = System.Drawing.Color.NavajoWhite;
            this.button2.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.inventario;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // butMainProyectos
            //
            resources.ApplyResources(this.butMainProyectos, "butMainProyectos");
            this.butMainProyectos.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.butMainProyectos.BackgroundImage = global::MatheoCaffieri_GestorCMB.Properties.Resources.proyecto;
            this.butMainProyectos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.butMainProyectos.Name = "butMainProyectos";
            this.butMainProyectos.UseVisualStyleBackColor = false;
            this.butMainProyectos.Click += new System.EventHandler(this.butMainProyectos_Click);
            // 
            // linkLabelUser
            // 
            resources.ApplyResources(this.linkLabelUser, "linkLabelUser");
            this.linkLabelUser.Name = "linkLabelUser";
            this.linkLabelUser.TabStop = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // HomeControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelUser);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelClientes);
            this.Controls.Add(this.labelPersonal);
            this.Controls.Add(this.labelInventario);
            this.Controls.Add(this.labelProyectos);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.butMainProyectos);
            this.Name = "HomeControl";
            this.Load += new System.EventHandler(this.HomeControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelClientes;
        private System.Windows.Forms.Label labelPersonal;
        private System.Windows.Forms.Label labelInventario;
        private System.Windows.Forms.Label labelProyectos;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button butMainProyectos;
        private System.Windows.Forms.LinkLabel linkLabelUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
    }
}
