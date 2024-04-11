using System.Text;
using global::RabbitMQ.Client;
using Newtonsoft.Json;

namespace Main.App.Messaging
{
    public class RabbitMQService : IMessagingService
    {
        private readonly ILogger<RabbitMQService> _logger;
        private RabbitMQSettings _rabbitMQSettings;
        // private ConnectionFactory _connectionFactory;
        private readonly List<IModel> _channels = new List<IModel>();
        private readonly RabbitMQConnectionService _rabbitMQConnectionService;


        public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger, RabbitMQConnectionService rabbitMQConnectionService)
        {
            _logger = logger;
            _rabbitMQConnectionService = rabbitMQConnectionService;
            
            _rabbitMQSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>()
            ?? throw new InvalidOperationException("As configurações do RabbitMQ não foram carregadas corretamente.");
            
        }

        public void SendMessage(string message, string queueName)
        {
            _logger.LogInformation(" SendMessage() | QueueName: {QueueName} | Message: {Message}", queueName, message);

            try
            {
                using (var connection = _rabbitMQConnectionService.GetConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending a message.");
            }
        }


        public IModel CreateChannel()
        {
            _logger.LogInformation(" CreateChannel() | Creating channel");
            var channel = _rabbitMQConnectionService.GetConnection().CreateModel();
            _channels.Add(channel);
            return channel;
        }

        public QueueSettings GetQueueSettingsByFeatureName(string featureName)
        {
            _logger.LogInformation(" GetQueueSettingsByFeatureName() | FeatureName: {FeatureName}", featureName);
            return _rabbitMQSettings.GetQueueSettingsByFeatureName(featureName);
        }

        public void CloseChannel(IModel channel)
        {
            _logger.LogInformation(" CloseChannel() | Closing channel");
            if (_channels.Contains(channel))
            {
                channel.Close();
                _channels.Remove(channel);
            }
        }

        public void Dispose()
        {
            _logger.LogInformation(" Dispose() | Disposing");
            foreach (var channel in _channels)
            {
                channel.Close();
            }
        }
    }
}

