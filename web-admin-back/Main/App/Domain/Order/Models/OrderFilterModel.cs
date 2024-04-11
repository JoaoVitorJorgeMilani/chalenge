using MongoDB.Driver;

namespace Main.App.Domain.Order
{
    public class OrderFilterModel
    {
        public string? Identifier { get; set; }

        public FilterDefinition<OrderEntity> GetFilterDefinition()
        {
            var filters = new List<FilterDefinition<OrderEntity>>();

            if (!string.IsNullOrEmpty(Identifier))
            {
                filters.Add(Builders<OrderEntity>.Filter.Eq(x => x.Identifier, Identifier));
            }

            if (filters.Count <= 0)
            {
                return Builders<OrderEntity>.Filter.Empty;
            }
            return Builders<OrderEntity>.Filter.And(filters);
        }
    }
}

