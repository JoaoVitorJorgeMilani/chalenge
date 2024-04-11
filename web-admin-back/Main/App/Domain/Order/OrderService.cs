

using System.ComponentModel.DataAnnotations;
using Main.App.Domain.User;
using Main.App.Messaging;
using Main.App.SignalR;
using Main.Utils;
using MongoDB.Bson;

namespace Main.App.Domain.Order
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _repository;
        private readonly IMessagingService _messagingService;
        private readonly HubService _hubService;
        private readonly IUserService _userService;

        private readonly IEncryptor _encryptor;


        public OrderService(IOrderRepository repository, IMessagingService messagingService, HubService hubService, IUserService userService, ILogger<OrderService> logger, IEncryptor encryptor)
        {
            _repository = repository;
            _messagingService = messagingService;
            _hubService = hubService;
            _userService = userService;
            _logger = logger;
            _encryptor = encryptor;
        }

        public bool Add(OrderEntity order)
        {
            _logger.LogInformation(" OrderService - Add() | Order: {Order}", order.Id.ToString());
            if (_repository.Add(order))
            {
                _messagingService.SendMessage(order.Id.ToString(), "order-queue");
                return true;
            }

            return false;
        }

        public List<OrderDto> Get(OrderFilterModel filter)
        {
            _logger.LogInformation(" OrderService - Get() | Filter: {Filter}", filter);
            return _repository.Get(filter);

        }

        public async Task<OrderDto> GetById(string encryptedOrderId)
        {
            _logger.LogInformation(" OrderService - GetById() | EncryptedOrderId: {EncryptedOrderId}", encryptedOrderId);

            ObjectId orderId = _encryptor.DecryptObjectId(encryptedOrderId);

            if (orderId == ObjectId.Empty)
            {
                _logger.LogError(" OrderService - GetById() | Invalid OrderId: {OrderId}", encryptedOrderId);
                throw new ValidationException("Invalid OrderId");
            }

            OrderEntity order = await _repository.GetOrderById(orderId);

            if (order == null)
            {
                _logger.LogError(" OrderService - GetById() | Order not found: {OrderId}", encryptedOrderId);
                throw new ValidationException("Order not found");
            }

            return OrderDto.Of(order, _encryptor);

        }

        public async Task<List<OrderDto>> GetAvailableOrders()
        {
            _logger.LogInformation(" OrderService - GetAvailableOrders()");
            return await _repository.GetAvailableOrders();
        }

        public async Task SendNotificationForElegibleUsers(string orderId)
        {
            _logger.LogInformation(" OrderService - SendNotificationForElegibleUsers() | OrderId: {OrderId}", orderId);

            if (!ObjectId.TryParse(orderId, out var _))
            {
                _logger.LogError(" OrderService - SendNotificationForElegibleUsers() | Invalid OrderId: {OrderId}", orderId);
                throw new ValidationException("Invalid OrderId");
            }

            OrderEntity order = await _repository.GetOrderById(ObjectId.Parse(orderId));

            if (order == null)
            {
                _logger.LogError(" OrderService - SendNotificationForElegibleUsers() | Order not found: {OrderId}", orderId);
                throw new ValidationException("Order not found");
            }

            List<ObjectId> userIds = new List<ObjectId>();

            // Get all users who are connected to the hub
            _hubService.GetAllConnectedUsers().ToList().ForEach(id => userIds.Add(ObjectId.Parse(id)));

            List<ObjectId> NotificatedUsers = new List<ObjectId>();

            // Verify if the users are able to accept the orderand and send notification
            _userService.GetUsersById(userIds).ForEach(async user =>
            {
                if (CanUserReceiveOrderNotification(user, order.Id))
                {
                    await _hubService.SendNotificationAsync(user.Id, OrderDto.Of(order, _encryptor));
                    NotificatedUsers.Add(user.Id);
                }
            });

            // Register users that have been notificated
            if (NotificatedUsers.Count > 0)
                await RegisterNotificatedUsers(order, NotificatedUsers);
        }

        public async Task<OrderDto> AcceptOrder(string encryptedOrderId, string encrptedUserId)
        {
            _logger.LogInformation(" OrderService - AcceptOrder() | encryptedOrderId: {encryptedOrderId} | encrptedUserId: {encrptedUserId}", encryptedOrderId, encrptedUserId);

            ObjectId orderId = _encryptor.DecryptObjectId(encryptedOrderId);
            ObjectId userId = _encryptor.DecryptObjectId(encrptedUserId);

            if (orderId == ObjectId.Empty)
            {
                _logger.LogError(" OrderService - AcceptOrder() | Invalid OrderId: {OrderId}", orderId);
                throw new ValidationException("Invalid OrderId");
            }

            if (userId == ObjectId.Empty)
            {
                _logger.LogError(" OrderService - AcceptOrder() | Invalid UserId: {UserId}", userId);
                throw new ValidationException("Invalid UserId");
            }

            var order = await _repository.GetOrderById(orderId);

            if (order == null)
            {
                _logger.LogError(" OrderService - AcceptOrder() | Invalid OrderId: {OrderId}", orderId);
                throw new ValidationException("Invalid OrderId");
            }

            if (!order.IsAvailableForDelivery())
            {
                _logger.LogError(" OrderService - AcceptOrder() | Order already accepted. OrderId: {OrderId}", orderId);
                throw new InvalidOperationException("Order already accepted");
            }

            var updatedUser = await _userService.UpdateUserOnPickUp(userId, order.Id);

            order!.Status = OrderStatus.OnDelivery;
            order!.Deliveries!.Add(Delivery.Of(updatedUser, DeliveryStatus.OnDelivery));

            OrderEntity updatedOrder = await _repository.UpdateOrderOnDelivery(order!);

            if (updatedOrder == null)
            {
                _logger.LogError(" OrderService - AcceptOrder() | Order not accepted. OrderId: {OrderId}", orderId);
                throw new InvalidOperationException("Order not accepted");
            }

            return OrderDto.Of(updatedOrder, _encryptor);

        }


        public async Task DeclineOrder(string encryptedOrderId, string encrptedUserId)
        {
            _logger.LogInformation(" OrderService - DeclineOrder() | encryptedOrderId: {encryptedOrderId} | encrptedUserId: {encrptedUserId}", encryptedOrderId, encrptedUserId);

            ObjectId orderId = _encryptor.DecryptObjectId(encryptedOrderId);

            if (orderId == ObjectId.Empty)
            {
                _logger.LogError(" OrderService - DeclineOrder() | Invalid OrderId: {OrderId}", encryptedOrderId);
                throw new ValidationException("Invalid OrderId");
            }

            ObjectId userId = _encryptor.DecryptObjectId(encrptedUserId);

            if (userId == ObjectId.Empty)
            {
                _logger.LogError(" OrderService - DeclineOrder() | Invalid UserId: {UserId}", encrptedUserId);
                throw new ValidationException("Invalid UserId");
            }

            var order = await _repository.GetOrderById(orderId);

            if (order == null)
            {
                _logger.LogError(" OrderService - DeclineOrder() | Invalid OrderId: {OrderId}", orderId);
                throw new ValidationException("Invalid OrderId");
            }

            if (order.IsOnDelivery())
            {
                _logger.LogError(" OrderService - DeclineOrder() | Order is not on delivery anymore. OrderId: {OrderId}", orderId);
                throw new InvalidOperationException("Order is not on delivery anymore");
            }

            await _userService.UpdateUserOnDecline(userId, order.Id);

            order.Status = OrderStatus.Available;
            order.Deliveries!.ForEach(delivery =>
            {
                if (delivery.UserId == userId)
                {
                    delivery.UpdateDate = DateTime.Now;
                    delivery.Status = DeliveryStatus.DeliveryDeclined;
                }
            });

            var updateResult = await _repository.UpdateOrderOnDecline(order!);

            if (!(updateResult.IsAcknowledged && updateResult.MatchedCount == 1 && updateResult.ModifiedCount == 1))
            {
                _logger.LogError(" OrderService - DeclineOrder() | Order not accepted. OrderId: {OrderId}", orderId);
                throw new InvalidOperationException("Order not accepted");
            }

            await SendNotificationForElegibleUsers(order.Id.ToString());

        }

        private async Task RegisterNotificatedUsers(OrderEntity order, List<ObjectId> userIds)
        {
            _logger.LogInformation(" OrderService - RegisterNotificatedUsers() | OrderId: {OrderId}", order.Id);

            _userService.GetUsersById(userIds).ForEach(user =>
            {
                order!.Notifications!.Add(Notification.Of(user));
            });

            var updateResult = await _repository.UpdateOrderNotifications(order!);

            if (updateResult.MatchedCount == 0)
            {
                _logger.LogError(" OrderService - RegisterNotificatedUsers() |  OrderId: {OrderId} | Notification cannot be registered.", order);
            }
        }

        private bool CanUserReceiveOrderNotification(UserEntity user, ObjectId orderId)
        {
            if (user.Status != UserStatus.Available)
                return false;

            if (user.Deliveries!.Find(delivery => delivery.OrderId == orderId) != null)
                return false;

            return true;
        }
    }
}