using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

public class VendaCriadaEventConsumer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName = "venda_eventos_queue";
    private readonly ILogger<VendaCriadaEventConsumer> _logger;

    public VendaCriadaEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<VendaCriadaEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;  // Injeta o logger

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

    // Exemplo de logging ao verificar mensagens na fila
    public void CheckForMessages()
    {
        var result = _channel.BasicGet(_queueName, autoAck: true);  // Recupera uma única mensagem

        if (result != null)
        {
            // Se encontrou uma mensagem, registre no log
            var body = result.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Mensagem encontrada na fila: {json}");
        }
        else
        {
            // Caso não tenha mensagem
            _logger.LogInformation("Não há mensagens na fila.");
        }
    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Mensagem recebida: {json}");

                var vendaCriadaEvent = JsonConvert.DeserializeObject<VendaCriadaEvent>(json);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEnvioEmailService>();
                    await SendEmailForVendaCriadaEvent(vendaCriadaEvent, emailService);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);
    }

    private async Task SendEmailForVendaCriadaEvent(VendaCriadaEvent vendaCriadaEvent, IEnvioEmailService emailService)
    {
        try
        {
            if (vendaCriadaEvent != null)
            {
                await emailService.SendEmailAsync(
                    vendaCriadaEvent.CustomerEmail,
                    "Venda Criada com Sucesso",
                    $"Sua venda foi criada com sucesso! ID da venda: {vendaCriadaEvent.SaleId}, Total: {vendaCriadaEvent.TotalAmount}");
            }
            else
            {
                _logger.LogError($"Erro vendaCriadaEvent null: {vendaCriadaEvent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar e-mail: {ex.Message}");
        }
    }

    public void StopConsuming()
    {
        _channel.Close();
        _connection.Close();
    }
}

