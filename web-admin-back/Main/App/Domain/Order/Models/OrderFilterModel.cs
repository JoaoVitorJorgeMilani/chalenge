using MongoDB.Driver;

namespace Main.App.Domain.Order
{   
    public class OrderFilterModel
    {
        public string? Identifier { get; set; }

        public FilterDefinition<Order> GetFilterDefinition()
        {
            var filters = new List<FilterDefinition<Order>>();

            if (!string.IsNullOrEmpty(Identifier))
            {
                filters.Add(Builders<Order>.Filter.Eq(x => x.Identifier, Identifier));
            }

            if(filters.Count <= 0){
                return Builders<Order>.Filter.Empty;
            }
            return Builders<Order>.Filter.And(filters);
        }
    }
}

