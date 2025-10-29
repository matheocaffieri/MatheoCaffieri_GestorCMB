using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logs
{
    public class Log
    {
            /// <summary>
            /// Mensaje descriptivo del evento registrado.
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Nivel de traza del evento (Info, Warning, Error, etc.).
            /// </summary>
            public TraceLevel TraceLevel { get; set; }

            /// <summary>
            /// Fecha y hora en que ocurrió el evento.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Inicializa una nueva instancia de la clase <see cref="System.Diagnostics.Log"/>.
            /// </summary>
            /// <param name="message">Mensaje del evento.</param>
            /// <param name="traceLevel">Nivel de traza del evento (por defecto: Info).</param>
            /// <param name="date">Fecha del evento (si no se especifica, se toma la fecha actual).</param>
            public Log(string message, TraceLevel traceLevel = TraceLevel.Info, DateTime date = default)
            {
                Message = message;
                TraceLevel = traceLevel;
                Date = (date == default) ? DateTime.Now : date;
            }
    }
}
