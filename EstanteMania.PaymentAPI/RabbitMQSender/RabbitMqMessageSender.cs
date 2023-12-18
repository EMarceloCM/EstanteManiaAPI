using EstanteMania.MessageBus;
using EstanteMania.PaymentAPI.Messages;
using RabbitMQ.Client;
using System.Text.Json;

namespace EstanteMania.PaymentAPI.RabbitMQSender
{
    public class RabbitMqMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        private const string ExchangeName = "FanoutPaymentUpdateExchange";

        public RabbitMqMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, durable: false);

                byte[] body = GetMessageAsByteArray(message);

                channel.BasicPublish(exchange: ExchangeName, routingKey: "", basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            var paymentJson = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);
            return System.Text.Encoding.UTF8.GetBytes(paymentJson);
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;

            CreateConnection();
            return _connection != null;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
