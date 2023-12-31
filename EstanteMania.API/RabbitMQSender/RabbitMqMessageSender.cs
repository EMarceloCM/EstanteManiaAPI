﻿using EstanteMania.API.Messages;
using EstanteMania.MessageBus;
using RabbitMQ.Client;
using System.Text.Json;

namespace EstanteMania.API.RabbitMQSender
{
    public class RabbitMqMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMqMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void SendMessage(BaseMessage message, string queueName)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

                byte[] body = GetMessageAsByteArray(message);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (message.GetType() == typeof(CartHeaderDTO))
            {
                var json = JsonSerializer.Serialize<CartHeaderDTO>((CartHeaderDTO)message, options);
                return System.Text.Encoding.UTF8.GetBytes(json);
            }
            var paymentJson = JsonSerializer.Serialize<PaymentDTO>((PaymentDTO)message, options);
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
