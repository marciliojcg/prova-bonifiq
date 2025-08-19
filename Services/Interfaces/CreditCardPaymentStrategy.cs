namespace ProvaPub.Services.Interfaces
{
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethod => "creditcard";

        public async Task<bool> ProcessPayment(decimal amount)
        {
            // Lógica específica para pagamento com cartão
            await Task.Delay(100); 
            return true;
        }
    }
}
