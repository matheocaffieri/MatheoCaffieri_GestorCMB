using DAL.FactoryDAL;
using DAL.LoginDAL;
using DomainModel.LoginDALInterfaces;
using Interfaces.LoginInterfaces;
using Services.LoginService;
using System;
using System.Net;
using System.Net.Mail;

namespace BL.LoginBL
{
    public class OtpService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPasswordHasher _hasher;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public OtpService(string connectionString, string smtpHost, int smtpPort, string smtpUser, string smtpPass)
        {
            var uow = new SqlLoginUnitOfWork(connectionString);
            _usuarioRepo = new UsuarioRepository(uow);
            _hasher = new PasswordHasher();
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
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

            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.Now.AddMinutes(15);

            usuario.Otp = otp;
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
            return string.Equals(usuario.Otp, otp.Trim(), StringComparison.Ordinal);
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
