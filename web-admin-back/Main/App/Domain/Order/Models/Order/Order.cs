using Main.Settings.Database;
using MongoDB.Bson.Serialization.Attributes;

namespace Main.App.Domain.Order
{
    public class OrderEntity : BaseEntity
    {
        public string? Identifier { get; set; }

        public decimal Fare { get; set; }

        public OrderStatus Status { get; set; }

        public List<Delivery>? Deliveries
        {
            get
            {
                if (deliveries == null)
                {
                    deliveries = new List<Delivery>();
                }
                return deliveries;
            }
            set
            {
                deliveries = value;
            }
        }

        public List<Notification>? Notifications
        {
            get
            {
                if (notifications == null)
                {
                    notifications = new List<Notification>();
                }
                return notifications;
            }
            set
            {
                notifications = value;
            }
        }

        public OrderEntity()
        {
            Status = OrderStatus.Available;
        }

        [BsonIgnore]
        private List<Notification>? notifications;



        [BsonIgnore]
        private List<Delivery>? deliveries;


        public bool IsAvailableForDelivery()
        {
            return Status == OrderStatus.Available;
        }

        public bool IsOnDelivery()
        {
            return Status == OrderStatus.Available;
        }

    }
}