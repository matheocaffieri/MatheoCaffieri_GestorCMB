using DomainModel.Login;
using Services.Language;
using Services.RoleService;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    /// <summary>
    /// Punto único de chequeo de permisos en la UI: toda navegación o acción
    /// privilegiada debe pasar por Require/Tiene antes de ejecutarse.
    /// </summary>
    internal static class PermisosUI
    {
        public static bool Tiene(TipoPermiso permiso) =>
            SessionContext.Has(permiso.ToString());

        /// <summary>
        /// Devuelve true si el usuario tiene el permiso; si no, muestra el
        /// mensaje de acceso denegado y devuelve false (el caller debe abortar).
        /// </summary>
        public static bool Require(TipoPermiso permiso)
        {
            if (Tiene(permiso))
                return true;

            MessageBox.Show(
                LanguageService.Current?.T("err_sin_permisos") ?? "No tenés permisos para realizar esta acción.",
                LanguageService.Current?.T("cap_acceso_denegado") ?? "Acceso denegado",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }
}
