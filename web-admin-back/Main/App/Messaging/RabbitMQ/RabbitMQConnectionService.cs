using Polly;
using RabbitMQ.Client;

namespace Main.App.Messaging
{
    public class RabbitMQConnectionService
    {
        private readonly ILogger<RabbitMQConnectionService> _logger;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection? _connection;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        public RabbitMQConnectionService(IConfiguration configuration, ILogger<RabbitMQConnectionService> logger)
        {
            _logger = logger;
            _rabbitMQSettings = configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>()
            ?? throw new InvalidOperationException("As configurações do RabbitMQ não foram carregadas corretamente.");

            _connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.Hostname,
                Port = _rabbitMQSettings.Port,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password
            };
        }

        private IConnection CreateConnection()
        {
            _logger.LogInformation("CreateConnection() | Creating connection");

            var retryPolicy = Policy
                .Handle<Exception>() 
                .WaitAndRetryForever(
                    _ => TimeSpan.FromMilliseconds(3000),
                    onRetry: (exception, span) =>
                    {
                        _logger.LogError($"Failed to connect to RabbitMQ. Retrying in {span.TotalSeconds} seconds...");
                        _logger.LogError(exception.Message);

                    });
            
            _connection = retryPolicy.Execute(() =>
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                return _connectionFactory.CreateConnection();
            });

            return _connection;
        }


        public IConnection GetConnection()
        {
            return _connection ?? CreateConnection();
        }

        public void Dispose()
        {
            _logger.LogInformation(" Disposing");
            cancellationTokenSource.Cancel();
            
            if(_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }               
        }

    }
}