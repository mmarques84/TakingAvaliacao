using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;


namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class EnvioEmailService : IEnvioEmailService
    {
        private readonly ILogger<EnvioEmailService> _logger;
        public EnvioEmailService(ILogger<EnvioEmailService> logger)
        {
            _logger = logger;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                _logger.LogInformation("E-mail SendEmailAsync iniciar!");
                var client = new SendGridClient("sua_chave_api_sendgrid");
                var from = new EmailAddress("seuemail@dominio.com", "Seu Nome");       
                var EmailAddressto = new EmailAddress(to);
                var plainTextContent = "Aqui está o corpo do e-mail";
                var htmlContent = "<strong>Aqui está o corpo do e-mail</strong>";
                var msg = MailHelper.CreateSingleEmail(from, EmailAddressto, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation("E-mail enviado com sucesso!");
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
