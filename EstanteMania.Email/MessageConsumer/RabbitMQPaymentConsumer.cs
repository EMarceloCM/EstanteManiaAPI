using EstanteMania.Email.Messages;
using EstanteMania.Email.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Email.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";
        private string queueName = "";
        public RabbitMQPaymentConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, ExchangeName, "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                UpdatePaymentResultMessage message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
                ProcessLogs(message).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume(queueName, false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessLogs(UpdatePaymentResultMessage message)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                    await emailRepository.LogEmail(message);
                }
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
