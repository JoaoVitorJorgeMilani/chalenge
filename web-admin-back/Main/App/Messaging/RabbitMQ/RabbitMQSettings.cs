namespace Main.App.Messaging
{
    public class RabbitMQSettings
    {
        public string? Hostname { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public List<QueueSettings>? Queues { get; set; }

        public QueueSettings GetQueueSettingsByFeatureName(string featureName)
        {
            return Queues!.First(queue => queue.FeatureName == featureName);
        }
    }

    public class QueueSettings
    {
        public string? FeatureName { get; set; }
        public string? QueueName { get; set; }
        public bool QueueDurable { get; set; }
        public bool QueueExclusive { get; set; }
        public bool QueueAutoDelete { get; set; }

    }
}
