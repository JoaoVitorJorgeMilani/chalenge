using System.ComponentModel.DataAnnotations;
using Main.App.Redis;
using Main.Utils;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver.Core.Connections;

namespace Main.App.SignalR
{
    public class HubService
    {
        private readonly RedisService _redisService;
        private readonly IEncryptor _encryptor;

        private IHubContext<MainHub> _hubContext;
        
        public HubService(RedisService redisService, IEncryptor encryptor, IHubContext<MainHub> hubContext)
        {
            _redisService = redisService;
            _encryptor = encryptor;
            _hubContext = hubContext;
        }

        public void AddConnection(string connectionId, string encryptedUserId)
        {   
            ObjectId userId = _encryptor.DecryptObjectId(encryptedUserId);           

            if(userId == ObjectId.Empty)
            {
                throw new ValidationException("Invalid UserId");
            }
            
            _redisService.AddUser(userId.ToString(), connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            _redisService.RemoveUser(connectionId);
        }

        public Task SendMessageForAllUsers(string message)
        {
            return _hubContext.Clients.All.SendAsync("Streaming", message);
        }

        public IEnumerable<string> GetAllConnectedUsers()
        {
            return _redisService.GetAllConnectedUsers();
        } 

        public async Task SendNotificationAsync(ObjectId userId, string message)
        {
            string connectionId = _redisService.GetConnectionId(userId.ToString())!;

            if(!string.IsNullOrEmpty(connectionId))
            {
               await SendToClient(connectionId, message);
            }
        }

        private async Task SendToClient(string connectionId, string message)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("OrderMessage", message);

        }
      
    }
}