
using Main.Settings.Database;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace Main.App.Redis
{
    public class RedisConnectionService
    {
        private RetryPolicy? _retryPolicy;
        private readonly ILogger<RedisConnectionService> _logger;
        private IConnectionMultiplexer? _redis;
        private readonly RedisSettings _redisSettings;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public RedisConnectionService(ILogger<RedisConnectionService> logger, RedisSettings redisSettings)
        {
            _logger = logger;
            _redisSettings = redisSettings;
            _retryPolicy = Policy
                .Handle<RedisConnectionException>()
               .WaitAndRetryForever(
                    _ => TimeSpan.FromMilliseconds(3000),
                    onRetry: (exception, span) =>
                    {
                        _logger.LogError($"Failed to connect to Redis. Retrying in {span.TotalSeconds} seconds...");
                        _logger.LogError(exception.Message);

                    });

        }

        private IConnectionMultiplexer Connect()
        {
            CancellationToken token = cancellationTokenSource.Token;
            try
            {
                _redis = _retryPolicy!.Execute((ct) =>
                {
                    ct.ThrowIfCancellationRequested();
                    return ConnectionMultiplexer.Connect(_redisSettings!.ConnectionString! + ",allowAdmin=true");
                }, token);
                return _redis;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Connection attempt to Redis was canceled.");
                throw new Exception("Connection attempt to Redis was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while connecting to Redis.");
                throw;
            }


        }

        private IConnectionMultiplexer GetConnection()
        {
            if(_redis != null)
                return _redis;

            try{

               return Connect();

            } 
            catch (Exception)
            {
                throw;
            }
            

        }



        public void Dispose()
        {
            _logger.LogInformation(" Disposing");
            cancellationTokenSource.Cancel();
        }

        public IDatabase GetDatabase()
        {
            try
            {
                var connection = GetConnection();
                return connection.GetDatabase();
            } 
            catch (Exception)
            {
                throw;
            }
        }
    }
}