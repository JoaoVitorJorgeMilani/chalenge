using Main.App.Domain.User;
using Main.App.Settings.Messaging;
using Main.App.SignalR;
using MongoDB.Bson;

namespace Main.App.Domain.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMessagingService _messagingService;
        private readonly HubService _hubService;
        private readonly IUserService _userService;


        public OrderService(IOrderRepository repository, IMessagingService messagingService, HubService hubService, IUserService userService)
        {
            _repository = repository;
            _messagingService = messagingService;
            _hubService = hubService;
            _userService = userService;
        }

        public bool Add(Order order)
        {
            Console.WriteLine("TESTANDO MESSAGING");
            _messagingService.SendMessage(order.Id.ToString(), "order-queue");

            return _repository.Add(order);
        }

        public List<OrderDto> Get(OrderFilterModel filter)
        {
            return _repository.Get(filter);

        }

        public void SendNotificationForElegibleUsers(string orderId)
        {
            List<ObjectId> userIds = new List<ObjectId>();

            // Get all users who are connected to the hub
            _hubService.GetAllConnectedUsers().ToList().ForEach(id => userIds.Add(ObjectId.Parse(id)));

            // Verify if the users are available and send notification
            _userService.GetUsersById(userIds).ForEach(async user => {
                if(user.Status == UserStatus.Available)
                {
                    await _hubService.SendNotificationAsync(user.Id, orderId);
                }
            });
        }
    }
}