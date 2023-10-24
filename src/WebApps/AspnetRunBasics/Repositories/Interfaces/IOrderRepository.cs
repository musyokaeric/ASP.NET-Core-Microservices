using AspnetRunBasics.Entities;

namespace AspnetRunBasics.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CheckOut(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
