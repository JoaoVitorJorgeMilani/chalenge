using Main.Settings.Database;
using Main.Utils;
using MongoDB.Driver;

namespace Main.App.Domain.Order
{
    public class OrderRepository : BaseMongoRepository<Order>, IOrderRepository
    {
        private readonly IEncryptor encryptor;

        public OrderRepository(IMongoDatabase mongoDatabase, IEncryptor encryptor): base(mongoDatabase, "Orders")
        {
            this.encryptor = encryptor;
        }

        public bool Add(Order order)
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
        
    }
}