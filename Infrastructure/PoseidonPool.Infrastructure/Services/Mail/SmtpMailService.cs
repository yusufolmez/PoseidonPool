using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using PoseidonPool.Application.Abstractions.Services;

namespace PoseidonPool.Infrastructure.Services.Mail
{
    public class SmtpMailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public SmtpMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = _configuration.GetValue<int>("Smtp:Port");
            var smtpFrom = _configuration["Smtp:From"];
            var smtpFromName = _configuration["Smtp:FromName"];
            var smtpUsername = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];

            if (string.IsNullOrWhiteSpace(smtpHost) || 
                string.IsNullOrWhiteSpace(smtpFrom) || 
                string.IsNullOrWhiteSpace(smtpUsername) || 
                string.IsNullOrWhiteSpace(smtpPassword))
            {
                throw new InvalidOperationException("SMTP configuration is incomplete. Please check appsettings.json");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpFromName, smtpFrom));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            if (isHtml)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

