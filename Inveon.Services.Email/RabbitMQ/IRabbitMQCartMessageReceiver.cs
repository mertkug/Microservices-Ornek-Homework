using Inveon.MessageBus;
using Inveon.Services.Email.Messages;

namespace Inveon.Services.Email.RabbitMQ;

public interface IRabbitMQCartMessageReceiver
{
    Task<CheckoutHeaderDto?> ReceiveMessage(String queueName);
}
