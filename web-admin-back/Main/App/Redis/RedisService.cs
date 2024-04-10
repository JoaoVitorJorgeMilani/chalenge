using StackExchange.Redis;

namespace Main.App.Redis
{
    public class RedisService
    {
        private readonly IDatabase _database;
        private const string ConnectedUsersHashKey = "connectedUsers";

        public RedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public void AddUser(string userId, string connectionId)
        {
            _database.HashSet(ConnectedUsersHashKey, connectionId, userId);
        }

        public void RemoveUser(string connectionId)
        {
            _database.HashDelete(ConnectedUsersHashKey, connectionId);
        }

        public string? GetConnectionId(string userId)
        {
            return _database.HashGet(ConnectedUsersHashKey, userId);
        }

        public IEnumerable<string> GetAllConnectedUsers()
        {
            return _database.HashValues(ConnectedUsersHashKey)
                .ToStringArray()
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(userId => userId!); 
        }
    }
}
