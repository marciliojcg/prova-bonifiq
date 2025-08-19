using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

        public PaymentStrategyFactory(IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _paymentStrategies = paymentStrategies;
        }

        public IPaymentStrategy GetPaymentStrategy(string paymentMethod)
        {
            var strategy = _paymentStrategies.FirstOrDefault(s =>
                s.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

            if (strategy == null)
                throw new ArgumentException($"Método de pagamento não suportado: {paymentMethod}");

            return strategy;
        }
    }
}
