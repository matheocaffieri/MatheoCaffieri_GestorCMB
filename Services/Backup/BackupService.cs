using Services.Logs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Services.Backup
{
    /// <summary>
    /// Genera copias de seguridad nativas (.bak) de las bases SQL Server via BACKUP DATABASE.
    /// Acceso a datos por ADO.NET (capa Services), fuera de la UI.
    /// </summary>
    public class BackupService
    {
        // Connection strings del App.config que representan bases respaldables.
        // El orden define cómo se listan en la UI.
        private static readonly string[] _connNames =
        {
            "GestorCMBConnection",
            "MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios",
            "LogsConnection"
        };

        // Carpeta usada si el App.config no define BackupDirectory.
        private const string DestinoFallback = @"C:\Backups\GestorCMB";

        /// <summary>
        /// Carpeta destino por defecto de los .bak, según BackupDirectory del App.config.
        /// Expande variables de entorno; si no está configurada, devuelve la carpeta por defecto.
        /// </summary>
        public string CarpetaDestinoPorDefecto()
        {
            var cfg = ConfigurationManager.AppSettings["BackupDirectory"];
            return string.IsNullOrWhiteSpace(cfg)
                ? DestinoFallback
                : Environment.ExpandEnvironmentVariables(cfg);
        }

        /// <summary>
        /// Bases disponibles para respaldar, leídas de las connection strings del App.config.
        /// Deduplica por catálogo (varias conn strings pueden apuntar a la misma base).
        /// </summary>
        public List<BackupDb> DatabasesDisponibles()
        {
            var list = new List<BackupDb>();
            var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var name in _connNames)
            {
                var cs = ConfigurationManager.ConnectionStrings[name];
                if (cs == null) continue;

                try
                {
                    var b = new SqlConnectionStringBuilder(cs.ConnectionString);
                    var catalogo = b.InitialCatalog;
                    if (string.IsNullOrWhiteSpace(catalogo)) continue;
                    if (!vistos.Add(catalogo)) continue;

                    list.Add(new BackupDb { Catalogo = catalogo, ConnectionStringName = name });
                }
                catch
                {
                    // Connection string que no es SqlClient o está mal formada: la ignoramos.
                }
            }

            return list;
        }

        /// <summary>
        /// Respalda cada base seleccionada a un archivo .bak dentro de <paramref name="carpetaDestino"/>.
        /// Nunca lanza por una base individual: el fallo queda en su BackupResult.Error.
        /// </summary>
        public List<BackupResult> Generar(IEnumerable<string> catalogos, string carpetaDestino)
        {
            if (catalogos == null) throw new ArgumentNullException(nameof(catalogos));
            if (string.IsNullOrWhiteSpace(carpetaDestino))
                throw new ArgumentException("La carpeta destino no puede estar vacía.", nameof(carpetaDestino));

            Directory.CreateDirectory(carpetaDestino);

            var disponibles = DatabasesDisponibles();
            var results = new List<BackupResult>();

            foreach (var catalogo in catalogos.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var db = disponibles.FirstOrDefault(d => string.Equals(d.Catalogo, catalogo, StringComparison.OrdinalIgnoreCase));
                if (db == null)
                {
                    // Solo respaldamos catálogos de la whitelist (evita inyección en el identificador BACKUP).
                    results.Add(new BackupResult { Base = catalogo, Ok = false, Error = "Base no reconocida." });
                    continue;
                }

                var archivo = Path.Combine(carpetaDestino, $"{catalogo}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
                try
                {
                    LoggerLogic.Info($"[Backup] Iniciando copia de '{catalogo}'.");
                    EjecutarBackup(db.ConnectionStringName, catalogo, archivo);
                    results.Add(new BackupResult { Base = catalogo, Ok = true, RutaArchivo = archivo });
                    LoggerLogic.Info($"[Backup] Copia OK de '{catalogo}' -> {archivo}");
                }
                catch (Exception ex)
                {
                    results.Add(new BackupResult { Base = catalogo, Ok = false, RutaArchivo = archivo, Error = ex.Message });
                    LoggerLogic.Error($"[Backup] Error copiando '{catalogo}'.", ex);
                }
            }

            return results;
        }

        private static void EjecutarBackup(string connStringName, string catalogo, string archivo)
        {
            var original = ConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            // Conectamos a master en el mismo servidor para ejecutar el BACKUP.
            var builder = new SqlConnectionStringBuilder(original) { InitialCatalog = "master" };

            // El nombre de la base NO puede ir como parámetro en BACKUP DATABASE: se escapa como identificador.
            var dbIdent = "[" + catalogo.Replace("]", "]]") + "]";

            using (var cn = new SqlConnection(builder.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = $"BACKUP DATABASE {dbIdent} TO DISK = @path WITH FORMAT, INIT, NAME = @name";
                cmd.Parameters.AddWithValue("@path", archivo);
                cmd.Parameters.AddWithValue("@name", $"{catalogo}-Full Backup");
                cmd.CommandTimeout = 0; // los backups pueden tardar; sin límite de tiempo

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
