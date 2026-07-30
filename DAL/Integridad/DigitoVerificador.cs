using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Integridad
{
    /// <summary>
    /// Cálculo de los dígitos verificadores. El DVH (horizontal) resume una fila;
    /// el DVV (vertical) resume todos los DVH de una tabla. Ambos son SHA-256 sobre
    /// una serialización determinística (orden fijo de columnas, cultura invariante),
    /// para que dos ejecuciones sobre los mismos datos den siempre el mismo valor.
    /// </summary>
    internal static class DigitoVerificador
    {
        /// <summary>DVH de una fila: SHA-256 de "col=valor;" con las columnas ordenadas por nombre.</summary>
        public static string CalcularDvh(IDictionary<string, object> columnas)
        {
            var sb = new StringBuilder();
            foreach (var col in columnas.Keys.OrderBy(k => k, StringComparer.Ordinal))
                sb.Append(col).Append('=').Append(FormatearValor(columnas[col])).Append(';');
            return Sha256Hex(sb.ToString());
        }

        /// <summary>DVV de una tabla: SHA-256 de los DVH ordenados por clave de registro.</summary>
        public static string CalcularDvv(IEnumerable<KeyValuePair<string, string>> dvhPorClave)
        {
            var sb = new StringBuilder();
            foreach (var kv in dvhPorClave.OrderBy(k => k.Key, StringComparer.Ordinal))
                sb.Append(kv.Key).Append('=').Append(kv.Value).Append(';');
            return Sha256Hex(sb.ToString());
        }

        private static string FormatearValor(object v)
        {
            if (v == null || v == DBNull.Value) return "<NULL>"; // marca el null de forma inequívoca
            switch (v)
            {
                case byte[] bytes: return Convert.ToBase64String(bytes);
                case DateTime dt: return dt.ToString("O", CultureInfo.InvariantCulture);
                case DateTimeOffset dto: return dto.ToString("O", CultureInfo.InvariantCulture);
                case bool b: return b ? "1" : "0";
                case IFormattable f: return f.ToString(null, CultureInfo.InvariantCulture);
                default: return v.ToString();
            }
        }

        private static string Sha256Hex(string s)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
