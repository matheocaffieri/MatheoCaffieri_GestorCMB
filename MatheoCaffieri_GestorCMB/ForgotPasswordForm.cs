using Services.LoginService.Logic;
using Services.Language;
using Services.Logs;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB
{
    public class ForgotPasswordForm : Form
    {
        private readonly OtpService _otpService;
        private string _mailConfirmado;

        // Step 1
        private Panel _panelStep1;
        private TextBox _txtMail;
        private Button _btnEnviar;

        // Step 2
        private Panel _panelStep2;
        private Label _lblInfoMail;
        private TextBox _txtOtp;
        private TextBox _txtNewPass;
        private TextBox _txtConfirmPass;
        private Button _btnCambiar;

        public ForgotPasswordForm(string connectionString)
        {
            var smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp-mail.outlook.com";
            var smtpPort = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out int port) ? port : 587;
            var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            var smtpPass = ConfigurationManager.AppSettings["SmtpPass"];

            _otpService = new OtpService(connectionString, smtpHost, smtpPort, smtpUser, smtpPass);

            BuildUI();
        }

        private static string T(string key, string fallback) =>
            LanguageService.Current?.T(key) ?? fallback;

        private void BuildUI()
        {
            Text = T("cap_recuperar_contrasena", "Recuperar contraseña");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 190);

            // ===== Paso 1 =====
            _panelStep1 = new Panel { Location = Point.Empty, Size = ClientSize };

            var lblTitle = new Label
            {
                Text = T("lbl_ingrese_mail", "Ingrese su correo electrónico"),
                AutoSize = true,
                Location = new Point(20, 25)
            };

            _txtMail = new TextBox
            {
                Location = new Point(20, 55),
                Width = 375,
                TabIndex = 0
            };

            _btnEnviar = new Button
            {
                Text = T("btn_enviar_codigo", "Enviar código"),
                Location = new Point(255, 100),
                Width = 140,
                TabIndex = 1
            };
            _btnEnviar.Click += BtnEnviar_Click;

            // Allow pressing Enter
            AcceptButton = _btnEnviar;

            _panelStep1.Controls.AddRange(new Control[] { lblTitle, _txtMail, _btnEnviar });
            Controls.Add(_panelStep1);

            // ===== Paso 2 =====
            _panelStep2 = new Panel { Location = Point.Empty, Size = new Size(420, 340), Visible = false };

            _lblInfoMail = new Label
            {
                Text = "",
                AutoSize = false,
                Width = 380,
                Height = 32,
                Location = new Point(20, 15)
            };

            var lblOtp = new Label { Text = T("lbl_codigo_verificacion", "Código de verificación:"), AutoSize = true, Location = new Point(20, 58) };
            _txtOtp = new TextBox { Location = new Point(20, 78), Width = 120, MaxLength = 6, TabIndex = 0 };

            var lblNewPass = new Label { Text = T("lbl_nueva_contrasena", "Nueva contraseña:"), AutoSize = true, Location = new Point(20, 118) };
            _txtNewPass = new TextBox { Location = new Point(20, 138), Width = 375, PasswordChar = '●', TabIndex = 1 };

            var lblConfirm = new Label { Text = T("lbl_confirmar_contrasena", "Confirmar contraseña:"), AutoSize = true, Location = new Point(20, 178) };
            _txtConfirmPass = new TextBox { Location = new Point(20, 198), Width = 375, PasswordChar = '●', TabIndex = 2 };

            _btnCambiar = new Button
            {
                Text = T("btn_cambiar_contrasena", "Cambiar contraseña"),
                Location = new Point(255, 245),
                Width = 140,
                TabIndex = 3
            };
            _btnCambiar.Click += BtnCambiar_Click;

            _panelStep2.Controls.AddRange(new Control[]
            {
                _lblInfoMail, lblOtp, _txtOtp, lblNewPass, _txtNewPass, lblConfirm, _txtConfirmPass, _btnCambiar
            });
            Controls.Add(_panelStep2);
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            var mail = _txtMail.Text.Trim();
            if (string.IsNullOrEmpty(mail))
            {
                LoggerLogic.Warn("[ForgotPasswordForm] Validación: mail vacío al solicitar OTP.");
                MessageBox.Show(
                    T("val_recuperar_mail_requerido", "Ingrese su correo electrónico."),
                    T("cap_campo_requerido", "Campo requerido"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _btnEnviar.Enabled = false;
            _btnEnviar.Text = T("btn_enviando", "Enviando...");
            Cursor = Cursors.WaitCursor;

            try
            {
                bool enviado = _otpService.EnviarOtp(mail);
                if (!enviado)
                {
                    LoggerLogic.Warn($"[ForgotPasswordForm] OTP no enviado: cuenta inexistente o inactiva. Mail='{mail}'");
                    MessageBox.Show(
                        T("err_recuperar_cuenta_no_encontrada", "No se encontró una cuenta activa con ese correo."),
                        T("cap_no_encontrado", "No encontrado"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoggerLogic.Info($"[ForgotPasswordForm] OTP de recuperación enviado. Mail='{mail}'");
                _mailConfirmado = mail;
                _lblInfoMail.Text = string.Format(
                    T("msg_otp_enviado_fmt", "Código enviado a {0}. Válido por 15 minutos."),
                    mail);

                ClientSize = new Size(420, 300);
                AcceptButton = _btnCambiar;
                _panelStep1.Visible = false;
                _panelStep2.Visible = true;
                _txtOtp.Focus();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ForgotPasswordForm] Falla al enviar OTP. Mail='{mail}'", ex);
                MessageBox.Show(
                    string.Format(T("err_otp_envio_fmt", "Error al enviar el código:\n{0}"), ex.Message),
                    T("cap_error", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnEnviar.Enabled = true;
                _btnEnviar.Text = T("btn_enviar_codigo", "Enviar código");
                Cursor = Cursors.Default;
            }
        }

        private void BtnCambiar_Click(object sender, EventArgs e)
        {
            var otp = _txtOtp.Text.Trim();
            var newPass = _txtNewPass.Text;
            var confirmPass = _txtConfirmPass.Text;

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                LoggerLogic.Warn($"[ForgotPasswordForm] Validación: faltan campos al cambiar contraseña. Mail='{_mailConfirmado}'");
                MessageBox.Show(
                    T("val_campos_completos", "Complete todos los campos."),
                    T("cap_campos_requeridos", "Campos requeridos"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                LoggerLogic.Warn($"[ForgotPasswordForm] Validación: contraseñas no coinciden. Mail='{_mailConfirmado}'");
                MessageBox.Show(
                    T("val_pass_no_coinciden", "Las contraseñas no coinciden."),
                    T("cap_error", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConfirmPass.Clear();
                _txtConfirmPass.Focus();
                return;
            }

            if (newPass.Length < 6)
            {
                LoggerLogic.Warn($"[ForgotPasswordForm] Validación: contraseña muy corta. Mail='{_mailConfirmado}'");
                MessageBox.Show(
                    T("val_pass_muy_corta", "La contraseña debe tener al menos 6 caracteres."),
                    T("cap_pass_muy_corta", "Contraseña muy corta"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_otpService.ValidarOtp(_mailConfirmado, otp))
            {
                LoggerLogic.Warn($"[ForgotPasswordForm] OTP inválido o expirado. Mail='{_mailConfirmado}'");
                MessageBox.Show(
                    T("err_otp_invalido", "El código es incorrecto o ha expirado."),
                    T("cap_otp_invalido", "Código inválido"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _txtOtp.Clear();
                _txtOtp.Focus();
                return;
            }

            try
            {
                _otpService.CambiarContraseña(_mailConfirmado, newPass);
                LoggerLogic.Info($"[ForgotPasswordForm] Contraseña restablecida vía OTP. Mail='{_mailConfirmado}'");
                MessageBox.Show(
                    T("msg_pass_cambiada", "Contraseña cambiada exitosamente. Ya puede iniciar sesión."),
                    T("cap_listo", "Listo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                LoggerLogic.Error($"[ForgotPasswordForm] Falla al cambiar contraseña. Mail='{_mailConfirmado}'", ex);
                MessageBox.Show(
                    string.Format(T("err_pass_cambio_fmt", "Error al cambiar la contraseña:\n{0}"), ex.Message),
                    T("cap_error", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
