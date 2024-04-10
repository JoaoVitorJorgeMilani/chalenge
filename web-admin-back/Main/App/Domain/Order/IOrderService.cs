namespace Main.App.Domain.Order
{
    public interface IOrderService 
    {
        public bool Add(Order order);
        public List<OrderDto> Get(OrderFilterModel filter);
        public void SendNotificationForElegibleUsers(string orderId);
        
    }
}