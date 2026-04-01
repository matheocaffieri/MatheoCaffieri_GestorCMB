using BL.LoginBL;
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

        private void BuildUI()
        {
            Text = "Recuperar contraseña";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 190);

            // ── STEP 1 ──────────────────────────────────────────────────────
            _panelStep1 = new Panel { Location = Point.Empty, Size = ClientSize };

            var lblTitle = new Label
            {
                Text = "Ingrese su correo electrónico",
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
                Text = "Enviar código",
                Location = new Point(255, 100),
                Width = 140,
                TabIndex = 1
            };
            _btnEnviar.Click += BtnEnviar_Click;

            // Allow pressing Enter
            AcceptButton = _btnEnviar;

            _panelStep1.Controls.AddRange(new Control[] { lblTitle, _txtMail, _btnEnviar });
            Controls.Add(_panelStep1);

            // ── STEP 2 ──────────────────────────────────────────────────────
            _panelStep2 = new Panel { Location = Point.Empty, Size = new Size(420, 340), Visible = false };

            _lblInfoMail = new Label
            {
                Text = "",
                AutoSize = false,
                Width = 380,
                Height = 32,
                Location = new Point(20, 15)
            };

            var lblOtp = new Label { Text = "Código de verificación:", AutoSize = true, Location = new Point(20, 58) };
            _txtOtp = new TextBox { Location = new Point(20, 78), Width = 120, MaxLength = 6, TabIndex = 0 };

            var lblNewPass = new Label { Text = "Nueva contraseña:", AutoSize = true, Location = new Point(20, 118) };
            _txtNewPass = new TextBox { Location = new Point(20, 138), Width = 375, PasswordChar = '●', TabIndex = 1 };

            var lblConfirm = new Label { Text = "Confirmar contraseña:", AutoSize = true, Location = new Point(20, 178) };
            _txtConfirmPass = new TextBox { Location = new Point(20, 198), Width = 375, PasswordChar = '●', TabIndex = 2 };

            _btnCambiar = new Button
            {
                Text = "Cambiar contraseña",
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
                MessageBox.Show("Ingrese su correo electrónico.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _btnEnviar.Enabled = false;
            _btnEnviar.Text = "Enviando...";
            Cursor = Cursors.WaitCursor;

            try
            {
                bool enviado = _otpService.EnviarOtp(mail);
                if (!enviado)
                {
                    MessageBox.Show("No se encontró una cuenta activa con ese correo.", "No encontrado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _mailConfirmado = mail;
                _lblInfoMail.Text = $"Código enviado a {mail}. Válido por 15 minutos.";

                ClientSize = new Size(420, 300);
                AcceptButton = _btnCambiar;
                _panelStep1.Visible = false;
                _panelStep2.Visible = true;
                _txtOtp.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el código:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnEnviar.Enabled = true;
                _btnEnviar.Text = "Enviar código";
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
                MessageBox.Show("Complete todos los campos.", "Campos requeridos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConfirmPass.Clear();
                _txtConfirmPass.Focus();
                return;
            }

            if (newPass.Length < 6)
            {
                MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Contraseña muy corta",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_otpService.ValidarOtp(_mailConfirmado, otp))
            {
                MessageBox.Show("El código es incorrecto o ha expirado.", "Código inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _txtOtp.Clear();
                _txtOtp.Focus();
                return;
            }

            try
            {
                _otpService.CambiarContraseña(_mailConfirmado, newPass);
                MessageBox.Show("Contraseña cambiada exitosamente. Ya puede iniciar sesión.", "Listo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar la contraseña:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
