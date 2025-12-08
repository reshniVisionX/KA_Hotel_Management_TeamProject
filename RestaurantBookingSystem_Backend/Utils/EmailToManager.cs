using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RestaurantBookingSystem.Utils
{
    public class EmailToManager
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailToManager> _logger;

        public EmailToManager(IConfiguration config, ILogger<EmailToManager> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string messageBody)
        {
            try
            {
                _logger.LogInformation("📧 Attempting to send email...");
                _logger.LogDebug("Parameters => To: {ToEmail}, Subject: {Subject}", toEmail, subject);

                // Load SMTP settings from appsettings.json
                var smtpHost = _config["EmailSettings:SmtpHost"];
                var smtpPortStr = _config["EmailSettings:SmtpPort"];
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var senderPassword = _config["EmailSettings:SenderPassword"];
                var enableSslStr = _config["EmailSettings:EnableSSL"];

                // Validate config values
                if (string.IsNullOrWhiteSpace(smtpHost) ||
                    string.IsNullOrWhiteSpace(smtpPortStr) ||
                    string.IsNullOrWhiteSpace(senderEmail) ||
                    string.IsNullOrWhiteSpace(senderPassword))
                {
                    _logger.LogError("❌ Email configuration missing or invalid. Please check appsettings.json.");
                    _logger.LogError("Values: Host={Host}, Port={Port}, Email={Email}, PasswordSet={HasPassword}",
                        smtpHost, smtpPortStr, senderEmail, !string.IsNullOrWhiteSpace(senderPassword));
                    return false;
                }

                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    _logger.LogError("❌ Recipient email is null or empty.");
                    return false;
                }

                // Parse config
                var smtpPort = int.TryParse(smtpPortStr, out var port) ? port : 587;
                var enableSsl = bool.TryParse(enableSslStr, out var ssl) ? ssl : true;

                // Create and send email
                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtp.EnableSsl = enableSsl;

                    var mail = new MailMessage
                    {
                        From = new MailAddress(senderEmail, "Restaurant Management System"),
                        Subject = subject,
                        Body = messageBody,
                        IsBodyHtml = true
                    };

                    mail.To.Add(toEmail);

                    _logger.LogInformation("📨 Sending email to {Recipient} via {Host}:{Port}", toEmail, smtpHost, smtpPort);
                    await smtp.SendMailAsync(mail);
                }

                _logger.LogInformation("✅ Email sent successfully to {Recipient}", toEmail);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "❌ SMTP error occurred while sending email to {Recipient}", toEmail);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Unexpected error occurred while sending email to {Recipient}", toEmail);
                return false;
            }
        }
    }
}
