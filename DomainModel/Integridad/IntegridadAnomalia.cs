namespace DomainModel.Integridad
{
    /// <summary>Tipo de inconsistencia detectada por el chequeo de dígitos verificadores.</summary>
    public enum TipoAnomalia
    {
        /// <summary>El DVH del registro no coincide: la fila fue modificada por fuera del sistema.</summary>
        Modificado,
        /// <summary>Hay una fila sin DVH registrado: se insertó por fuera del sistema.</summary>
        Agregado,
        /// <summary>Hay un DVH registrado sin fila: se eliminó por fuera del sistema.</summary>
        Eliminado,
        /// <summary>El DVV de la tabla no coincide (chequeo vertical a nivel tabla).</summary>
        DigitoTablaInvalido
    }

    /// <summary>
    /// Una inconsistencia concreta detectada al verificar la integridad de una tabla.
    /// </summary>
    public class IntegridadAnomalia
    {
        public string Tabla { get; set; }
        public string ClaveRegistro { get; set; }
        public TipoAnomalia Tipo { get; set; }

        public IntegridadAnomalia() { }

        public IntegridadAnomalia(string tabla, string claveRegistro, TipoAnomalia tipo)
        {
            Tabla = tabla;
            ClaveRegistro = claveRegistro;
            Tipo = tipo;
        }
    }
}
