using Main.Utils;

namespace Main.App.Domain.Order
{
    public class OrderDto
    {
        public string? encryptedId { get; set; }
        public string? identifier { get; set; }
        public decimal? fare { get; set; }
        public string? status { get; set; }
        public List<DeliveryDto> deliveries { get; set; } = new List<DeliveryDto>();
        public List<NotificationDto> notifications { get; set; } = new List<NotificationDto>();
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }

        public static OrderDto Of(OrderEntity order, IEncryptor encryptor)
        {
            return new OrderDto()
            {
                encryptedId = encryptor.Encrypt(order.Id),
                identifier = order.Identifier,
                fare = order.Fare,
                status = order.Status.ToString(),
                deliveries = order.Deliveries!.Select(delivery => DeliveryDto.Of(delivery, encryptor)).ToList(),
                notifications = order.Notifications!.Select(notification => NotificationDto.Of(notification, encryptor)).ToList(),
                createDate = order.CreateDate,
                updateDate = order.UpdateDate
            };
        }
    }
}



