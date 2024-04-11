using Main.Settings.Database;
using MongoDB.Driver.Linq;
using Polly;
using StackExchange.Redis;

namespace Main.App.Redis
{
    public class RedisService
    {
        private readonly RedisConnectionService _redisConnectionService;
        private readonly ILogger<RedisService> _logger;
        private IDatabase? _database;

        private IDatabase Database 
        { 
            get => _database ??= GetDatabase(); 
            set => _database = value; 
        }

        private const string ConnectedUsersHashKey = "connectedUsers";

        public RedisService(ILogger<RedisService> logger, RedisConnectionService redisConnectionService)
        {
            _redisConnectionService = redisConnectionService;
            _logger = logger;
            _database = redisConnectionService.GetDatabase();

        }

        public void AddUser(string userId, string connectionId)
        {
            _logger.LogInformation(" AddUser() | UserId: {UserId} | ConnectionId: {ConnectionId}", userId, connectionId);
            Database.HashSet(ConnectedUsersHashKey, connectionId, userId);
        }

        public void RemoveUser(string connectionId)
        {
            _logger.LogInformation(" RemoveUser() | ConnectionId: {ConnectionId}", connectionId);
            Database.HashDelete(ConnectedUsersHashKey, connectionId);
        }

        public string? GetConnectionId(string userId)
        {
            _logger.LogInformation(" GetConnectionId() | UserId: {UserId}", userId);
            return Database.HashKeys(ConnectedUsersHashKey)
                .ToStringArray()
                .FirstOrDefault(key =>
                    Database.HashGet(ConnectedUsersHashKey, key).ToString() == userId
                );
        }

        public IEnumerable<string> GetAllConnectionIds()
        {

            if(Database == null || Database.HashKeys(ConnectedUsersHashKey) == null)
                return Enumerable.Empty<string>();
    
            return Database.HashKeys(ConnectedUsersHashKey).ToStringArray()
                .Select(connectionIdKey => Database.HashGet(ConnectedUsersHashKey, connectionIdKey).ToString());
        }

        public IEnumerable<string> GetAllConnectedUsers()
        {
            return Database.HashValues(ConnectedUsersHashKey)
                .ToStringArray()
                .Where(userId => !string.IsNullOrEmpty(userId))
                .Select(userId => userId!);
        }

        public void Dispose()
        {
            _logger.LogInformation(" Disposing");
            Database.Execute("FLUSHDB");
        }

        private IDatabase GetDatabase()
        {
            try
            {
                if(_database != null)
                    return _database;

                _database = _redisConnectionService.GetDatabase();
                return _database;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while connecting to Redis.");
                throw;
            }
        }
    }
}