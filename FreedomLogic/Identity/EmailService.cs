using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            using (var smtp = new SmtpClient())
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(new MailAddress(message.Destination));
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.Body;
                mailMessage.IsBodyHtml = true;
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}
