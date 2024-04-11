using MongoDB.Bson;
using MongoDB.Driver;

namespace Main.App.Domain.Order
{
    public interface IOrderRepository
    {
        public bool Add(OrderEntity order);
        public List<OrderDto> Get(OrderFilterModel filter);
        public Task<OrderEntity> GetOrderById(ObjectId orderId);
        public Task<UpdateResult> UpdateOrderNotifications(OrderEntity order);
        public Task<List<OrderDto>> GetAvailableOrders();
        public Task<OrderEntity> UpdateOrderOnDelivery(OrderEntity order);
        public Task<UpdateResult> UpdateOrderOnDecline(OrderEntity order);


    }
}