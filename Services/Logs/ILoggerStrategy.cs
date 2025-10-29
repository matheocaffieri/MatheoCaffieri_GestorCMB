using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs
{
    public interface ILoggerStrategy
    {
        /// <summary>
        /// Contrato de la estrategia de salida de logs (archivo, DB, etc.).
        /// </summary>
        
            /// <param name="log">Evento de log (mensaje, tipo, usuario, etc.).</param>
            /// <param name="ex">Excepción opcional asociada.</param>
            void WriteLog(Log log, Exception ex = null);
        
    }
}
