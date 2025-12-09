using System.Net;
using System.Net.Mail;

namespace RestaurantBookingSystem.Utils
{
    public class SendEmail
    {
        private readonly IConfiguration _configuration;
        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // --------------------- LOAD TEMPLATE ---------------------
        private string LoadEmailTemplate(string templateName, Dictionary<string, string> placeholders)
        {
            try
            {
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Email Template", templateName);
                var html = File.ReadAllText(templatePath);

                foreach (var pair in placeholders)
                {
                    html = html.Replace("{" + pair.Key + "}", pair.Value);
                }

                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailTemplate Error]: {ex.Message}");
                throw;
            }
        }

        // --------------------- SEND TEMPLATE EMAIL ---------------------
        public void SendTemplatedEmail(
      string receiverEmail,
      string subject,
      string templateName,
      Dictionary<string, string> placeholders)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSSL"]);

                var htmlBody = LoadEmailTemplate(templateName, placeholders);

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail, "InstantEats Food Ordering");
                    mail.To.Add(receiverEmail);
                    mail.Subject = subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = enableSsl;

                        try
                        {
                            smtp.Send(mail);
                            Console.WriteLine($"✅ Email sent successfully to {receiverEmail}");
                        }
                        catch (SmtpException smtpEx)
                        {
                            Console.WriteLine($"⚠️ SMTP Error while sending email: {smtpEx.Message}");
                        }
                        catch (System.Net.Sockets.SocketException sockEx)
                        {
                            Console.WriteLine($"⚠️ Network Error: No internet or host unreachable ({sockEx.Message})");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Unexpected email send error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception outerEx)
            {

                Console.WriteLine($"[Email Error - Configuration/Template]: {outerEx.Message}");

            }
        }

      
    }
}
