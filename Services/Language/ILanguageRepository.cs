using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Language
{
    public interface ILanguageRepository
    {
        string GetString(string key, string cultureCode);
        IReadOnlyList<Language> GetAvailableLanguages();
    }
}
