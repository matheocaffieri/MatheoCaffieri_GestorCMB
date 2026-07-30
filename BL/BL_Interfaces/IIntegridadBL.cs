using System.Collections.Generic;
using DomainModel.Integridad;

namespace BL.BL_Interfaces
{
    /// <summary>Fachada de negocio para el módulo de dígitos verificadores.</summary>
    public interface IIntegridadBL
    {
        bool HayLineaBase();
        void RecalcularLineaBase();
        List<IntegridadAnomalia> Verificar();

        /// <summary>Si no hay línea base la crea (primer arranque) y devuelve vacío; si no, verifica.</summary>
        List<IntegridadAnomalia> VerificarOInicializar();
    }
}
