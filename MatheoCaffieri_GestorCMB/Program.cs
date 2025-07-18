using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());


            try
            {
                var dbManager = new DatabaseManager();
                dbManager.InitializeDatabase(); // Inicializar la base de datos a través de BLL
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar la base de datos: {ex.Message}");
            }
        }

        
    }
}
