using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Inveon.MessageBus;
using Inveon.Services.Email.Messages;

namespace Inveon.Services.Email.RabbitMQ
{
    public class RabbitMQCartMessageReceiver : IRabbitMQCartMessageReceiver
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCartMessageReceiver()
        {
            _hostname = "127.0.0.1";
            _password = "guest";
            _username = "guest";
            CreateConnection();
        }
        public async Task<CheckoutHeaderDto?> ReceiveMessage(string queueName)
        {
            TaskCompletionSource<CheckoutHeaderDto?> tcs = new();
            if (!ConnectionExists()) return null;
            _channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedMessage = JsonConvert.DeserializeObject<CheckoutHeaderDto>(message);
                if (receivedMessage != null) tcs.SetResult(receivedMessage);
            };
            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return await tcs.Task;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception)
            {
                // Log the exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null && _channel != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null && _channel != null;
        }
    }
}
