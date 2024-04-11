using RabbitMQ.Client;

namespace Main.App.Messaging
{
    public interface IMessagingService
    {
        public void SendMessage(string message, string queueName);
        IModel CreateChannel();
        QueueSettings GetQueueSettingsByFeatureName(string featureName);
        void CloseChannel(IModel channel);
        void Dispose();

    }
}