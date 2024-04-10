using Main.Settings.Database;
using MongoDB.Bson;

namespace Main.App.Domain.User
{
    public class Delivery : BaseEntity
    {

        public ObjectId OrderId { get; set; }
        public DeliveryStatus Status { get; set; }

        public static Delivery Of(ObjectId orderId, DeliveryStatus status)
        {
            return new Delivery()
            {
                OrderId = orderId,
                Status = status
            };
        }
    }
}