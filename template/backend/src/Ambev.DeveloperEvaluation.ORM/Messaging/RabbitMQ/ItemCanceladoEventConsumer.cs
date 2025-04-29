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
    public class ItemCanceladoEventConsumer : IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IEnvioEmailService _emailService;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _queueName = "item_cancelado_eventos_queue";

        public ItemCanceladoEventConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;


            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
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
                var itemCanceladoEvent = JsonConvert.DeserializeObject<ItemCanceladoEvent>(json);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _emailService = scope.ServiceProvider.GetRequiredService<IEnvioEmailService>();
                    await SendEmailForItemCanceladoEvent(itemCanceladoEvent);
                }
            };

            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private async Task SendEmailForItemCanceladoEvent(ItemCanceladoEvent itemCanceladoEvent)
        {
            try
            {
                // Envia o e-mail para o cliente
                await _emailService.SendEmailAsync(
                    itemCanceladoEvent.CustomerEmail,
                    "Item Cancelado",
                    $"O item foi cancelado na sua venda. ID da venda: {itemCanceladoEvent.SaleId}, Item ID: {itemCanceladoEvent.ItemId}");
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
