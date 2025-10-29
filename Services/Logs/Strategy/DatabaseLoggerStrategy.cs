using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs.Strategy
{
    /// <summary>
    /// Estrategia de logging hacia base de datos.
    /// </summary>
    public class DatabaseLoggerStrategy : ILoggerStrategy
    {
        public void WriteLog(Log log, Exception ex = null)
            => LoggerRepository.WriteLogToDatabase(log, ex);
    }
}

