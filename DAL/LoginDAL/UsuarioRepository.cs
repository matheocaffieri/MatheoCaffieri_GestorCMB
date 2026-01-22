using DomainModel;
using DomainModel.Login;
using DomainModel.LoginDALInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

// IMPORTANTE: este es el UoW correcto para este repo
using DAL.FactoryDAL;
namespace DAL.LoginDAL
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // B) Forzá el tipo exacto para evitar que tome el IUnitOfWork equivocado
        private readonly ILoginUnitOfWork _uow;

        public UsuarioRepository(ILoginUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        // =====================
        // CONTROL UoW (writes)
        // =====================
        private bool EnsureUowStarted()
        {
            // Para escrituras: si no hay conexión abierta o no hay transacción, arrancamos el UoW
            if (_uow.Connection == null ||
                _uow.Connection.State != ConnectionState.Open ||
                _uow.Transaction == null)
            {
                _uow.Begin();
                return true;
            }
            return false;
        }

        private void FinishUow(bool iStarted, bool success)
        {
            if (!iStarted) return;

            try
            {
                if (success) _uow.Commit();
                else _uow.Rollback();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }

        // =====================
        // HELPERS (reads)
        // =====================
        private SqlConnection RequireSqlConnection()
        {
            var cn = _uow.Connection as SqlConnection;
            if (cn == null) throw new InvalidOperationException("IUnitOfWork.Connection debe ser SqlConnection.");
            return cn;
        }

        private bool EnsureConnectionOpenForRead(out SqlConnection cn)
        {
            cn = RequireSqlConnection();
            var mustOpen = cn.State != ConnectionState.Open;

            if (mustOpen)
                cn.Open(); // lectura: NO inicia transacción

            return mustOpen;
        }

        private void CloseIfOpenedForRead(bool mustClose, SqlConnection cn)
        {
            // Cierro solo si yo abrí y no hay transacción activa
            if (mustClose && _uow.Transaction == null && cn.State == ConnectionState.Open)
                cn.Close();
        }

        // =====================
        // CRUD
        // =====================
        public void Add(Usuario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            bool iStarted = EnsureUowStarted();
            bool ok = false;

            try
            {
                using (var cmd = _uow.Connection.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = _uow.Transaction;

                    cmd.CommandText = @"
INSERT INTO dbo.Usuario (IdUsuario, Mail, [Contraseña], Telefono, Idioma, IsActive, Otp, OtpExpiry)
VALUES (@Id, @Mail, @Pass, @Tel, @Idi, @Act, @Otp, @OtpExp);";

                    Add(cmd, "@Id", entity.IdUsuario);
                    Add(cmd, "@Mail", entity.Mail, SqlDbType.NVarChar, 256);
                    Add(cmd, "@Pass", entity.Contraseña, SqlDbType.NVarChar, 256);
                    Add(cmd, "@Tel", entity.Telefono);
                    Add(cmd, "@Idi", (object)entity.Idioma ?? DBNull.Value);
                    Add(cmd, "@Act", entity.IsActive);
                    Add(cmd, "@Otp", (object)entity.Otp ?? DBNull.Value);
                    Add(cmd, "@OtpExp", (object)entity.OtpExpiry ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    ok = true;
                }
            }
            finally { FinishUow(iStarted, ok); }
        }

        public void Update(Usuario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            bool iStarted = EnsureUowStarted();
            bool ok = false;

            try
            {
                using (var cmd = _uow.Connection.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = _uow.Transaction;

                    cmd.CommandText = @"
UPDATE dbo.Usuario SET 
    Mail=@Mail, [Contraseña]=@Pass, Telefono=@Tel, Idioma=@Idi,
    IsActive=@Act, Otp=@Otp, OtpExpiry=@OtpExp
WHERE IdUsuario=@Id;";

                    Add(cmd, "@Id", entity.IdUsuario);
                    Add(cmd, "@Mail", entity.Mail, SqlDbType.NVarChar, 256);
                    Add(cmd, "@Pass", entity.Contraseña, SqlDbType.NVarChar, 256);
                    Add(cmd, "@Tel", entity.Telefono);
                    Add(cmd, "@Idi", (object)entity.Idioma ?? DBNull.Value);
                    Add(cmd, "@Act", entity.IsActive);
                    Add(cmd, "@Otp", (object)entity.Otp ?? DBNull.Value);
                    Add(cmd, "@OtpExp", (object)entity.OtpExpiry ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    ok = true;
                }
            }
            finally { FinishUow(iStarted, ok); }
        }

        public void Delete(object id)
        {
            bool iStarted = EnsureUowStarted();
            bool ok = false;

            try
            {
                using (var cmd = _uow.Connection.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = _uow.Transaction;

                    cmd.CommandText = "DELETE FROM dbo.Usuario WHERE IdUsuario=@Id;";
                    Add(cmd, "@Id", id);

                    cmd.ExecuteNonQuery();
                    ok = true;
                }
            }
            finally { FinishUow(iStarted, ok); }
        }

        public void SetActivo(Guid idUsuario, bool activo)
        {
            bool iStarted = EnsureUowStarted();
            bool ok = false;

            try
            {
                using (var cmd = _uow.Connection.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = _uow.Transaction;

                    cmd.CommandText = "UPDATE dbo.Usuario SET IsActive=@a WHERE IdUsuario=@id;";
                    Add(cmd, "@a", activo);
                    Add(cmd, "@id", idUsuario);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0) throw new InvalidOperationException("Usuario no encontrado.");
                    ok = true;
                }
            }
            finally { FinishUow(iStarted, ok); }
        }

        public IEnumerable<Usuario> List()
        {
            bool mustClose = EnsureConnectionOpenForRead(out var cn);

            try
            {
                using (var cmd = cn.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = (SqlTransaction)_uow.Transaction;

                    cmd.CommandText = @"
SELECT IdUsuario, Mail, [Contraseña], IsActive, Telefono, Idioma, Otp, OtpExpiry
FROM dbo.Usuario
ORDER BY Mail;";

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
            finally
            {
                CloseIfOpenedForRead(mustClose, cn);
            }
        }

        public Usuario FindByEmail(string mail)
        {
            if (string.IsNullOrWhiteSpace(mail))
                throw new ArgumentException("mail requerido.", nameof(mail));

            bool mustClose = EnsureConnectionOpenForRead(out var cn);

            try
            {
                using (var cmd = cn.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = (SqlTransaction)_uow.Transaction;

                    cmd.CommandText = @"
SELECT IdUsuario, Mail, [Contraseña] AS Contrasena, Telefono, Idioma, IsActive, Otp, OtpExpiry
FROM dbo.Usuario
WHERE Mail=@mail;";

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
            finally
            {
                CloseIfOpenedForRead(mustClose, cn);
            }
        }

        public Usuario GetById(object id)
        {
            bool mustClose = EnsureConnectionOpenForRead(out var cn);

            try
            {
                using (var cmd = cn.CreateCommand())
                {
                    if (_uow.Transaction != null) cmd.Transaction = (SqlTransaction)_uow.Transaction;

                    cmd.CommandText = @"
SELECT IdUsuario, Mail, [Contraseña], IsActive, Telefono, Idioma, Otp, OtpExpiry
FROM dbo.Usuario
WHERE IdUsuario=@Id;";

                    Add(cmd, "@Id", id);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read()) return null;

                        long tel = rd.IsDBNull(rd.GetOrdinal("Telefono")) ? 0L : rd.GetInt64(rd.GetOrdinal("Telefono"));

                        return new Usuario
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
            finally
            {
                CloseIfOpenedForRead(mustClose, cn);
            }
        }

        /* ============================================================
         *  ADAPTADORES IGenericRepository<Usuario>
         * ============================================================ */

        List<Usuario> DomainModel.Interfaces.IGenericRepository<Usuario>.GetAll()
            => new List<Usuario>(List());

        Usuario DomainModel.Interfaces.IGenericRepository<Usuario>.GetById(Guid id)
            => GetById((object)id);

        void DomainModel.Interfaces.IGenericRepository<Usuario>.Delete(Usuario entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
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
            var p = (SqlParameter)cmd.CreateParameter();
            p.ParameterName = name;
            p.SqlDbType = type;
            p.Size = size;
            p.Value = (object)value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }
    }
}
