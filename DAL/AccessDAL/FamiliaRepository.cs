using DomainModel.Login;
using Interfaces.LoginInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DomainModel.LoginDALInterfaces;

namespace DAL.AccessDAL
{
    public class FamiliaRepository : DALBase, IFamiliaRepository
    {
        public FamiliaRepository(string cs) : base(cs) { }

        public List<(Guid Id, string Nombre)> GetAll()
        {
            var list = new List<(Guid, string)>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT idFamilia, nombre FROM Familia ORDER BY nombre", cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add((rd.GetGuid(0), rd.GetString(1)));
                }
            }

            return list;
        }

        public Guid Create(string nombre)
        {
            var id = Guid.NewGuid();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                "INSERT INTO Familia (idFamilia, nombre) VALUES (@id, @n)", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@n", nombre);
                cn.Open();
                cmd.ExecuteNonQuery();
            }

            return id;
        }

        public List<Acceso> GetAccesos(Guid idFamilia)
        {
            var list = new List<Acceso>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                @"SELECT a.idAcceso, a.nombre, a.dataKey
                  FROM Familia_Acceso fa
                  JOIN Acceso a ON a.idAcceso = fa.idAcceso
                  WHERE fa.idFamilia = @f
                  ORDER BY a.nombre", cn))
            {
                cmd.Parameters.AddWithValue("@f", idFamilia);
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

        public void AddAcceso(Guid idFamilia, Guid idAcceso)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                @"IF NOT EXISTS(SELECT 1 FROM Familia_Acceso WHERE idFamilia=@f AND idAcceso=@a)
                  INSERT INTO Familia_Acceso (idFamilia,idAcceso) VALUES (@f,@a)", cn))
            {
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cmd.Parameters.AddWithValue("@a", idAcceso);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveAcceso(Guid idFamilia, Guid idAcceso)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                "DELETE FROM Familia_Acceso WHERE idFamilia=@f AND idAcceso=@a", cn))
            {
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cmd.Parameters.AddWithValue("@a", idAcceso);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Usuario> GetUsuarios(Guid idFamilia)
        {
            var list = new List<Usuario>();

            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                @"SELECT u.idUsuario, u.mail, u.isActive, u.telefono, u.idioma
                  FROM Usuario_Familia uf
                  JOIN Usuario u ON u.idUsuario = uf.idUsuario
                  WHERE uf.idFamilia = @f
                  ORDER BY u.mail", cn))
            {
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cn.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Usuario
                        {
                            IdUsuario = rd.GetGuid(0),
                            Mail = rd.GetString(1),
                            IsActive = rd.GetBoolean(2),
                            Telefono = rd.IsDBNull(3)
                                ? 0
                                : (int)Math.Max(int.MinValue, Math.Min(int.MaxValue, rd.GetInt64(3))),
                            Idioma = rd.IsDBNull(4) ? "es" : rd.GetString(4)
                        });
                    }
                }
            }

            return list;
        }

        public void AddUsuario(Guid idFamilia, Guid idUsuario)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                @"IF NOT EXISTS(SELECT 1 FROM Usuario_Familia WHERE idUsuario=@u AND idFamilia=@f)
                  INSERT INTO Usuario_Familia (idUsuario,idFamilia) VALUES (@u,@f)", cn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveUsuario(Guid idFamilia, Guid idUsuario)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(
                "DELETE FROM Usuario_Familia WHERE idUsuario=@u AND idFamilia=@f", cn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }





        public List<(Guid Id, string Nombre)> GetRolesDeUsuario(Guid idUsuario)
        {
            var list = new List<(Guid, string)>();
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(@"
            SELECT f.idFamilia, f.nombre
            FROM Usuario_Familia uf
            JOIN Familia f ON f.idFamilia = uf.idFamilia
            WHERE uf.idUsuario = @u
            ORDER BY f.nombre;", cn))
            {
                cmd.Parameters.AddWithValue("@u", idUsuario);
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        list.Add((rd.GetGuid(0), rd.GetString(1)));
                }
            }
            return list;
        }
    }
}
