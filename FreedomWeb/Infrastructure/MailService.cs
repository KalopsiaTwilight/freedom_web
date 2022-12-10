using FreedomLogic.Infrastructure;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace FreedomWeb.Infrastructure
{
    public class MailService
    {
        public SmtpConfiguration _smtpConfig { get; set; }
        public MailService(AppConfiguration appConfiguration)
        {
            _smtpConfig = appConfiguration.Smtp;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse($"\"{_smtpConfig.DisplayName}\" <{_smtpConfig.FromAddress}>"),
                Subject = subject
            };
            email.To.Add(MailboxAddress.Parse(toAddress));
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpConfig.Host, _smtpConfig.Port);
            smtp.Authenticate(_smtpConfig.User, _smtpConfig.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
