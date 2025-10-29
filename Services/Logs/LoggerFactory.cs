using Services.Logs.Strategy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs
{
    public static class LoggerFactory
    {
        public static ILoggerStrategy Create()
        {
            var raw = (ConfigurationManager.AppSettings["LoggerType"] ?? "file").Trim();
            var tokens = raw
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim().ToLowerInvariant())
                .ToArray();

            if (tokens.Length == 1 && tokens[0] == "both")
                tokens = new[] { "file", "database" };

            if (tokens.Length == 1)
            {
                switch (tokens[0])
                {
                    case "file": return new FileLoggerStrategy();
                    case "database": return new DatabaseLoggerStrategy();
                    default: throw new NotSupportedException($"LoggerType '{raw}' no soportado.");
                }
            }

            var list = tokens.Select(t =>
            {
                switch (t)
                {
                    case "file": return (ILoggerStrategy)new FileLoggerStrategy();
                    case "database": return (ILoggerStrategy)new DatabaseLoggerStrategy();
                    default: throw new NotSupportedException($"LoggerType '{t}' no soportado.");
                }
            }).ToArray();

            return new CompositeLoggerStrategy(list);
        }
    }
}
