using System;
using System.Collections.Generic;

using DomainModel;

namespace DAL.DAL_Interfaces
{
    // Repositorio del snapshot histórico de faltantes de un informe de compra.
    // Al confirmar la compra los Material_faltante originales se borran; este snapshot
    // conserva qué se compró para poder mostrarlo en el historial (informes finalizados).
    public interface IInformeSnapshotFaltanteRepository
    {
        // Persiste (reemplazando el previo) el snapshot de faltantes de un informe.
        // No llama SaveChanges: la persistencia la dispara el UnitOfWork al Commit.
        void Guardar(Guid idInformeCompra, IEnumerable<MaterialFaltante> materiales);

        // Lee el snapshot de faltantes de un informe. Lista vacía si no hay.
        List<MaterialFaltante> Leer(Guid idInformeCompra);
    }
}
