using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;   
using System.IO;
using System.Linq;
using System.Text;

namespace Services.Logs
{


   
        internal static class LoggerRepository
        {
            public static void WriteLogToFile(Log log, Exception ex = null)
            {
                var dir = ConfigurationManager.AppSettings["LogDirectory"] ?? "logs";
                Directory.CreateDirectory(dir);

                var fileName = (log.TraceLevel == TraceLevel.Error)
                    ? "error.log" : "info.log";

                var path = Path.Combine(dir, $"{DateTime.Now:yyyy-MM-dd}_{fileName}");
                var line = FormatLine(log, ex);

                using (var sw = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    sw.WriteLine(line);

                TryRetention(dir);
            }

            // si luego usás DB
            public static void WriteLogToDatabase(Log log, Exception ex = null)
            {
            var cs = ConfigurationManager.ConnectionStrings["LogsConnection"].ConnectionString;

            using (var cn = new SqlConnection(cs))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
                    INSERT INTO Logs (Fecha, Nivel, Mensaje, Excepcion)
                    VALUES (@fecha, @nivel, @mensaje, @excepcion)";

                cmd.Parameters.AddWithValue("@fecha", log.Date);
                cmd.Parameters.AddWithValue("@nivel", log.TraceLevel.ToString());
                cmd.Parameters.AddWithValue("@mensaje", log.Message ?? "");
                cmd.Parameters.AddWithValue("@excepcion", ex != null ? ex.ToString() : (object)DBNull.Value);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

            private static string FormatLine(Log log, Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append('[').Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("] ");
                sb.Append('[').Append(log.TraceLevel).Append("] ");
                sb.Append(" :: ").Append(log.Message ?? "");
                if (log.Date != default) sb.Append(" | date=").Append(log.Date.ToString("s"));
                if (ex != null) sb.Append(" | ex=").Append(ex);
                return sb.ToString();
            }

            private static void TryRetention(string dir)
            {
                var raw = ConfigurationManager.AppSettings["LogRetentionDays"];
                if (!int.TryParse(raw, out var days)) days = 14;
                var cutoff = DateTime.Now.AddDays(-days);

                try
                {
                    foreach (var f in Directory.GetFiles(dir, "????-??-??_*.log"))
                        if (new FileInfo(f).LastWriteTime < cutoff) File.Delete(f);
                }
                catch { /* no romper por limpieza */ }
            }
        }
}



