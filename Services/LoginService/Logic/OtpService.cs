using Services.LoginService.DataAccess;
using Services.Services_Interfaces;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Services.LoginService.Logic
{
    public class OtpService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        // El hash BCrypt del OTP ocupa 60 caracteres; la columna debe tener al menos ese ancho.
        private const int OtpColumnMinLength = 100;
        private static bool _esquemaVerificado;

        public OtpService(string connectionString, string smtpHost, int smtpPort, string smtpUser, string smtpPass)
        {
            var uow = new SqlLoginUnitOfWork(connectionString);
            _usuarioRepo = new UsuarioRepository(uow);
            _hasher = new PasswordHasher();
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            EnsureOtpColumnWidth(connectionString);
        }

        /// <summary>
        /// Si la columna Usuario.otp quedó más chica que el hash BCrypt (p. ej. tras
        /// regenerar la BD desde un script viejo), la ensancha automáticamente.
        /// Se ejecuta una sola vez por proceso.
        /// </summary>
        private static void EnsureOtpColumnWidth(string connectionString)
        {
            if (_esquemaVerificado)
                return;

            string sql = @"
IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Usuario' AND COLUMN_NAME = 'otp'
      AND CHARACTER_MAXIMUM_LENGTH BETWEEN 1 AND " + (OtpColumnMinLength - 1) + @"
)
    ALTER TABLE dbo.Usuario ALTER COLUMN otp VARCHAR(" + OtpColumnMinLength + @") NULL;";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                _esquemaVerificado = true;
            }
            catch
            {
                // Sin permisos de ALTER no podemos auto-reparar; si la columna está bien,
                // el envío de OTP funciona igual y si no, el error real aparece al guardar.
            }
        }

        /// <summary>
        /// Genera un OTP, lo guarda en la BD y lo envía por mail.
        /// Devuelve false si el mail no corresponde a ningún usuario activo.
        /// </summary>
        public bool EnviarOtp(string mail)
        {
            var usuario = _usuarioRepo.FindByEmail(mail);
            if (usuario == null || !usuario.IsActive)
                return false;

            var otp = GenerarOtpSeguro();
            var expiry = DateTime.Now.AddMinutes(15);

            usuario.Otp = _hasher.Hash(otp);   // se guarda hasheado, no en texto plano
            usuario.OtpExpiry = expiry;
            _usuarioRepo.Update(usuario);

            EnviarMail(mail, otp, expiry);
            return true;
        }

        /// <summary>
        /// Valida que el OTP coincida y no haya expirado.
        /// </summary>
        public bool ValidarOtp(string mail, string otp)
        {
            var usuario = _usuarioRepo.FindByEmail(mail);
            if (usuario?.Otp == null || usuario.OtpExpiry == null)
                return false;
            if (DateTime.Now > usuario.OtpExpiry.Value)
                return false;
            return _hasher.Verify(usuario.Otp, otp.Trim());
        }

        /// <summary>
        /// Cambia la contraseña y limpia el OTP de la BD.
        /// </summary>
        public void CambiarContraseña(string mail, string nuevaContraseña)
        {
            var usuario = _usuarioRepo.FindByEmail(mail);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no encontrado.");

            usuario.Contraseña = _hasher.Hash(nuevaContraseña);
            usuario.Otp = null;
            usuario.OtpExpiry = null;
            _usuarioRepo.Update(usuario);
        }

        /// <summary>
        /// Genera un OTP de 6 dígitos usando un RNG criptográfico (no Random, que es predecible).
        /// </summary>
        private static string GenerarOtpSeguro()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                // valor 0..999999 sin sesgo perceptible para este rango
                var valor = (int)(BitConverter.ToUInt32(bytes, 0) % 1000000);
                return valor.ToString("D6");
            }
        }

        private void EnviarMail(string destinatario, string otp, DateTime expiry)
        {
            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);

                var msg = new MailMessage
                {
                    From = new MailAddress(_smtpUser, "CMB Instalaciones"),
                    Subject = "Código de recuperación de contraseña",
                    Body =
                        $"Su código de recuperación es: {otp}\r\n\r\n" +
                        $"Válido hasta las {expiry:HH:mm} del {expiry:dd/MM/yyyy}.\r\n\r\n" +
                        $"Si no solicitó este código, ignórelo.",
                    IsBodyHtml = false
                };
                msg.To.Add(destinatario);
                client.Send(msg);
            }
        }
    }
}
