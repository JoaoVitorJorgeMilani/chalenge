using Main.App.Domain.Order;
using Main.App.Messaging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Main.App.Consumer
{
    public class OrderConsumer
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IModel _channel;
        private readonly IMessagingService _messagingService;
        private readonly IOrderService _orderService;

        public OrderConsumer(IMessagingService messagingService, IOrderService orderService, ILogger<OrderConsumer> logger)
        {
            _logger = logger;
            _orderService = orderService;
            _messagingService = messagingService;
            _channel = _messagingService.CreateChannel();
            BuildQueue();
        }

        private void BuildQueue()
        {
            _logger.LogInformation(" BuildQueue() | Building queue for Order");
            var queueConfig = _messagingService.GetQueueSettingsByFeatureName("Order");

            _channel.QueueDeclare(
                queue: queueConfig.QueueName,
                durable: queueConfig.QueueDurable,
                exclusive: queueConfig.QueueExclusive,
                autoDelete: queueConfig.QueueAutoDelete,
                arguments: null
            );
        }

        public void StartListening(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(" OrderConsumer start listening");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = System.Text.Encoding.UTF8.GetString(body.ToArray());

                    _logger.LogInformation(" Received: {Message}", message);
                    await _orderService.SendNotificationForElegibleUsers(message);
                };

                _channel.BasicConsume(queue: _messagingService.GetQueueSettingsByFeatureName("Order").QueueName, autoAck: true, consumer: consumer);

                cancellationToken.Register(() =>
                {
                    _logger.LogInformation(" OrderConsumer cancelation requested. Closing channel.");
                    _messagingService.CloseChannel(_channel);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while listening for messages.");
            }
        }

        public Task StopListening()
        {
            _logger.LogInformation(" OrderConsumer stop listening");
            _messagingService.CloseChannel(_channel);
            return Task.CompletedTask;
        }
    }
}