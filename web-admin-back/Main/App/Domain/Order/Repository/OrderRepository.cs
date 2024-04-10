using Main.Settings.Database;
using Main.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog.Filters;

namespace Main.App.Domain.Order
{
    public class OrderRepository : BaseMongoRepository<OrderEntity>, IOrderRepository
    {
        private readonly IEncryptor encryptor;

        public OrderRepository(IMongoDatabase mongoDatabase, IEncryptor encryptor) : base(mongoDatabase, "Orders")
        {
            this.encryptor = encryptor;
        }

        public bool Add(OrderEntity order)
        {
            try
            {
                base.InsertOne(order);
                return true;
            }
            catch (MongoWriteException)
            {
                return false;
            }
        }

        public List<OrderDto> Get(OrderFilterModel filter)
        {
            var orders = base.Find(filter.GetFilterDefinition()).ToList();

            List<OrderDto> orderResp = new List<OrderDto>();
            orders.ForEach(order => orderResp.Add(OrderDto.Of(order, encryptor)));

            return orderResp;
        }

        public Task<OrderEntity> GetOrderById(ObjectId orderId)
        {
            var filter = Builders<OrderEntity>.Filter
                .Eq(order => order.Id, orderId);

            return base.FindFirstAsync(filter);

        }

        public Task<UpdateResult> UpdateOrderNotifications(OrderEntity order)
        {
            var filter = Builders<OrderEntity>.Filter.Eq(x => x.Id, order.Id);
            var update = Builders<OrderEntity>.Update
                .Set(x => x.Notifications, order.Notifications);

            return base.UpdateOneAsync(filter, update);

        }

        public async Task<List<OrderDto>> GetAvailableOrders()
        {
            var orders = await base.FindAsync(Builders<OrderEntity>.Filter.Eq(order => order.Status, OrderStatus.Available)).ContinueWith(x => x.Result.ToList());

            if (orders.Count <= 0)
                return new List<OrderDto>();

            return orders.Select(order => OrderDto.Of(order, encryptor)).ToList();
        }

        public Task<OrderEntity> UpdateOrderOnDelivery(OrderEntity order)
        {
            var filter = Builders<OrderEntity>.Filter.Eq(x => x.Id, order.Id);
            var update = Builders<OrderEntity>.Update
                .Set(x => x.Deliveries, order.Deliveries)
                .Set(x => x.Status, order.Status);

            return base.UpdateOneAndGetAsync(filter, update);

        }

        public Task<UpdateResult> UpdateOrderOnDecline(OrderEntity order)
        {
            var filter = Builders<OrderEntity>.Filter.Eq(x => x.Id, order.Id);
            var update = Builders<OrderEntity>.Update
                .Set(x => x.Deliveries, order.Deliveries)
                .Set(x => x.Status, order.Status);

            return base.UpdateOneAsync(filter, update);

        }
    }
}