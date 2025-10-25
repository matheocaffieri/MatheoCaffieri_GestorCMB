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
            this.configurarParámetrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionarUsuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.FormPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.FormPanel.Controls.Add(this.buttonExit);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(1082, 32);
            this.FormPanel.TabIndex = 0;
            this.FormPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseDown);
            this.FormPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormPanel_MouseMove);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(1044, 3);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(30, 23);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "X";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1082, 33);
            this.panel2.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.proyectosToolStripMenuItem,
            this.inventarioToolStripMenuItem,
            this.personalToolStripMenuItem,
            this.ajustesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(444, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // proyectosToolStripMenuItem
            // 
            this.proyectosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verProyectosToolStripMenuItem,
            this.agregarProyectosToolStripMenuItem});
            this.proyectosToolStripMenuItem.Name = "proyectosToolStripMenuItem";
            this.proyectosToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.proyectosToolStripMenuItem.Text = "Proyectos";
            // 
            // verProyectosToolStripMenuItem
            // 
            this.verProyectosToolStripMenuItem.Name = "verProyectosToolStripMenuItem";
            this.verProyectosToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.verProyectosToolStripMenuItem.Text = "Ver proyectos";
            this.verProyectosToolStripMenuItem.Click += new System.EventHandler(this.verProyectosToolStripMenuItem_Click);
            // 
            // agregarProyectosToolStripMenuItem
            // 
            this.agregarProyectosToolStripMenuItem.Name = "agregarProyectosToolStripMenuItem";
            this.agregarProyectosToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.agregarProyectosToolStripMenuItem.Text = "Agregar proyectos";
            this.agregarProyectosToolStripMenuItem.Click += new System.EventHandler(this.agregarProyectosToolStripMenuItem_Click);
            // 
            // inventarioToolStripMenuItem
            // 
            this.inventarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verInventarioToolStripMenuItem,
            this.agregarMaterialesToolStripMenuItem,
            this.consultarInfToolStripMenuItem,
            this.agregarProveedoresToolStripMenuItem});
            this.inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            this.inventarioToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.inventarioToolStripMenuItem.Text = "Inventario";
            // 
            // verInventarioToolStripMenuItem
            // 
            this.verInventarioToolStripMenuItem.Name = "verInventarioToolStripMenuItem";
            this.verInventarioToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.verInventarioToolStripMenuItem.Text = "Ver inventario";
            this.verInventarioToolStripMenuItem.Click += new System.EventHandler(this.verInventarioToolStripMenuItem_Click);
            // 
            // agregarMaterialesToolStripMenuItem
            // 
            this.agregarMaterialesToolStripMenuItem.Name = "agregarMaterialesToolStripMenuItem";
            this.agregarMaterialesToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.agregarMaterialesToolStripMenuItem.Text = "Agregar materiales";
            this.agregarMaterialesToolStripMenuItem.Click += new System.EventHandler(this.agregarMaterialesToolStripMenuItem_Click);
            // 
            // consultarInfToolStripMenuItem
            // 
            this.consultarInfToolStripMenuItem.Name = "consultarInfToolStripMenuItem";
            this.consultarInfToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.consultarInfToolStripMenuItem.Text = "Consultar informes de compra";
            this.consultarInfToolStripMenuItem.Click += new System.EventHandler(this.consultarInfToolStripMenuItem_Click);
            // 
            // agregarProveedoresToolStripMenuItem
            // 
            this.agregarProveedoresToolStripMenuItem.Name = "agregarProveedoresToolStripMenuItem";
            this.agregarProveedoresToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.agregarProveedoresToolStripMenuItem.Text = "Agregar proveedores";
            this.agregarProveedoresToolStripMenuItem.Click += new System.EventHandler(this.agregarProveedoresToolStripMenuItem_Click);
            // 
            // personalToolStripMenuItem
            // 
            this.personalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verEmpleadosToolStripMenuItem,
            this.cargarEmpleadosToolStripMenuItem});
            this.personalToolStripMenuItem.Name = "personalToolStripMenuItem";
            this.personalToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.personalToolStripMenuItem.Text = "Personal";
            // 
            // verEmpleadosToolStripMenuItem
            // 
            this.verEmpleadosToolStripMenuItem.Name = "verEmpleadosToolStripMenuItem";
            this.verEmpleadosToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.verEmpleadosToolStripMenuItem.Text = "Ver empleados";
            this.verEmpleadosToolStripMenuItem.Click += new System.EventHandler(this.verEmpleadosToolStripMenuItem_Click);
            // 
            // cargarEmpleadosToolStripMenuItem
            // 
            this.cargarEmpleadosToolStripMenuItem.Name = "cargarEmpleadosToolStripMenuItem";
            this.cargarEmpleadosToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.cargarEmpleadosToolStripMenuItem.Text = "Cargar empleados";
            this.cargarEmpleadosToolStripMenuItem.Click += new System.EventHandler(this.cargarEmpleadosToolStripMenuItem_Click);
            // 
            // ajustesToolStripMenuItem
            // 
            this.ajustesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verLogsToolStripMenuItem,
            this.configurarParámetrosToolStripMenuItem,
            this.gestionarUsuariosToolStripMenuItem});
            this.ajustesToolStripMenuItem.Name = "ajustesToolStripMenuItem";
            this.ajustesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.ajustesToolStripMenuItem.Text = "Ajustes";
            // 
            // verLogsToolStripMenuItem
            // 
            this.verLogsToolStripMenuItem.Name = "verLogsToolStripMenuItem";
            this.verLogsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.verLogsToolStripMenuItem.Text = "Ver logs";
            this.verLogsToolStripMenuItem.Click += new System.EventHandler(this.verLogsToolStripMenuItem_Click);
            // 
            // configurarParámetrosToolStripMenuItem
            // 
            this.configurarParámetrosToolStripMenuItem.Name = "configurarParámetrosToolStripMenuItem";
            this.configurarParámetrosToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.configurarParámetrosToolStripMenuItem.Text = "Configurar parámetros";
            // 
            // gestionarUsuariosToolStripMenuItem
            // 
            this.gestionarUsuariosToolStripMenuItem.Name = "gestionarUsuariosToolStripMenuItem";
            this.gestionarUsuariosToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.gestionarUsuariosToolStripMenuItem.Text = "Gestionar usuarios";
            this.gestionarUsuariosToolStripMenuItem.Click += new System.EventHandler(this.gestionarUsuariosToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.MainPanel.Location = new System.Drawing.Point(7, 71);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(1067, 441);
            this.MainPanel.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 518);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
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
        private System.Windows.Forms.ToolStripMenuItem configurarParámetrosToolStripMenuItem;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem agregarProveedoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionarUsuariosToolStripMenuItem;
    }
}

