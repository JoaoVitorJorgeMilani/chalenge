using Main.App.Domain.User;
using MongoDB.Bson;

namespace Main.App.Domain.Order
{
    public class Notification
    {
        public ObjectId UserId { get; set; }
        public DateTime NotificationDate { get; set; }
        public string? UserName { get; set; }
        public string? UserCnpj { get; set; }


        public static Notification Of(UserEntity user)
        {
            return new Notification()
            {
                UserId = user.Id,
                NotificationDate = DateTime.Now,
                UserName = user.Name,
                UserCnpj = user.Cnpj
            };
        }

        public static Notification Of(ObjectId userId)
        {
            return new Notification()
            {
                UserId = userId,
                NotificationDate = DateTime.Now
            };
        }
    }
}