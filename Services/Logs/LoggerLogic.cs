using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs
{
    /// <summary>
    /// Fachada estática y simple para escribir logs sin exponer detalles de la estrategia.
    /// Compatible con C# 7.3 (sin features nuevos).
    /// </summary>
    public static class LoggerLogic
    {
        private static readonly Lazy<ILoggerStrategy> _strategy =
            new Lazy<ILoggerStrategy>(() => LoggerFactory.Create());

        // Info
        public static void Info(string message)
        {
            Write(message, TraceLevel.Info, null);
        }

        // Warning
        public static void Warn(string message)
        {
            Write(message, TraceLevel.Warning, null);
        }

        // Debug (Verbose en TraceLevel)
        public static void Debug(string message)
        {
            Write(message, TraceLevel.Verbose, null);
        }

        // Error (con y sin excepción)
        public static void Error(string message)
        {
            Write(message, TraceLevel.Error, null);
        }

        public static void Error(string message, Exception ex)
        {
            Write(message, TraceLevel.Error, ex);
        }

        // Reemplazar TraceLevel.Critical por TraceLevel.Error (ya que TraceLevel no tiene 'Critical' en .NET Framework 4.7.2)
        public static void Critical(string message, Exception ex)
        {
            Write(message, TraceLevel.Error, ex);
        }

        // Método común
        private static void Write(string message, TraceLevel level, Exception ex)
        {
            var log = new Log(message, level);
            _strategy.Value.WriteLog(log, ex);
        }
    }
}
