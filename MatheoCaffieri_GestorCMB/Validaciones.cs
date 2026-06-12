using System.Text.RegularExpressions;

namespace MatheoCaffieri_GestorCMB
{
    /// <summary>Validaciones de formato compartidas entre formularios.</summary>
    public static class Validaciones
    {
        // Formato básico: algo@algo.algo, sin espacios.
        private static readonly Regex MailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static bool EsMailValido(string mail) =>
            !string.IsNullOrWhiteSpace(mail) && MailRegex.IsMatch(mail.Trim());
    }
}
