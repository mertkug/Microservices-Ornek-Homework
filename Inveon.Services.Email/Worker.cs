using Inveon.MessageBus;
using Inveon.Services.Email.Messages;
using Inveon.Services.Email.RabbitMQ;
using RabbitMQ.Client;

namespace Inveon.Services.Email;

public class Worker : BackgroundService
{
    protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiver = new RabbitMQCartMessageReceiver();
        var checkoutHeaderDto = await receiver.ReceiveMessage("checkoutqueue");

        if (checkoutHeaderDto != null) MailClient.SendOrderConfirmationEmail(checkoutHeaderDto);
        return Task.CompletedTask;
    }
}