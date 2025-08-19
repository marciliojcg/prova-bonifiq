namespace ProvaPub.Services.Interfaces
{
    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethod => "paypal";

        public async Task<bool> ProcessPayment(decimal amount)
        {
            // Lógica específica para pagamento com PayPal
            await Task.Delay(100); 
            return true;
        }
    }
}
