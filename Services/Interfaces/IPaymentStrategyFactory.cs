namespace ProvaPub.Services.Interfaces
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(string paymentMethod);
    }
}
