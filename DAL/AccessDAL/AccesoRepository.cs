using DomainModel.Login;
using Interfaces;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace DAL.AccessDAL
{
    public class AccesoRepository : DALBase, IAccesoRepository
    {
        public AccesoRepository(string cs) : base(cs) { }

        public List<Acceso> GetAll()
        {
            var list = new List<Acceso>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT idAcceso, nombre, dataKey FROM Acceso ORDER BY nombre", cn))
            {
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
                        typeof(Acceso).GetProperty("Id").SetValue(acc, id);
                        list.Add(acc);
                    }
                }
            }
            return list;
        }

        public Acceso Create(string nombre, TipoPermiso dataKey)
        {
            var id = Guid.NewGuid();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                "INSERT INTO Acceso (idAcceso, nombre, dataKey) VALUES (@id, @n, @k)", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@k", dataKey.ToString());
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            var acc = new Acceso(nombre, dataKey);
            typeof(Acceso).GetProperty("Id").SetValue(acc, id);
            return acc;
        }

    }
}
