using aspnetcore_webapi.Models;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace aspnetcore_webapi.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;
        private SmtpClient _client;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new SmtpClient(_configuration["EmailConfiguration:SmtpServer"], Convert.ToInt32(_configuration["EmailConfiguration:Port"]));
            _client.Credentials = new NetworkCredential(_configuration["EmailConfiguration:Username"], _configuration["EmailConfiguration:Password"]);
            _client.EnableSsl = true;
        }

        public void EmailTwoFactorCode(ApplicationUser user)
        {
            string message = $"Hello {user.FirstName} {user.LastName},\n\nYour code is: {user.TwoFactorCode}";
            SendEmailAsync(user.Email, "Two Factor Code", message);
        }

        public Task SendEmailAsync(string recipient, string subject, string message)
        {
            using (MailMessage mailMessage = new MailMessage(_configuration["Email:From"], recipient, subject, message))
            {
                try
                {
                    _client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email: {0}", ex.ToString());
                }
            }
            return null;
        }
    }
}
