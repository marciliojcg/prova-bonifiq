public interface IPaymentStrategy
{
    string PaymentMethod { get; }
    Task<bool> ProcessPayment(decimal amount);
}
