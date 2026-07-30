using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Services.Logs;

namespace Services.Ayuda
{
    // Ayuda contextual: resuelve la sección del manual según la pantalla activa
    // y abre el navegador predeterminado. No referencia WinForms a propósito:
    // la UI solo le pasa el nombre del tipo de la pantalla como string.
    public static class AyudaService
    {
        private const string ArchivoManual = @"Ayuda\Manual-Ayuda-GestorCMB.html";
        private const string AnclaInicio = "inicio";

        private static readonly Dictionary<string, string> Secciones =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["LoginForm"] = "m-login",
                ["ForgotPasswordForm"] = "m-login",
                ["MainForm"] = "m-principal",
                ["HomeControl"] = "m-principal",
                ["VerProyectosControl"] = "m-proyectos",
                ["DetalleProyectoControl"] = "m-proyectos",
                ["AddProyectosForm"] = "m-proyectos",
                ["EditProyectoForm"] = "m-proyectos",
                ["AgregarEmpleadoProyectoForm"] = "m-proyectos",
                ["AgregarMaterialProyectoForm"] = "m-proyectos",
                ["AnalisisProyectoForm"] = "m-proyectos",
                ["VerInventarioControl"] = "m-inventario",
                ["AddMaterialesForm"] = "m-inventario",
                ["ProveedorControl"] = "m-proveedores",
                ["EditProveedorForm"] = "m-proveedores",
                ["InformesDeCompraControl"] = "m-informes",
                ["HistorialInformesControl"] = "m-informes",
                ["VerEmpleadosControl"] = "m-empleados",
                ["AddEmpleadosForm"] = "m-empleados",
                ["EditEmpleadoForm"] = "m-empleados",
                ["ClientesControl"] = "m-clientes",
                ["EditClienteForm"] = "m-clientes",
                ["GestionUsuariosControl"] = "m-usuarios",
                ["EditUserForm"] = "m-usuarios",
                ["AccesosForm"] = "m-usuarios",
                ["ConfigurarParametrosControl"] = "m-parametros",
                ["VerLogsForm"] = "m-logs",
                ["BackupForm"] = "m-backup",
            };

        public static bool AbrirManual() => AbrirEnAncla(AnclaInicio);

        public static bool AbrirManual(string nombrePantalla)
        {
            string ancla;
            if (nombrePantalla == null || !Secciones.TryGetValue(nombrePantalla, out ancla))
                ancla = AnclaInicio;
            return AbrirEnAncla(ancla);
        }

        private static bool AbrirEnAncla(string ancla)
        {
            try
            {
                var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArchivoManual);
                if (!File.Exists(ruta))
                {
                    LogHelper.Warn("[Ayuda]", $"No se encontró el manual en {ruta}");
                    return false;
                }

                var url = new Uri(ruta).AbsoluteUri + "#" + ancla;

                // ShellExecute directo sobre un file:// suele descartar el #ancla,
                // así que invocamos el exe del navegador predeterminado con la URL
                // como argumento. Si no lo podemos resolver, abrimos sin ancla.
                var navegador = ResolverExeNavegador();
                if (navegador != null)
                    Process.Start(navegador, "\"" + url + "\"");
                else
                    Process.Start(ruta);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("[Ayuda]", ex, "No se pudo abrir el manual");
                return false;
            }
        }

        private static string ResolverExeNavegador()
        {
            try
            {
                string progId;
                using (var k = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
                {
                    progId = k?.GetValue("ProgId") as string;
                }
                if (string.IsNullOrEmpty(progId)) return null;

                string comando;
                using (var k = Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command"))
                {
                    comando = k?.GetValue(null) as string;
                }
                if (string.IsNullOrEmpty(comando)) return null;

                // El comando viene como: "C:\...\msedge.exe" --flags "%1"
                comando = comando.TrimStart();
                if (comando.StartsWith("\"", StringComparison.Ordinal))
                {
                    var fin = comando.IndexOf('"', 1);
                    return fin > 1 ? comando.Substring(1, fin - 1) : null;
                }
                var espacio = comando.IndexOf(' ');
                return espacio > 0 ? comando.Substring(0, espacio) : comando;
            }
            catch
            {
                return null;
            }
        }
    }
}
