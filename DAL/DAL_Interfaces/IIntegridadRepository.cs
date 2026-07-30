using System.Collections.Generic;
using DomainModel.Integridad;

namespace DAL.DAL_Interfaces
{
    /// <summary>
    /// Acceso a los dígitos verificadores de las tablas de dominio.
    /// La verificación compara la BD actual contra los dígitos guardados.
    /// </summary>
    public interface IIntegridadRepository
    {
        /// <summary>True si ya existe una línea base de dígitos (al menos un DVH guardado).</summary>
        bool HayLineaBase();

        /// <summary>Recalcula y re-sella los dígitos de todas las tablas protegidas.</summary>
        void RecalcularTodo();

        /// <summary>Verifica todas las tablas protegidas y devuelve las inconsistencias encontradas.</summary>
        List<IntegridadAnomalia> Verificar();
    }
}
