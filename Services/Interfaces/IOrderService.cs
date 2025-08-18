using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
    public interface IOrderService : IBaseService<Order> {

        Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId);

    }
}
