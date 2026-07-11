using System;

namespace Services.Logs
{
    /// <summary>
    /// Modelo de lectura de un registro de log de la BD.
    /// Se usa para presentar logs (p. ej. en la UI) sin exponer detalles de acceso a datos.
    /// </summary>
    public class LogEntry
    {
        public DateTime Fecha { get; set; }
        public string Nivel { get; set; }
        public string Mensaje { get; set; }
        public string Excepcion { get; set; }
    }
}
