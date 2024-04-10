using Main.Utils;

namespace Main.App.Domain.Order
{
    public class DeliveryDto
    {
        public string? encrptedUserId { get; set; }
        public string? status { get; set; }
        public string? userName { get; set; }
        public string? userCnpj { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }

        public static DeliveryDto Of(Delivery delivery, IEncryptor encryptor)
        {
            return new DeliveryDto()
            {
                encrptedUserId = encryptor.Encrypt(delivery.Id),
                status = delivery.Status.ToString(),
                userName = delivery.UserName,
                userCnpj = delivery.UserCnpj,
                createDate = delivery.CreateDate,
                updateDate = delivery.UpdateDate
            };
        }
    }
}