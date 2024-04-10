namespace Main.App.Settings.Messaging
{
    public interface IMessagingService 
    {
        public void SendMessage(string message, string queueName);
        
    }
}