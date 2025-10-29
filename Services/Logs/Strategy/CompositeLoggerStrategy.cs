using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs.Strategy
{
    public sealed class CompositeLoggerStrategy : ILoggerStrategy
    {
        private readonly IList<ILoggerStrategy> _targets;
        public CompositeLoggerStrategy(params ILoggerStrategy[] targets)
        {
            _targets = targets ?? Array.Empty<ILoggerStrategy>();
        }

        public void WriteLog(Log log, Exception ex)
        {
            foreach (var t in _targets)
            {
                try { t.WriteLog(log, ex); }
                catch { /* no romper a los demás */ }
            }
        }
    }
}
