using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            EmailSettings = emailSettings.Value;
            Logger = logger;
        }

        public EmailSettings EmailSettings { get; }
        public ILogger<EmailService> Logger { get; }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(EmailSettings.ApiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var body = email.Body;

            var from = new EmailAddress
            {
                Email = EmailSettings.FromAddress,
                Name = EmailSettings.FromName
            };

            var message = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(message);

            Logger.LogInformation("Email sent...");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

            Logger.LogInformation("Email sending failed...");
            return false;
        }
    }
}
