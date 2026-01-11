using Application.Abstraction.ExternalService;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
       
        private readonly MailSettings _settings;

        public EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Configuramos el cliente según lo que te dio Mailtrap
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            // Creamos el mensaje
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Importante para que el link de Angular funcione
            };

            mailMessage.To.Add(to);

            // Envío asíncrono
            await client.SendMailAsync(mailMessage);
        }
    }
}
