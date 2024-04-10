using Main.Utils;

namespace Main.App.Domain.Order
{
    public class OrderDto
    {
        public string? encryptedId { get; set; }
        public string? identifier { get; set; }
        public decimal? fare { get; set; }

        public static OrderDto Of(Order order, IEncryptor encryptor)
        {
            return new OrderDto(
                encryptor.Encrypt(order.Id),
                order.Identifier,
                order.Fare
            );
        }

        private OrderDto(
            string? encryptedId,
            string? identifier,
            decimal? fare
        )
        {
            this.encryptedId = encryptedId;
            this.identifier = identifier;
            this.fare = fare;
        }

        public OrderDto() { }
    }
}



