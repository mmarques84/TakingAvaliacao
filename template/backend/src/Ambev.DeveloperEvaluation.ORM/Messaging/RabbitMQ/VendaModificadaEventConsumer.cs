using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ambev.DeveloperEvaluation.ORM.Messaging.RabbitMQ
{
    public class VendaModificadaEventConsumer : IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IEnvioEmailService _emailService;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _queueName = "venda_modificada_eventos_queue";

        public VendaModificadaEventConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            // Configuração da conexão com RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",  // Nome de usuário padrão
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara a fila (caso não exista)
            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var vendaModificadaEvent = JsonConvert.DeserializeObject<VendaModificadaEvent>(json);

                // Aqui criamos o escopo manualmente para resolver dependências `Scoped`
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _emailService = scope.ServiceProvider.GetRequiredService<IEnvioEmailService>();
                    await SendEmailForVendaModificadaEvent(vendaModificadaEvent);
                }
            };

            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private async Task SendEmailForVendaModificadaEvent(VendaModificadaEvent vendaModificadaEvent)
        {
            try
            {
                // Envia o e-mail para o cliente
                await _emailService.SendEmailAsync(
                    vendaModificadaEvent.CustomerEmail,
                    "Venda Modificada",
                    $"Sua venda foi modificada com sucesso! ID da venda: {vendaModificadaEvent.SaleId}, Novo Total: {vendaModificadaEvent.TotalAmount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }

        public void StopConsuming()
        {
            _channel.Close();
            _connection.Close();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }

}
