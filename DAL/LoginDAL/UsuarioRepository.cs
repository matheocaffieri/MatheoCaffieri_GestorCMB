using DomainModel;
using DomainModel.Interfaces;   // IUnitOfWork
using DomainModel.Login;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.LoginDAL
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IUnitOfWork _uow;

        public UsuarioRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }



        /* ============================================================
         *  MÉTODOS PRINCIPALES (idénticos a los que ya usabas)
         * ============================================================ */

        public void Add(Usuario entity)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                    INSERT INTO Usuario (IdUsuario, Mail, [Contraseña], Telefono, Idioma, IsActive, Otp, OtpExpiry)
                    VALUES (@Id, @Mail, @Pass, @Tel, @Idi, @Act, @Otp, @OtpExp)";
                Add(cmd, "@Id", entity.IdUsuario);
                Add(cmd, "@Mail", entity.Mail, SqlDbType.NVarChar, 256);
                Add(cmd, "@Pass", entity.Contraseña, SqlDbType.NVarChar, 256);
                Add(cmd, "@Tel", entity.Telefono);
                Add(cmd, "@Idi", (object)entity.Idioma ?? DBNull.Value);
                Add(cmd, "@Act", entity.IsActive);
                Add(cmd, "@Otp", (object)entity.Otp ?? DBNull.Value);
                Add(cmd, "@OtpExp", (object)entity.OtpExpiry ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Usuario entity)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                    UPDATE Usuario SET 
                      Mail=@Mail, [Contraseña]=@Pass, Telefono=@Tel, Idioma=@Idi,
                      IsActive=@Act, Otp=@Otp, OtpExpiry=@OtpExp
                    WHERE IdUsuario=@Id";
                Add(cmd, "@Id", entity.IdUsuario);
                Add(cmd, "@Mail", entity.Mail, SqlDbType.NVarChar, 256);
                Add(cmd, "@Pass", entity.Contraseña, SqlDbType.NVarChar, 256);
                Add(cmd, "@Tel", entity.Telefono);
                Add(cmd, "@Idi", (object)entity.Idioma ?? DBNull.Value);
                Add(cmd, "@Act", entity.IsActive);
                Add(cmd, "@Otp", (object)entity.Otp ?? DBNull.Value);
                Add(cmd, "@OtpExp", (object)entity.OtpExpiry ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(object id)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = "DELETE FROM Usuario WHERE IdUsuario=@Id";
                Add(cmd, "@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void SetActivo(Guid idUsuario, bool activo)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = "UPDATE Usuario SET IsActive=@a WHERE IdUsuario=@id";
                Add(cmd, "@a", activo);
                Add(cmd, "@id", idUsuario);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Usuario no encontrado.");
            }
        }

        public IEnumerable<Usuario> List()
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                    SELECT IdUsuario, Mail, [Contraseña], IsActive, Telefono, Idioma, Otp, OtpExpiry
                    FROM dbo.Usuario ORDER BY Mail;";
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        long tel = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? 0L : rd.GetInt64(rd.GetOrdinal("Telefono"));
                        yield return new Usuario
                        {
                            IdUsuario = rd.GetGuid(rd.GetOrdinal("IdUsuario")),
                            Mail = rd.GetString(rd.GetOrdinal("Mail")),
                            Contraseña = rd.GetString(rd.GetOrdinal("Contraseña")),
                            IsActive = rd.GetBoolean(rd.GetOrdinal("IsActive")),
                            Telefono = (int)Math.Min(tel, int.MaxValue),
                            Idioma = rd.IsDBNull(rd.GetOrdinal("Idioma")) ? null : rd.GetString(rd.GetOrdinal("Idioma")),
                            Otp = rd.IsDBNull(rd.GetOrdinal("Otp")) ? null : rd.GetString(rd.GetOrdinal("Otp")),
                            OtpExpiry = rd.IsDBNull(rd.GetOrdinal("OtpExpiry")) ? (DateTime?)null : rd.GetDateTime(rd.GetOrdinal("OtpExpiry"))
                        };
                    }
                }
            }
        }

        public Usuario FindByEmail(string mail)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = @"
                    SELECT IdUsuario, Mail, [Contraseña] AS Contrasena, Telefono, Idioma, IsActive, Otp, OtpExpiry
                    FROM dbo.Usuario WHERE Mail=@mail;";
                Add(cmd, "@mail", mail, SqlDbType.NVarChar, 256);

                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    long tel = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? 0L : rd.GetInt64(rd.GetOrdinal("Telefono"));
                    return new Usuario
                    {
                        IdUsuario = rd.GetGuid(rd.GetOrdinal("IdUsuario")),
                        Mail = rd.GetString(rd.GetOrdinal("Mail")),
                        Contraseña = rd.GetString(rd.GetOrdinal("Contrasena")),
                        Telefono = (int)Math.Min(tel, int.MaxValue),
                        Idioma = rd.IsDBNull(rd.GetOrdinal("Idioma")) ? null : rd.GetString(rd.GetOrdinal("Idioma")),
                        IsActive = rd.GetBoolean(rd.GetOrdinal("IsActive")),
                        Otp = rd.IsDBNull(rd.GetOrdinal("Otp")) ? null : rd.GetString(rd.GetOrdinal("Otp")),
                        OtpExpiry = rd.IsDBNull(rd.GetOrdinal("OtpExpiry")) ? (DateTime?)null : rd.GetDateTime(rd.GetOrdinal("OtpExpiry"))
                    };
                }
            }
        }

        public Usuario GetById(object id)
        {
            using (var cmd = _uow.Connection.CreateCommand())
            {
                cmd.Transaction = _uow.Transaction;
                cmd.CommandText = "SELECT * FROM Usuario WHERE IdUsuario=@Id";
                Add(cmd, "@Id", id);
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new Usuario
                    {
                        IdUsuario = rd.GetGuid(rd.GetOrdinal("IdUsuario")),
                        Mail = rd.GetString(rd.GetOrdinal("Mail")),
                        Contraseña = rd.GetString(rd.GetOrdinal("Contraseña")),
                        Telefono = rd.GetInt32(rd.GetOrdinal("Telefono")),
                        Idioma = rd.IsDBNull(rd.GetOrdinal("Idioma")) ? null : rd.GetString(rd.GetOrdinal("Idioma")),
                        IsActive = rd.GetBoolean(rd.GetOrdinal("IsActive")),
                        Otp = rd.IsDBNull(rd.GetOrdinal("Otp")) ? null : rd.GetString(rd.GetOrdinal("Otp")),
                        OtpExpiry = rd.IsDBNull(rd.GetOrdinal("OtpExpiry")) ? (DateTime?)null : rd.GetDateTime(rd.GetOrdinal("OtpExpiry"))
                    };
                }
            }
        }

        /* ============================================================
         *  ADAPTADORES para cumplir con IGenericRepository<Usuario>
         * ============================================================ */

        // Implementación explícita correcta para tu interfaz
        System.Collections.Generic.List<Usuario> DomainModel.Interfaces.IGenericRepository<Usuario>.GetAll()
        {
            // tu método List() devuelve IEnumerable<Usuario>,
            // así que simplemente lo convertís a List<Usuario>:
            return new List<Usuario>(List());
        }


        Usuario IGenericRepository<Usuario>.GetById(Guid id) => GetById((object)id);

        void IGenericRepository<Usuario>.Delete(Usuario entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Delete((object)entity.IdUsuario);
        }

        /* ============================================================
         *  HELPERS internos
         * ============================================================ */

        private static void Add(IDbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static void Add(IDbCommand cmd, string name, string value, SqlDbType type, int size)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            ((SqlParameter)p).SqlDbType = type;
            ((SqlParameter)p).Size = size;
            p.Value = (object)value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}
