namespace BL.BL_Interfaces
{
    /// <summary>Fachada de negocio para tareas de arranque del sistema.</summary>
    public interface ISistemaBL
    {
        /// <summary>Calienta el contexto de datos para evitar la latencia de la primera consulta.</summary>
        void WarmUp();
    }
}
