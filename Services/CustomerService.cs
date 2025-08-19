using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(TestDbContext ctx) : base(ctx) { }

        protected override IQueryable<Customer> GetBaseQuery() =>
            _ctx.Customers.OrderBy(c => c.Id);

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            // Implementação existente do repositório original
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));
            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            await ExistCustomer(customerId);

            bool isOnePaymentPerMonth = await HasOnePaymentPerMonth(customerId);
            if (isOnePaymentPerMonth == false)
            {
                return false;
            }

            bool isMaxFirstPurchase = await ValidateMaxFirstPurchase(customerId, purchaseValue);
            if (isMaxFirstPurchase == false)
            {
                return false;
            }

            bool isPurchaseOnlyWorkingDays = ValidatePurchaseOnlyWorkingDays();
            if (isPurchaseOnlyWorkingDays == false)
            {
                return false;
            }

            return true;
        }

        private static bool  ValidatePurchaseOnlyWorkingDays()
        {
            //Business Rule: A customer can purchases only during business hours and working days
            if (DateTime.UtcNow.Hour < 8 || DateTime.UtcNow.Hour > 18 || DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday || DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                return false;
            return true;
        }

        private async Task<bool> ValidateMaxFirstPurchase(int customerId, decimal purchaseValue)
        {
            decimal maxValuePurchase = 100;
            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > maxValuePurchase)
                return false;
            return true;
        }

        private async Task<bool> HasOnePaymentPerMonth(int customerId)
        {
            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;
            return true;
        }

        private async Task ExistCustomer(int customerId)
        {
            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");
        }
    }
}
