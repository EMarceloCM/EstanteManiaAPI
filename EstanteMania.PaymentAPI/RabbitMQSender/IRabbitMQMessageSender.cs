using EstanteMania.MessageBus;

namespace EstanteMania.PaymentAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage message);
    }
}
