using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;

namespace Task1.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string email, string subject, string msg)
        { // Plug in your email service here to send an email. 

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("photoportfoliosite231@gmail.com", "photoportfoliosite231@gmail.com"));
            message.To.Add(new MailboxAddress("Anuraj", email));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = msg;
            message.Body = bodyBuilder.ToMessageBody();

            await Task.FromResult(0);
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("photoportfoliosite231@gmail.com", "1qa2ws3ed4rf5tg");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
