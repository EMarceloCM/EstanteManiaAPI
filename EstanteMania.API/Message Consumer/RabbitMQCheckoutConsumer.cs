using EstanteMania.API.DTO_s;
using EstanteMania.API.Messages;
using EstanteMania.API.RabbitMQSender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.OrderAPI.MessageConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        //private readonly CartRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQCheckoutConsumer(/*CartRepository repository,*/
            IRabbitMQMessageSender rabbitMQMessageSender)
        {
            //_repository = repository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chanel, evt) =>
            {
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                CartHeaderDTO vo = JsonSerializer.Deserialize<CartHeaderDTO>(content);
                ProcessOrder(vo).GetAwaiter().GetResult();
                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task ProcessOrder(CartHeaderDTO vo)
        {
            CartHeaderDTO order = new()
            {
                UserId = vo.UserId,
                Name = vo.Name,
                CartDetails = new List<CarrinhoItemDTO>(),
                CardNumber = vo.CardNumber,
                CouponCode = vo.CouponCode,
                CVV = vo.CVV,
                DiscountAmount = vo.DiscountAmount,
                Email = vo.Email,
                ExpiryMonthYear = vo.ExpiryMonthYear,
                TotalAmount = vo.TotalAmount,
                Payment_Status = 0,
                Phone = vo.Phone,
                DateTime = vo.DateTime,
                cartId = vo.cartId,
                CartTotalItens = vo.CartTotalItens
            };

            foreach (var details in vo.CartDetails)
            {
                CarrinhoItemDTO detail = new()
                {
                    BookId = details.BookId,
                    BookName = details.BookName,
                    Price = details.Price,
                    Quantidade = details.Quantidade
                };
                order.CartTotalItens += 1;
                order.CartDetails.ToList().Add(detail);
            }

            // await _repository.AddItem(order);

            PaymentDTO payment = new()
            {
                Name = order.Name,
                CardNumber = order.CardNumber,
                CVV = order.CVV,
                ExpireMonthYear = order.ExpiryMonthYear,
                OrderId = order.cartId,
                PurchaseAmount = order.TotalAmount,
                Email = order.Email
            };
            try
            {
                _rabbitMQMessageSender.SendMessage(payment, "orderpaymentprocessqueue");
            }
            catch (Exception)
            {
                //Log
                throw;
            }
        }
    }
}
