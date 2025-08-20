using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
    public interface ICustomerService : IBaseService<Customer> {

        Task<bool> CanPurchase(int customerId, decimal purchaseValue);
        bool ValidatePurchaseOnlyWorkingDays(DateTime datePurchase);

    }
}
