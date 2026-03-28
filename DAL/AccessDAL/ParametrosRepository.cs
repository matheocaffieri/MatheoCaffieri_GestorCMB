using DomainModel.Login;
using DomainModel.LoginDALInterfaces;
using System;
using System.Data.SqlClient;

namespace DAL.AccessDAL
{
    public class ParametrosRepository : DALBase, IParametrosRepository
    {
        public ParametrosRepository(string cs) : base(cs) { }

        public void EnsureTableAndSeed()
        {
            using (var cn = new SqlConnection(_cs))
            {
                cn.Open();

                const string createTable = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Parametros')
                    BEGIN
                        CREATE TABLE Parametros (
                            IdParametro        uniqueidentifier NOT NULL PRIMARY KEY,
                            MargenEmpleados    decimal(5,4)     NOT NULL DEFAULT 0.2000,
                            MargenMateriales   decimal(5,4)     NOT NULL DEFAULT 0.0000,
                            UtilidadEmpresa    decimal(5,4)     NOT NULL DEFAULT 0.1000,
                            UltimaModificacion datetime         NOT NULL DEFAULT GETDATE(),
                            ModificadoPor      uniqueidentifier NULL
                        )
                    END";

                using (var cmd = new SqlCommand(createTable, cn))
                    cmd.ExecuteNonQuery();

                const string seedRow = @"
                    IF NOT EXISTS (SELECT 1 FROM Parametros)
                    BEGIN
                        INSERT INTO Parametros (IdParametro, MargenEmpleados, MargenMateriales, UtilidadEmpresa, UltimaModificacion)
                        VALUES (NEWID(), 0.2000, 0.0000, 0.1000, GETDATE())
                    END";

                using (var cmd = new SqlCommand(seedRow, cn))
                    cmd.ExecuteNonQuery();
            }
        }

        public Parametros Obtener()
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 IdParametro, MargenEmpleados, MargenMateriales, UtilidadEmpresa, UltimaModificacion, ModificadoPor FROM Parametros", cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;

                    return new Parametros
                    {
                        IdParametro        = rd.GetGuid(0),
                        MargenEmpleados    = rd.GetDecimal(1),
                        MargenMateriales   = rd.GetDecimal(2),
                        UtilidadEmpresa    = rd.GetDecimal(3),
                        UltimaModificacion = rd.GetDateTime(4),
                        ModificadoPor      = rd.IsDBNull(5) ? (Guid?)null : rd.GetGuid(5)
                    };
                }
            }
        }

        public void Guardar(Parametros p)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(@"
                UPDATE Parametros
                SET MargenEmpleados    = @emp,
                    MargenMateriales   = @mat,
                    UtilidadEmpresa    = @util,
                    UltimaModificacion = @fecha,
                    ModificadoPor      = @modificadoPor
                WHERE IdParametro = @id", cn))
            {
                cmd.Parameters.AddWithValue("@emp",          p.MargenEmpleados);
                cmd.Parameters.AddWithValue("@mat",          p.MargenMateriales);
                cmd.Parameters.AddWithValue("@util",         p.UtilidadEmpresa);
                cmd.Parameters.AddWithValue("@fecha",        p.UltimaModificacion);
                cmd.Parameters.AddWithValue("@modificadoPor", (object)p.ModificadoPor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id",           p.IdParametro);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
