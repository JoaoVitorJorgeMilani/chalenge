using System.Text;
using global::RabbitMQ.Client;

namespace Main.App.Settings.Messaging.RabbitMQ
{
    public class RabbitMQService : IMessagingService
    {
        private RabbitMQSettings _rabbitMQSettings;
        private ConnectionFactory _connectionFactory;
        private readonly List<IModel> _channels = new List<IModel>();


        public RabbitMQService(IConfiguration configuration)
        {
            _rabbitMQSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>()
            ?? throw new InvalidOperationException("As configurações do RabbitMQ não foram carregadas corretamente.");

            _connectionFactory = _connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.Hostname,
                Port = _rabbitMQSettings.Port,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password
            };
        }

        public void SendMessage(string message, string queueName)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                Console.WriteLine($"Mensagem enviada para a fila '{queueName}': {message}");
            }
        }


        public IModel CreateChannel()
        {
            var channel = _connectionFactory.CreateConnection().CreateModel();
            _channels.Add(channel);
            return channel;
        }

        public QueueSettings GetQueueSettingsByFeatureName(string featureName)
        {
            return _rabbitMQSettings.GetQueueSettingsByFeatureName(featureName);
        }

        public void CloseChannel(IModel channel)
        {
            if (_channels.Contains(channel))
            {
                channel.Close();
                _channels.Remove(channel);
            }
        }

        public void Dispose()
        {
            foreach (var channel in _channels)
            {
                channel.Close();
            }
        }
    }
}

