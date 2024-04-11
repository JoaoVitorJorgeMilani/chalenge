using Main.Utils;

namespace Main.App.Domain.Order
{
    public class NotificationDto
    {
        public string? encrptedUserId { get; set; }
        public DateTime notificationDate { get; set; }
        public string? userName { get; set; }
        public string? userCnpj { get; set; }

        public static NotificationDto Of(Notification notification, IEncryptor encryptor)
        {
            return new NotificationDto()
            {
                encrptedUserId = encryptor.Encrypt(notification.UserId),
                notificationDate = notification.NotificationDate,
                userName = notification.UserName,
                userCnpj = notification.UserCnpj
            };
        }
    }
}