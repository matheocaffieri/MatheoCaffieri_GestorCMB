using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs
{
    public static class LogHelper
    {
        // Acción simple (Info/OK/FAIL automático)
        public static void Run(string scope, Action work)
        {
            if (string.IsNullOrWhiteSpace(scope)) scope = "UnknownScope";

            try
            {
                LoggerLogic.Info($"{scope} :: START");
                work();
                LoggerLogic.Info($"{scope} :: OK");
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"{scope} :: FAIL", ex);
                throw; // IMPORTANTÍSIMO
            }
        }

        // Variante con retorno
        public static T Run<T>(string scope, Func<T> work)
        {
            if (string.IsNullOrWhiteSpace(scope)) scope = "UnknownScope";

            try
            {
                LoggerLogic.Info($"{scope} :: START");
                var result = work();
                LoggerLogic.Info($"{scope} :: OK");
                return result;
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"{scope} :: FAIL", ex);
                throw;
            }
        }

        public static void Info(string scope, string msg = null)
            => LoggerLogic.Info(Format(scope, msg));

        public static void Warn(string scope, string msg = null)
            => LoggerLogic.Warn(Format(scope, msg));

        public static void Error(string scope, Exception ex, string msg = null)
            => LoggerLogic.Error(Format(scope, msg), ex);

        private static string Format(string scope, string msg)
        {
            if (string.IsNullOrWhiteSpace(scope)) scope = "UnknownScope";
            return string.IsNullOrWhiteSpace(msg) ? scope : $"{scope} :: {msg}";
        }
    }
}
