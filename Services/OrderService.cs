using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{

    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;
        public OrderService(TestDbContext ctx, IPaymentStrategyFactory paymentStrategyFactory) : base(ctx) { _paymentStrategyFactory = paymentStrategyFactory; }

        protected override IQueryable<Order> GetBaseQuery() =>
           _ctx.Orders.OrderBy(p => p.Id);

        //public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        //{   


        //    if (paymentMethod == "pix")
        //    {
        //        //Faz pagamento...
        //    }
        //    else if (paymentMethod == "creditcard")
        //    {
        //        //Faz pagamento...
        //    }
        //    else if (paymentMethod == "paypal")
        //    {
        //        //Faz pagamento...
        //    }

        //    return await InsertOrder(new Order() //Retorna o pedido para o controller
        //    {  
        //        Value = paymentValue,
        //        CustomerId = customerId,
        //        OrderDate = DateTime.Now,

        //    });

        //}

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            // Obtém a estratégia de pagamento
            var paymentStrategy = _paymentStrategyFactory.GetPaymentStrategy(paymentMethod);

            // Processa o pagamento
            var paymentSuccess = await paymentStrategy.ProcessPayment(paymentValue);

            if (!paymentSuccess)
                throw new InvalidOperationException("Falha no processamento do pagamento");

            var order = new Order
            {
                Value = paymentValue,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow, 
            };

            return await InsertOrder(order);
        }

        public async Task<Order> InsertOrder(Order order)
        {
            var result = await _ctx.Orders.AddAsync(order);
            await _ctx.SaveChangesAsync(); 
            return result.Entity;
        }
    }


}
