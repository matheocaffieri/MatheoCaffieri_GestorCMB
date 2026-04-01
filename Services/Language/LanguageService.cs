using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Language
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _repo;
        private string _cultureCode;

        public static ILanguageService Current { get; private set; }

        private LanguageService(ILanguageRepository repo, string initialCultureCode)
        {
            _repo = repo;
            SetCulture(string.IsNullOrEmpty(initialCultureCode) ? "es-AR" : initialCultureCode);
        }

        /// <summary>
        /// Inicializa el Singleton la primera vez. En llamadas posteriores solo actualiza la cultura.
        /// </summary>
        public static void Initialize(ILanguageRepository repo, string cultureCode)
        {
            if (Current == null)
                Current = new LanguageService(repo, cultureCode);
            else
                Current.SetCulture(string.IsNullOrEmpty(cultureCode) ? "es-AR" : cultureCode);
        }

        public void SetCulture(string cultureCode)
        {
            _cultureCode = string.IsNullOrEmpty(cultureCode) ? "es-AR" : cultureCode;
            var c = new CultureInfo(_cultureCode);
            Thread.CurrentThread.CurrentCulture = c;      // números/fechas
            Thread.CurrentThread.CurrentUICulture = c;    // recursos UI
        }

        public string T(string key) => _repo.GetString(key, _cultureCode);
        public IReadOnlyList<Language> Languages() => _repo.GetAvailableLanguages();
    }
}
