using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.IO;
using Carts.Helpers.Interface;

namespace Carts.Helpers
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }
        public ILogger _log { get; }

        public EmailSender(IConfiguration configuration, ILoggerFactory log)
        {
            Configuration = configuration;
            _log = log.CreateLogger<EmailSender>();
        }

        public Task SendEmailAsync(string email, string subject, string message, string attachment=null)
        {
            return string.IsNullOrEmpty(attachment)? Task.Run(()=>Execute(Configuration["SendGridKey"], subject, message, email)) 
                : Task.Run(() => Execute(Configuration["SendGridKey"], subject, message, email, attachment));
        }

        public Task Execute(string apiKey, string subject, string message, string email, string attachment = null)
        {
            var client = new SendGridClient(apiKey);
            
            var msg = new SendGridMessage()
            {
                //this could be any email
                From = new EmailAddress("test@carts.com", "Carts"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            if (!string.IsNullOrEmpty(attachment))
            {
                var title= attachment.Substring(attachment.LastIndexOf('\\') + 1);
                msg.AddAttachment(title, Convert.ToBase64String(File.ReadAllBytes(attachment)));
            }
            

            return client.SendEmailAsync(msg);
        }
    }
}

