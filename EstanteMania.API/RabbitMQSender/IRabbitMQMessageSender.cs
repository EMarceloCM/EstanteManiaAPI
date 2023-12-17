using EstanteMania.MessageBus;

namespace EstanteMania.API.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}
