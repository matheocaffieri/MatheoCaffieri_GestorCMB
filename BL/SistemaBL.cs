using BL.BL_Interfaces;
using DAL;

namespace BL
{
    /// <summary>
    /// Tareas de arranque del sistema. Expone el warm-up de la base a la capa de
    /// presentación sin que ésta tenga que referenciar DAL directamente.
    /// </summary>
    public class SistemaBL : ISistemaBL
    {
        public void WarmUp()
        {
            new DatabaseService().WarmUp();
        }
    }
}
