using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs.Strategy
{
    public class FileLoggerStrategy : ILoggerStrategy
    {
        public void WriteLog(Log log, Exception ex = null)
            => LoggerRepository.WriteLogToFile(log, ex);
    }

    
}
