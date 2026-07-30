using DAL.DAL_Interfaces;
using DomainModel.Integridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Integridad
{
    /// <summary>
    /// Dígitos verificadores por ADO.NET sobre la BD de dominio (GestorCMB).
    /// Lee las tablas crudas (SELECT *) para verificar exactamente lo que una
    /// manipulación por SQL externo tocaría, sin depender del mapeo EF ni del EDMX.
    /// </summary>
    public class IntegridadRepository : IIntegridadRepository
    {
        // Tablas de dominio protegidas (mismos nombres que en la BD / entidades EF).
        private static readonly string[] TablasProtegidas =
        {
            "Cliente", "Empleado", "Material", "Proveedor", "Inventario", "Proyecto",
            "Informe_compra", "Informe_monto", "Informe_snapshot_faltante",
            "Detalle_informe_material_faltante", "Detalle_proyecto_empleado",
            "Detalle_proyecto_material", "Material_faltante"
        };

        private static string ConnString =>
            ConfigurationManager.ConnectionStrings["GestorCMBConnection"].ConnectionString;

        public static bool EsProtegida(string tabla) =>
            TablasProtegidas.Contains(tabla, StringComparer.Ordinal);

        // ---------- API pública (abre su propia conexión) ----------

        public bool HayLineaBase()
        {
            using (var cn = new SqlConnection(ConnString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Digito_Verificador", cn))
                    return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
            }
        }

        public void RecalcularTodo()
        {
            using (var cn = new SqlConnection(ConnString))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    foreach (var t in TablasProtegidas)
                        RecalcularTabla(t, cn, tx);
                    tx.Commit();
                }
            }
        }

        public List<IntegridadAnomalia> Verificar()
        {
            var anomalias = new List<IntegridadAnomalia>();
            using (var cn = new SqlConnection(ConnString))
            {
                cn.Open();
                foreach (var t in TablasProtegidas)
                    VerificarTabla(t, cn, anomalias);
            }
            return anomalias;
        }

        // ---------- Usado por el hook de SaveChanges ----------

        /// <summary>Recalcula tablas reusando la conexión/transacción del context EF (atómico con la escritura).</summary>
        public void RecalcularTablas(ICollection<string> tablas, SqlConnection cn, SqlTransaction tx)
        {
            foreach (var t in tablas.Where(EsProtegida))
                RecalcularTabla(t, cn, tx);
        }

        /// <summary>Recalcula tablas abriendo conexión propia (cuando la escritura no usó transacción explícita).</summary>
        public void RecalcularTablas(ICollection<string> tablas)
        {
            var protegidas = tablas.Where(EsProtegida).ToList();
            if (protegidas.Count == 0) return;

            using (var cn = new SqlConnection(ConnString))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    foreach (var t in protegidas)
                        RecalcularTabla(t, cn, tx);
                    tx.Commit();
                }
            }
        }

        // ---------- Núcleo ----------

        private void RecalcularTabla(string tabla, SqlConnection cn, SqlTransaction tx)
        {
            var dvhPorClave = CalcularDvhTabla(tabla, cn, tx);

            using (var del = new SqlCommand("DELETE FROM dbo.Digito_Verificador WHERE Tabla = @t", cn, tx))
            {
                del.Parameters.AddWithValue("@t", tabla);
                del.ExecuteNonQuery();
            }

            foreach (var kv in dvhPorClave)
            {
                using (var ins = new SqlCommand(
                    "INSERT INTO dbo.Digito_Verificador (Tabla, ClaveRegistro, DVH) VALUES (@t, @c, @h)", cn, tx))
                {
                    ins.Parameters.AddWithValue("@t", tabla);
                    ins.Parameters.AddWithValue("@c", kv.Key);
                    ins.Parameters.AddWithValue("@h", kv.Value);
                    ins.ExecuteNonQuery();
                }
            }

            var dvv = DigitoVerificador.CalcularDvv(dvhPorClave);
            using (var up = new SqlCommand(
                @"UPDATE dbo.Digito_Verificador_Tabla SET DVV = @v WHERE Tabla = @t;
                  IF @@ROWCOUNT = 0 INSERT INTO dbo.Digito_Verificador_Tabla (Tabla, DVV) VALUES (@t, @v);", cn, tx))
            {
                up.Parameters.AddWithValue("@t", tabla);
                up.Parameters.AddWithValue("@v", dvv);
                up.ExecuteNonQuery();
            }
        }

        private void VerificarTabla(string tabla, SqlConnection cn, List<IntegridadAnomalia> anomalias)
        {
            var actual = CalcularDvhTabla(tabla, cn, null);
            var guardado = LeerDvhGuardados(tabla, cn);
            var dvvGuardado = LeerDvvGuardado(tabla, cn);

            var antes = anomalias.Count;

            foreach (var kv in actual)
            {
                if (!guardado.TryGetValue(kv.Key, out var h))
                    anomalias.Add(new IntegridadAnomalia(tabla, kv.Key, TipoAnomalia.Agregado));
                else if (!string.Equals(h, kv.Value, StringComparison.Ordinal))
                    anomalias.Add(new IntegridadAnomalia(tabla, kv.Key, TipoAnomalia.Modificado));
            }
            foreach (var kv in guardado)
            {
                if (!actual.ContainsKey(kv.Key))
                    anomalias.Add(new IntegridadAnomalia(tabla, kv.Key, TipoAnomalia.Eliminado));
            }

            // Chequeo vertical: si el DVV no coincide pero las filas dieron OK, alguien
            // tocó el DVV guardado directamente. Lo reportamos solo si no hubo hallazgos de fila.
            var dvvActual = DigitoVerificador.CalcularDvv(actual);
            var dvvOk = dvvGuardado != null && string.Equals(dvvGuardado, dvvActual, StringComparison.Ordinal);
            if (!dvvOk && anomalias.Count == antes)
                anomalias.Add(new IntegridadAnomalia(tabla, null, TipoAnomalia.DigitoTablaInvalido));
        }

        private Dictionary<string, string> CalcularDvhTabla(string tabla, SqlConnection cn, SqlTransaction tx)
        {
            var pkCols = GetPkColumns(tabla, cn, tx);
            if (pkCols.Count == 0)
                throw new InvalidOperationException($"La tabla '{tabla}' no tiene PK; no se puede calcular su DVH.");

            var dict = new Dictionary<string, string>(StringComparer.Ordinal);

            // El nombre de tabla proviene de la whitelist, no de input de usuario (seguro contra inyección).
            using (var cmd = new SqlCommand($"SELECT * FROM [{tabla}]", cn, tx))
            using (var rd = cmd.ExecuteReader())
            {
                while (rd.Read())
                {
                    var columnas = new Dictionary<string, object>(StringComparer.Ordinal);
                    for (var i = 0; i < rd.FieldCount; i++)
                        columnas[rd.GetName(i)] = rd.GetValue(i);

                    var clave = string.Join("|", pkCols.Select(pk => FormatearClave(columnas[pk])));
                    dict[clave] = DigitoVerificador.CalcularDvh(columnas);
                }
            }
            return dict;
        }

        private static List<string> GetPkColumns(string tabla, SqlConnection cn, SqlTransaction tx)
        {
            var cols = new List<string>();
            const string sql = @"
                SELECT c.name
                FROM sys.indexes i
                JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                JOIN sys.columns c        ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                WHERE i.is_primary_key = 1 AND i.object_id = OBJECT_ID(@t)
                ORDER BY ic.key_ordinal";
            using (var cmd = new SqlCommand(sql, cn, tx))
            {
                cmd.Parameters.AddWithValue("@t", "dbo." + tabla);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read()) cols.Add(rd.GetString(0));
            }
            return cols;
        }

        private static Dictionary<string, string> LeerDvhGuardados(string tabla, SqlConnection cn)
        {
            var dict = new Dictionary<string, string>(StringComparer.Ordinal);
            using (var cmd = new SqlCommand("SELECT ClaveRegistro, DVH FROM dbo.Digito_Verificador WHERE Tabla = @t", cn))
            {
                cmd.Parameters.AddWithValue("@t", tabla);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read()) dict[rd.GetString(0)] = rd.GetString(1);
            }
            return dict;
        }

        private static string LeerDvvGuardado(string tabla, SqlConnection cn)
        {
            using (var cmd = new SqlCommand("SELECT DVV FROM dbo.Digito_Verificador_Tabla WHERE Tabla = @t", cn))
            {
                cmd.Parameters.AddWithValue("@t", tabla);
                var r = cmd.ExecuteScalar();
                return r == null || r == DBNull.Value ? null : (string)r;
            }
        }

        private static string FormatearClave(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is IFormattable f) return f.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
            return v.ToString();
        }
    }
}
