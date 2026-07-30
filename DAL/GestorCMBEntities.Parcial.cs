using DAL.Integridad;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DAL
{
    public partial class GestorCMBEntities : DbContext
    {
        public GestorCMBEntities(EntityConnection entityConnection, bool contextOwnsConnection)
            : base(entityConnection, contextOwnsConnection)
        {
        }

        // Tras cada escritura legítima recalculamos los dígitos de las tablas afectadas.
        // Es el único punto por el que pasan TODAS las escrituras EF: una manipulación por
        // SQL externo no llega acá, y por eso la verificación posterior la detecta.
        public override int SaveChanges()
        {
            var tablas = TablasAfectadas();
            var result = base.SaveChanges();
            if (tablas.Count > 0)
                RecalcularIntegridad(tablas);
            return result;
        }

        private HashSet<string> TablasAfectadas()
        {
            var set = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in ChangeTracker.Entries())
            {
                if (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                {
                    // GetObjectType desenvuelve el proxy dinámico de EF para obtener el tipo real (= nombre de tabla).
                    var tipo = ObjectContext.GetObjectType(e.Entity.GetType());
                    if (IntegridadRepository.EsProtegida(tipo.Name))
                        set.Add(tipo.Name);
                }
            }
            return set;
        }

        private void RecalcularIntegridad(HashSet<string> tablas)
        {
            try
            {
                var repo = new IntegridadRepository();
                var tx = Database.CurrentTransaction;
                if (tx != null)
                {
                    // Hay transacción de UnitOfWork abierta: reusamos su conexión/transacción → atómico.
                    var cn = (SqlConnection)Database.Connection;
                    var sqlTx = (SqlTransaction)tx.UnderlyingTransaction;
                    repo.RecalcularTablas(tablas, cn, sqlTx);
                }
                else
                {
                    // Sin transacción explícita: base.SaveChanges ya persistió; abrimos conexión propia.
                    repo.RecalcularTablas(tablas);
                }
            }
            catch (Exception ex)
            {
                // No rompemos la operación de negocio por un fallo del bookkeeping de integridad.
                // (DAL no referencia Services.Logs para no invertir la dependencia de capas.)
                Trace.WriteLine("[Integridad] Error recalculando dígitos tras SaveChanges: " + ex);
            }
        }
    }
}
