using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using Services.Language;

namespace MatheoCaffieri_GestorCMB.Language
{
    public class ResxLanguageRepository : ILanguageRepository
    {
        private readonly ResourceManager _rm;
        private readonly List<Services.Language.Language> _langs;

        public ResxLanguageRepository()
        {
            _rm = MatheoCaffieri_GestorCMB.Properties.Resources.ResourceManager;

            _langs = new List<Services.Language.Language>
            {
                new Services.Language.Language { Code = "es-AR", Name = "Español (Argentina)" },
                new Services.Language.Language { Code = "en-US", Name = "English (United States)" }
            };
        }

        public string GetString(string key, string cultureCode)
        {
            var code = string.IsNullOrEmpty(cultureCode) ? "es-AR" : cultureCode;
            var culture = new CultureInfo(code);

            var prev = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = culture;
                var s = _rm.GetString(key, culture);
                return string.IsNullOrEmpty(s) ? "[" + key + "]" : s;
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = prev;
            }
        }

        public IReadOnlyList<Services.Language.Language> GetAvailableLanguages() => _langs;
    }
}
