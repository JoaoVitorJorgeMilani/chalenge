using Main.Settings.Database;
using MongoDB.Driver.Linq;
using Polly;
using StackExchange.Redis;

namespace Main.App.Redis
{
    public class RedisService
    {
        private readonly RedisConnectionService _redisConnectionService;
        // private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisService> _logger;
        private readonly IDatabase _database;
        private const string ConnectedUsersHashKey = "connectedUsers";

        public RedisService(ILogger<RedisService> logger, RedisConnectionService redisConnectionService)
        {
            _redisConnectionService = redisConnectionService;
            // _redis = redisConnectionService.GetConnection();
            _database = redisConnectionService.GetDatabase();
            _logger = logger;
        }

        public void AddUser(string userId, string connectionId)
        {
            _logger.LogInformation(" AddUser() | UserId: {UserId} | ConnectionId: {ConnectionId}", userId, connectionId);
            _database.HashSet(ConnectedUsersHashKey, connectionId, userId);
        }

        public void RemoveUser(string connectionId)
        {
            _logger.LogInformation(" RemoveUser() | ConnectionId: {ConnectionId}", connectionId);
            _database.HashDelete(ConnectedUsersHashKey, connectionId);
        }

        public string? GetConnectionId(string userId)
        {
            _logger.LogInformation(" GetConnectionId() | UserId: {UserId}", userId);
            return _database.HashKeys(ConnectedUsersHashKey)
                .ToStringArray()
                .FirstOrDefault(key =>
                    _database.HashGet(ConnectedUsersHashKey, key).ToString() == userId
                );
        }

        public IEnumerable<string> GetAllConnectionIds()
        {

            if(_database == null || _database.HashKeys(ConnectedUsersHashKey) == null)
                return Enumerable.Empty<string>();
    
            return _database.HashKeys(ConnectedUsersHashKey).ToStringArray()
                .Select(connectionIdKey => _database.HashGet(ConnectedUsersHashKey, connectionIdKey).ToString());
        }

        public IEnumerable<string> GetAllConnectedUsers()
        {
            return _database.HashValues(ConnectedUsersHashKey)
                .ToStringArray()
                .Where(userId => !string.IsNullOrEmpty(userId))
                .Select(userId => userId!);
        }

        public void Dispose()
        {
            _logger.LogInformation(" Disposing");
            _database.Execute("FLUSHDB");
        }
    }
}