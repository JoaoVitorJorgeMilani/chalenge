using System.ComponentModel.DataAnnotations;
using Main.App.Redis;
using Main.Utils;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace Main.App.SignalR
{
    public class HubService
    {
        private readonly ILogger<HubService> logger;
        private readonly RedisService _redisService;
        private readonly IEncryptor _encryptor;

        private IHubContext<MainHub> _hubContext;

        public HubService(RedisService redisService, IEncryptor encryptor, IHubContext<MainHub> hubContext, ILogger<HubService> logger)
        {
            _redisService = redisService;
            _encryptor = encryptor;
            _hubContext = hubContext;
            this.logger = logger;
        }

        public void AddConnection(string connectionId, string encryptedUserId)
        {
            logger.LogInformation("AddConnection() | ConnectionId: {ConnectionId} | EncryptedUserId: {EncryptedUserId}", connectionId, encryptedUserId);

            ObjectId userId = _encryptor.DecryptObjectId(encryptedUserId);

            if (userId == ObjectId.Empty)
            {
                throw new ValidationException("Invalid UserId");
            }



            _redisService.AddUser(userId.ToString(), connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            logger.LogInformation("RemoveConnection() | ConnectionId: {ConnectionId}", connectionId);
            _redisService.RemoveUser(connectionId);
        }

        public Task SendMessageForAllUsers(string message)
        {
            logger.LogInformation("SendMessageForAllUsers() | Message: {Message}", message);
            return _hubContext.Clients.All.SendAsync("Streaming", message);
        }

        public IEnumerable<string> GetAllConnectedUsers()
        {
            return _redisService.GetAllConnectedUsers();
        }

        public async Task SendNotificationAsync(ObjectId userId, string message)
        {
            logger.LogInformation("SendNotificationAsync() | UserId: {UserId} | Message: {Message}", userId, message);

            string connectionId = _redisService.GetConnectionId(userId.ToString())!;

            logger.LogInformation("SendNotificationAsync() | Retrivied ConnectionId: {ConnectionId}", connectionId);

            if (!string.IsNullOrEmpty(connectionId))
            {
                await SendToClient(connectionId, message);
            }
        }

        public async Task SendNotificationAsync(ObjectId userId, object message)
        {
            logger.LogInformation("SendNotificationAsync() | UserId: {UserId} | Message: {Message}", userId, message);

            string connectionId = _redisService.GetConnectionId(userId.ToString())!;

            if (string.IsNullOrEmpty(connectionId))
            {
                logger.LogError("SendNotificationAsync() | ConnectionId not found");
            }

            logger.LogInformation("SendNotificationAsync() | Retrivied ConnectionId: {ConnectionId}", connectionId);

            try
            {
                string strMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message);

                if (string.IsNullOrEmpty(strMessage))
                {
                    logger.LogError("SendNotificationAsync() | It was not possible to serialize message: {Message}", strMessage);
                }
                await SendToClient(connectionId, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while sending a message to the client | ConnectionId: {ConnectionId} | Message: {Message}", connectionId, message);
            }
        }

        private async Task SendToClient(string connectionId, string message)
        {
            logger.LogInformation("SendToClient() | ConnectionId: {ConnectionId} | Message: {Message}", connectionId, message);
            await _hubContext.Clients.Client(connectionId).SendAsync("Streaming", message);
        }

        private async Task SendToClient(string connectionId, object message)
        {
            logger.LogInformation("SendToClient() | ConnectionId: {ConnectionId} ", connectionId);
            await _hubContext.Clients.Client(connectionId).SendAsync("Streaming", message);
        }

        public async Task Dispose()
        {
            logger.LogInformation("Dispose() | Disconnecting all connections");
            foreach (var connectionId in _redisService.GetAllConnectionIds())
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ForceDisconnect");
            }
        }
    }
}