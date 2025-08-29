using DomainModel.Login;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.AccessDAL
{
    public class UsuarioAccesoRepository : DALBase, IUsuarioAccesoRepository
    {
        public UsuarioAccesoRepository(string cs) : base(cs) { }

        public List<Acceso> GetDirectos(Guid idUsuario)
        {
            var list = new List<Acceso>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                @"SELECT a.idAcceso, a.nombre, a.dataKey
                  FROM Usuario_Acceso ua
                  JOIN Acceso a ON a.idAcceso = ua.idAcceso
                  WHERE ua.idUsuario = @u
                  ORDER BY a.nombre", cn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cn.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var id = rd.GetGuid(0);
                        var nombre = rd.GetString(1);
                        var keyStr = rd.GetString(2);
                        var tipo = (TipoPermiso)Enum.Parse(typeof(TipoPermiso), keyStr, true);

                        var acc = new Acceso(nombre, tipo);
                        typeof(Acceso).GetProperty("Id")?.SetValue(acc, id);
                        list.Add(acc);
                    }
                }
            }

            return list;
        }

        public void ReplaceDirectos(Guid idUsuario, IEnumerable<Guid> idsAcceso)
        {
            using (var cn = new SqlConnection(_cs))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        using (var del = new SqlCommand(
                            "DELETE FROM Usuario_Acceso WHERE idUsuario=@u", cn, tx))
                        {
                            del.Parameters.AddWithValue("@u", idUsuario);
                            del.ExecuteNonQuery();
                        }

                        foreach (var idA in new HashSet<Guid>(idsAcceso))
                        {
                            using (var ins = new SqlCommand(
                                "INSERT INTO Usuario_Acceso (idUsuario,idAcceso) VALUES (@u,@a)", cn, tx))
                            {
                                ins.Parameters.AddWithValue("@u", idUsuario);
                                ins.Parameters.AddWithValue("@a", idA);
                                ins.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
