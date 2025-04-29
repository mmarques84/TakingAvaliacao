using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Ambev.DeveloperEvaluation.ORM.Messaging.RabbitMQ
{
    public class VendaEventsPublisher
    {
        private readonly string _hostname = "localhost";
        private readonly string _queueName = "venda_eventos_queue"; // Nome da fila

        private IConnection _connection;
        private IModel _channel;

        public VendaEventsPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar a fila
            _channel.QueueDeclare(queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        }

        public void PublishVendaCriadaEvent(VendaCriadaEvent vendaCriadaEvent)
        {
            PublishEvent("VendaCriada", vendaCriadaEvent);
        }

        public void PublishVendaModificadaEvent(VendaModificadaEvent vendaModificadaEvent)
        {
            PublishEvent("VendaModificada", vendaModificadaEvent);
        }

        public void PublishVendaCanceladaEvent(VendaCanceladaEvent vendaCanceladaEvent)
        {
            PublishEvent("VendaCancelada", vendaCanceladaEvent);
        }

        public void PublishItemCanceladoEvent(ItemCanceladoEvent itemCanceladoEvent)
        {
            PublishEvent("ItemCancelado", itemCanceladoEvent);
        }

        private void PublishEvent(string eventType, object eventMessage)
        {
            var message = JsonConvert.SerializeObject(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.ConfirmSelect();
            _channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] Evento {eventType} publicado: {message}");

            bool confirmed = _channel.WaitForConfirms(); // Espera confirmação do RabbitMQ

            if (confirmed)
                Console.WriteLine($"[✔] Evento {eventType} confirmado e publicado: {message}");
            else
                Console.WriteLine($"[✖] Falha ao publicar evento {eventType}");
        }

        // Garantir que a conexão e o canal sejam fechados corretamente quando a aplicação terminar
        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
