using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Infrastructures.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtp_host = "mail.share.baysoft.com.br";
            var smtp_port = 587;

            var smtp_client_name = "postmaster@share.baysoft.com.br";
            var smtp_client_password = "_bahia147";
            var enableSSL = false;

            var project_noreply = "postmaster@share.baysoft.com.br";

            SmtpClient smtp = new SmtpClient(smtp_host, smtp_port);
            smtp.Credentials = new NetworkCredential(smtp_client_name, smtp_client_password);
            smtp.EnableSsl = enableSSL;

            var message = new MailMessage(new MailAddress(project_noreply), new MailAddress(email));
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            return Task.Factory.StartNew(() =>
            {
                smtp.Send(message);
            });
        }
    }
}
