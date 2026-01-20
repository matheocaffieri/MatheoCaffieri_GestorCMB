using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RoleService
{
    public class SessionContext
    {
        public static Guid IdUsuario { get; private set; }
        public static HashSet<string> Accesos { get; private set; } = new HashSet<string>();

        // Normalización única
        private static string Norm(string s)
            => (s ?? "").Trim().ToUpperInvariant();

        public static void SetUsuario(Guid idUsuario, IEnumerable<string> accesos)
        {
            IdUsuario = idUsuario;

            Accesos = new HashSet<string>(
                (accesos ?? Enumerable.Empty<string>()).Select(Norm)
            );
        }

        public static bool Has(string acceso)
            => Accesos.Contains(Norm(acceso));
    }
}
