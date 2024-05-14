using GraduationProject.Core.Models;
using GraduationProject.Core.ServiceInterfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace GraduationProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IConfiguration _configuration;

        public EmailService(EmailConfiguration emailConfig,IConfiguration configuration)
        {
            _emailConfig = emailConfig;
            _configuration = configuration;
        }
        public void SendEmail(Message message)
        {
            var EmailMessage = CreateEmailMessage(message);
            Send(EmailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var EmailMessage = new MimeMessage();
            EmailMessage.From.Add(new MailboxAddress("Email", _configuration["EmailConfiguration:From"]));
            EmailMessage.To.AddRange(message.To);
            EmailMessage.Subject = message.Subject;
            EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return EmailMessage;
        }
        private void Send(MimeMessage message)
        {
            using var Client = new SmtpClient();
            try
            {
                Client.Connect(_configuration["EmailConfiguration:SmtpServer"], int.Parse(_configuration["EmailConfiguration:Port"]), true);
                Client.AuthenticationMechanisms.Remove("XOAUTH2");
                Client.Authenticate(_configuration["EmailConfiguration:Username"], _configuration["EmailConfiguration:Password"]);
                Client.Send(message);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Client.Disconnect(true);
                Client.Dispose();
            }
        }
    }
}
