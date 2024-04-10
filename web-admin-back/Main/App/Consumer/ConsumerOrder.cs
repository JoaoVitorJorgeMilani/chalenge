using Main.App.Domain.Order;
using Main.App.Settings.Messaging.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Main.App.Consumer
{
    public class ConsumerOrder
    {
        private readonly IModel _channel;                
        private readonly RabbitMQService _rabbitService;
        private readonly IOrderService _orderService;

        public ConsumerOrder(RabbitMQService rabbitService, IOrderService orderService)
        {
            _orderService = orderService;
            _rabbitService = rabbitService;
            _channel = _rabbitService.CreateChannel();
            BuildQueue();
            StartListening();
        }

        private void BuildQueue()
        {
            var queueConfig = _rabbitService.GetQueueSettingsByFeatureName("Order");

            _channel.QueueDeclare(
                queue: queueConfig.QueueName,
                durable: queueConfig.QueueDurable,
                exclusive: queueConfig.QueueExclusive,
                autoDelete: queueConfig.QueueAutoDelete,
                arguments: null
            );
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = System.Text.Encoding.UTF8.GetString(body.ToArray());

                Console.WriteLine($"Mensagem recebida: {message}");

                _orderService.SendNotificationForElegibleUsers(message);
            };

            _channel.BasicConsume(queue: _rabbitService.GetQueueSettingsByFeatureName("Order").QueueName, autoAck: true, consumer: consumer);
        }

        public void Stop()
        {
            _rabbitService.CloseChannel(_channel);
        }
    }

}
