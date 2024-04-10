using Microsoft.AspNetCore.SignalR;

namespace Main.App.SignalR
{
    public class MainHub : Hub
    {
        private readonly HubService _hubService;

        public MainHub(HubService hubService)
        {
            _hubService = hubService;
        }

        public async Task Streaming(string testestr)
        {
            await Clients.All.SendAsync("Streaming", testestr);
        }

        

        public override async Task OnConnectedAsync()
        {
            var encryptedUserId = Context.GetHttpContext()?.Request?.Query["encryptedUserId"].ToString();

            if(string.IsNullOrEmpty(encryptedUserId))
            {
                 throw new InvalidOperationException("EncryptedUserId not found on request");
            }

            _hubService.AddConnection(Context.ConnectionId.ToString(), encryptedUserId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _hubService.RemoveConnection(Context.ConnectionId.ToString());
            await base.OnDisconnectedAsync(exception);
        }
    }
}