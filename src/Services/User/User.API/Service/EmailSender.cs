using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace User.API.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly MailConfig _mailConfig;

        public EmailSender(IOptions<MailConfig> mailConfig)
        {
            _mailConfig = mailConfig.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_mailConfig.Host, int.Parse(_mailConfig.Port!))
            {
                EnableSsl = true,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailConfig.Google_Email, _mailConfig.Google_Password)
            };
            try
            {
                await client.SendMailAsync(
                    new MailMessage(from: _mailConfig.Google_Email!,
                                    to: email,
                                    subject: subject,
                                    body: message
                                    ));
            }
            catch(Exception ex)
            {
                
            }
        }

    }
}
