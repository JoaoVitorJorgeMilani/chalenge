
namespace Main.App.Consumer
{
    public class HostedService : BackgroundService
    {
        private readonly OrderConsumer _consumer;
        private readonly ILogger<HostedService> _logger;

        public HostedService(OrderConsumer consumer, ILogger<HostedService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(" Starting consumer BackgroundService.");

            try
            {
                _consumer.StartListening(cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, cancellationToken);
                }

                await StopAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while starting the consumer.");
                await StopAsync(cancellationToken);
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(" Stopping consumer BackgroundService.");
            await _consumer.StopListening();
            await base.StopAsync(cancellationToken);
        }
    }
}