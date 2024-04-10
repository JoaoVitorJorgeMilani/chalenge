using Main.Settings.Database;

namespace Main.App.Domain.Order
{
    public class Order : BaseEntity
    {
        public string? Identifier { get; set; }
        
        // Mongo is serializing it to string
        public decimal Fare { get; set; }

    }
}