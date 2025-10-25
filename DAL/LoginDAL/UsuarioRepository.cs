using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DAL.LoginDAL
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string cs)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MatheoCaffieri_GestorCMB.Properties.Settings.ConnUsuarios"].ConnectionString;
        }

        public void Add(Usuario entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Usuario (IdUsuario, Mail, Contraseña, Telefono, Idioma, IsActive, Otp, OtpExpiry)
                              VALUES (@Id, @Mail, @Pass, @Tel, @Idi, @Act, @Otp, @OtpExp)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@Mail", entity.Mail);
                    cmd.Parameters.AddWithValue("@Pass", entity.Contraseña);
                    cmd.Parameters.AddWithValue("@Tel", entity.Telefono);
                    cmd.Parameters.AddWithValue("@Idi", entity.Idioma);
                    cmd.Parameters.AddWithValue("@Act", entity.IsActive);
                    cmd.Parameters.AddWithValue("@Otp", entity.Otp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OtpExp", entity.OtpExpiry ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Usuario entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Usuario WHERE IdUsuario = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", entity.IdUsuario);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SetActivo(Guid idUsuario, bool activo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Usuario SET isActive = @a WHERE idUsuario = @id", conn))
            {
                cmd.Parameters.AddWithValue("@a", activo);
                cmd.Parameters.AddWithValue("@id", idUsuario);
                conn.Open();
                var rows = cmd.ExecuteNonQuery();
                if (rows == 0) throw new InvalidOperationException("Usuario no encontrado.");
            }
        }

        public List<Usuario> GetAll()
        {
            var lista = new List<Usuario>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT idUsuario, mail, [contraseña], isActive, telefono, idioma, otp
        FROM dbo.Usuario
        ORDER BY mail;", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var ordId = reader.GetOrdinal("idUsuario");
                    var ordMail = reader.GetOrdinal("mail");
                    var ordPass = reader.GetOrdinal("contraseña"); // con ñ
                    var ordAct = reader.GetOrdinal("isActive");
                    var ordTel = reader.GetOrdinal("telefono");
                    var ordIdi = reader.GetOrdinal("idioma");
                    var ordOtp = reader.GetOrdinal("otp");

                    while (reader.Read())
                    {
                        long tel = reader.IsDBNull(ordTel) ? 0L : reader.GetInt64(ordTel);

                        lista.Add(new Usuario
                        {
                            IdUsuario = reader.GetGuid(ordId),
                            Mail = reader.GetString(ordMail),
                            Contraseña = reader.GetString(ordPass),
                            IsActive = reader.GetBoolean(ordAct),
                            Telefono = (int)Math.Min(tel, int.MaxValue), // si tu modelo es int
                            Idioma = reader.IsDBNull(ordIdi) ? null : reader.GetString(ordIdi),
                            Otp = reader.IsDBNull(ordOtp) ? null : reader.GetString(ordOtp)
                        });
                    }
                }
            }
            return lista;
        }




        public Usuario FindByEmail(string mail)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT 
            u.idUsuario,
            u.mail,
            u.[contraseña]     AS contrasena,
            u.telefono,
            u.idioma,
            u.isActive,
            u.otp,
            u.otpExpiry
        FROM dbo.Usuario u
        WHERE u.mail = @mail;", conn))
            {
                // Evitá AddWithValue cuando puedas; forzá tipo/tamaño razonable
                cmd.Parameters.Add("@mail", System.Data.SqlDbType.NVarChar, 256).Value = mail;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    var ordId = reader.GetOrdinal("idUsuario");
                    var ordMail = reader.GetOrdinal("mail");
                    var ordPass = reader.GetOrdinal("contrasena");
                    var ordTel = reader.GetOrdinal("telefono");
                    var ordIdi = reader.GetOrdinal("idioma");
                    var ordAct = reader.GetOrdinal("isActive");
                    var ordOtp = reader.GetOrdinal("otp");
                    var ordExp = reader.GetOrdinal("otpExpiry");

                    // telefono puede ser BIGINT o NULL
                    long telLong = reader.IsDBNull(ordTel) ? 0L : reader.GetInt64(ordTel);

                    return new Usuario
                    {
                        IdUsuario = reader.GetGuid(ordId),
                        Mail = reader.GetString(ordMail),
                        Contraseña = reader.GetString(ordPass), // viene como [contraseña]
                        Telefono = (int)Math.Min(telLong, int.MaxValue), // si tu entidad usa int
                        Idioma = reader.IsDBNull(ordIdi) ? null : reader.GetString(ordIdi),
                        IsActive = reader.GetBoolean(ordAct),
                        Otp = reader.IsDBNull(ordOtp) ? null : reader.GetString(ordOtp),
                        OtpExpiry = reader.IsDBNull(ordExp) ? (DateTime?)null : reader.GetDateTime(ordExp)
                    };
                }
            }
        }


        public void Update(Usuario entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Usuario
                      SET Mail = @Mail, 
                          Contraseña = @Contraseña,
                          Telefono = @Telefono,
                          Idioma = @Idioma,
                          IsActive = @IsActive,
                          Otp = @Otp,
                          OtpExpiry = @OtpExpiry
                      WHERE IdUsuario = @IdUsuario";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", entity.IdUsuario);
                    cmd.Parameters.AddWithValue("@Mail", entity.Mail);
                    cmd.Parameters.AddWithValue("@Contraseña", entity.Contraseña);
                    cmd.Parameters.AddWithValue("@Telefono", entity.Telefono);
                    cmd.Parameters.AddWithValue("@Idioma", entity.Idioma);
                    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cmd.Parameters.AddWithValue("@Otp", entity.Otp != null ? (object)entity.Otp : DBNull.Value);
                    cmd.Parameters.AddWithValue("@OtpExpiry", entity.OtpExpiry.HasValue ? (object)entity.OtpExpiry.Value : DBNull.Value);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public Usuario GetById(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Usuario WHERE IdUsuario = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetGuid(reader.GetOrdinal("IdUsuario")),
                                Mail = reader.GetString(reader.GetOrdinal("Mail")),
                                Contraseña = reader.GetString(reader.GetOrdinal("Contraseña")),
                                Telefono = reader.GetInt32(reader.GetOrdinal("Telefono")),
                                Idioma = reader.GetString(reader.GetOrdinal("Idioma")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                Otp = reader.IsDBNull(reader.GetOrdinal("Otp")) ? null : reader.GetString(reader.GetOrdinal("Otp")),
                                OtpExpiry = reader.IsDBNull(reader.GetOrdinal("OtpExpiry")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("OtpExpiry"))
                            };
                        }
                    }
                }
            }

            return null;
        }

    }
}
