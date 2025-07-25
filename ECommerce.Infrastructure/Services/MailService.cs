using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services
{   
    // Mail servisi ile bagli isler 
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async  Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendEmailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(_configuration["Mail:Username"], "ECommerse",System.Text.Encoding.UTF8);
            foreach (var to in tos)
            {
                mail.To.Add(new MailAddress(to));
            }
            SmtpClient smtp = new(_configuration["Mail:Host"], Convert.ToInt32(_configuration["Mail:Port"]));
            smtp.Credentials = new System.Net.NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
        }

        public Task SendPasswordResetEmailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Salam <br> Yeni sifre teleb edirsinizse asagidaki linkden sifrenizi yenileyin...<br> <strong> <a target =\"_blank\" href=\"............/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni sifre ucun daxil olun...</a></strong><br><br><br><span style =\"font-size:12px; \"> ");

            SendEmailAsync(to, "Sifrenizi yenileyin", mail.ToString() );
            return null;
        }
    }
}
