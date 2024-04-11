namespace Main.App.Domain.Order
{
    public interface IOrderService
    {
        public bool Add(OrderEntity order);
        public List<OrderDto> Get(OrderFilterModel filter);
        public Task<OrderDto> GetById(string encryptedOrderId);
        public Task SendNotificationForElegibleUsers(string orderId);
        public Task<List<OrderDto>> GetAvailableOrders();
        public Task<OrderDto> AcceptOrder(string orderId, string userId);
        public Task DeclineOrder(string orderId, string userId);


    }
}