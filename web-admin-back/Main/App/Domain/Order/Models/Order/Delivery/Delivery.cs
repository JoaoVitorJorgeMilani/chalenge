using Main.App.Domain.User;
using Main.Settings.Database;
using MongoDB.Bson;

namespace Main.App.Domain.Order
{
    public class Delivery : BaseEntity
    {
        public ObjectId UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserCnpj { get; set; }
        public DeliveryStatus Status { get; set; }

        public static Delivery Of(UserEntity user, DeliveryStatus status)
        {
            return new Delivery()
            {
                UserId = user.Id,
                UserName = user.Name,
                UserCnpj = user.Cnpj,
                Status = status
            };
        }
    }
}