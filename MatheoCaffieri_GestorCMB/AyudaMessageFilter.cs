using System;
using System.Windows.Forms;
using Services.Ayuda;
using Services.Language;

namespace MatheoCaffieri_GestorCMB
{
    // Escucha F1 en toda la app (se registra una sola vez en Program.Main)
    // y abre el manual en la sección de la pantalla activa. Filtro global
    // en vez de HelpRequested por form: cubre pantallas futuras sin tocarlas.
    public class AyudaMessageFilter : IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_F1 = 0x70;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_KEYDOWN || m.WParam != (IntPtr)VK_F1)
                return false;

            // Bit 30 del lParam: la tecla ya estaba apretada (autorepeat).
            // Sin este filtro, mantener F1 abriría una pestaña por repetición.
            if ((m.LParam.ToInt64() & 0x40000000) != 0)
                return true;

            if (!AyudaService.AbrirManual(ResolverPantallaActiva()))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_manual_no_encontrado")
                        ?? "No se encontró el manual de ayuda. Reinstalá la aplicación o contactá al administrador.",
                    LanguageService.Current?.T("cap_ayuda") ?? "Ayuda",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return true;
        }

        private static string ResolverPantallaActiva()
        {
            var form = Form.ActiveForm;
            if (form is MainForm main)
                return main.NombrePantallaActual() ?? nameof(MainForm);
            return form?.GetType().Name;
        }
    }
}
