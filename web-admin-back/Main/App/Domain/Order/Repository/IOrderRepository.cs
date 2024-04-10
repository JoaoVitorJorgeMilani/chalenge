namespace Main.App.Domain.Order
{
    public interface IOrderRepository
    {
        public bool Add(Order order);
        public List<OrderDto> Get(OrderFilterModel filter);

    }
}