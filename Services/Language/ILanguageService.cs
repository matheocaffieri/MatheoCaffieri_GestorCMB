using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Language
{
    public interface ILanguageService
    {
        string T(string key);
        IReadOnlyList<Language> Languages();
        void SetCulture(string cultureCode);
    }
}
