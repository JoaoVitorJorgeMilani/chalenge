
using Main.Settings.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Main.App.Domain.User
{
    public class UserEntity : BaseEntity
    {
        public string? Name { get; set; }

        public string? Cnpj { get; set; }

        public string? Cnh { get; set; }

        public string? Birthday { get; set; }

        public bool HasRentedBike { get; set; }

        public UserStatus? Status { get; set; }

        public ObjectId? DeliveringOrder { get; set; }

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

        public UserEntity() : base()
        {
            Status = UserStatus.Offline;
        }


        [BsonIgnore]
        private List<Delivery>? deliveries { get; set; }

        [BsonIgnore]
        private ObjectId? deliveringOrder { get; set; }


    }
}
