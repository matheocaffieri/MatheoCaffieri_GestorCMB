namespace MatheoCaffieri_GestorCMB
{
    partial class MainForm
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.FormPanel = new System.Windows.Forms.Panel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proyectosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verProyectosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarProyectosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verInventarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarMaterialesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultarInfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agregarProveedoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verEmpleadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarEmpleadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajustesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarUsuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurarParametrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.FormPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            resources.ApplyResources(this.FormPanel, "FormPanel");
            this.FormPanel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            // 
            // buttonExit
            // 
            resources.ApplyResources(this.buttonExit, "buttonExit");
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Name = "panel2";
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.proyectosToolStripMenuItem,
            this.inventarioToolStripMenuItem,
            this.personalToolStripMenuItem,
            this.ajustesToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // homeToolStripMenuItem
            // 
            resources.ApplyResources(this.homeToolStripMenuItem, "homeToolStripMenuItem");
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // proyectosToolStripMenuItem
            // 
            resources.ApplyResources(this.proyectosToolStripMenuItem, "proyectosToolStripMenuItem");
            this.proyectosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verProyectosToolStripMenuItem,
            this.agregarProyectosToolStripMenuItem});
            this.proyectosToolStripMenuItem.Name = "proyectosToolStripMenuItem";
            // 
            // verProyectosToolStripMenuItem
            // 
            resources.ApplyResources(this.verProyectosToolStripMenuItem, "verProyectosToolStripMenuItem");
            this.verProyectosToolStripMenuItem.Name = "verProyectosToolStripMenuItem";
            this.verProyectosToolStripMenuItem.Click += new System.EventHandler(this.verProyectosToolStripMenuItem_Click);
            // 
            // agregarProyectosToolStripMenuItem
            // 
            resources.ApplyResources(this.agregarProyectosToolStripMenuItem, "agregarProyectosToolStripMenuItem");
            this.agregarProyectosToolStripMenuItem.Name = "agregarProyectosToolStripMenuItem";
            this.agregarProyectosToolStripMenuItem.Click += new System.EventHandler(this.agregarProyectosToolStripMenuItem_Click);
            // 
            // inventarioToolStripMenuItem
            // 
            resources.ApplyResources(this.inventarioToolStripMenuItem, "inventarioToolStripMenuItem");
            this.inventarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verInventarioToolStripMenuItem,
            this.agregarMaterialesToolStripMenuItem,
            this.consultarInfToolStripMenuItem,
            this.agregarProveedoresToolStripMenuItem});
            this.inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            // 
            // verInventarioToolStripMenuItem
            // 
            resources.ApplyResources(this.verInventarioToolStripMenuItem, "verInventarioToolStripMenuItem");
            this.verInventarioToolStripMenuItem.Name = "verInventarioToolStripMenuItem";
            this.verInventarioToolStripMenuItem.Click += new System.EventHandler(this.verInventarioToolStripMenuItem_Click);
            // 
            // agregarMaterialesToolStripMenuItem
            // 
            resources.ApplyResources(this.agregarMaterialesToolStripMenuItem, "agregarMaterialesToolStripMenuItem");
            this.agregarMaterialesToolStripMenuItem.Name = "agregarMaterialesToolStripMenuItem";
            this.agregarMaterialesToolStripMenuItem.Click += new System.EventHandler(this.agregarMaterialesToolStripMenuItem_Click);
            // 
            // consultarInfToolStripMenuItem
            // 
            resources.ApplyResources(this.consultarInfToolStripMenuItem, "consultarInfToolStripMenuItem");
            this.consultarInfToolStripMenuItem.Name = "consultarInfToolStripMenuItem";
            this.consultarInfToolStripMenuItem.Click += new System.EventHandler(this.consultarInfToolStripMenuItem_Click);
            // 
            // agregarProveedoresToolStripMenuItem
            // 
            resources.ApplyResources(this.agregarProveedoresToolStripMenuItem, "agregarProveedoresToolStripMenuItem");
            this.agregarProveedoresToolStripMenuItem.Name = "agregarProveedoresToolStripMenuItem";
            this.agregarProveedoresToolStripMenuItem.Click += new System.EventHandler(this.agregarProveedoresToolStripMenuItem_Click);
            // 
            // personalToolStripMenuItem
            // 
            resources.ApplyResources(this.personalToolStripMenuItem, "personalToolStripMenuItem");
            this.personalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verEmpleadosToolStripMenuItem,
            this.cargarEmpleadosToolStripMenuItem});
            this.personalToolStripMenuItem.Name = "personalToolStripMenuItem";
            // 
            // verEmpleadosToolStripMenuItem
            // 
            resources.ApplyResources(this.verEmpleadosToolStripMenuItem, "verEmpleadosToolStripMenuItem");
            this.verEmpleadosToolStripMenuItem.Name = "verEmpleadosToolStripMenuItem";
            this.verEmpleadosToolStripMenuItem.Click += new System.EventHandler(this.verEmpleadosToolStripMenuItem_Click);
            // 
            // cargarEmpleadosToolStripMenuItem
            // 
            resources.ApplyResources(this.cargarEmpleadosToolStripMenuItem, "cargarEmpleadosToolStripMenuItem");
            this.cargarEmpleadosToolStripMenuItem.Name = "cargarEmpleadosToolStripMenuItem";
            this.cargarEmpleadosToolStripMenuItem.Click += new System.EventHandler(this.cargarEmpleadosToolStripMenuItem_Click);
            // 
            // ajustesToolStripMenuItem
            // 
            resources.ApplyResources(this.ajustesToolStripMenuItem, "ajustesToolStripMenuItem");
            this.ajustesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verLogsToolStripMenuItem,
            this.gestionarUsuariosToolStripMenuItem,
            this.configurarParametrosToolStripMenuItem});
            this.ajustesToolStripMenuItem.Name = "ajustesToolStripMenuItem";
            // 
            // verLogsToolStripMenuItem
            // 
            resources.ApplyResources(this.verLogsToolStripMenuItem, "verLogsToolStripMenuItem");
            this.verLogsToolStripMenuItem.Name = "verLogsToolStripMenuItem";
            this.verLogsToolStripMenuItem.Click += new System.EventHandler(this.verLogsToolStripMenuItem_Click);
            //
            // gestionarUsuariosToolStripMenuItem
            // 
            resources.ApplyResources(this.gestionarUsuariosToolStripMenuItem, "gestionarUsuariosToolStripMenuItem");
            this.gestionarUsuariosToolStripMenuItem.Name = "gestionarUsuariosToolStripMenuItem";
            this.gestionarUsuariosToolStripMenuItem.Click += new System.EventHandler(this.gestionarUsuariosToolStripMenuItem_Click);
            //
            // configurarParametrosToolStripMenuItem
            //
            this.configurarParametrosToolStripMenuItem.Name = "configurarParametrosToolStripMenuItem";
            this.configurarParametrosToolStripMenuItem.Text = "Configurar Parámetros";
            this.configurarParametrosToolStripMenuItem.Click += new System.EventHandler(this.configurarParametrosToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            resources.ApplyResources(this.MainPanel, "MainPanel");
            this.MainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.MainPanel.Name = "MainPanel";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.FormPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proyectosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verProyectosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarProyectosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verInventarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem agregarMaterialesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultarInfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verEmpleadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarEmpleadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajustesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verLogsToolStripMenuItem;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem agregarProveedoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarUsuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurarParametrosToolStripMenuItem;
    }
}

