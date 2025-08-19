namespace ProvaPub.Services.Interfaces
{
    public class PixPaymentStrategy : IPaymentStrategy
    {
        public string PaymentMethod => "pix";

        public async Task<bool> ProcessPayment(decimal amount)
        {
            // Lógica específica para pagamento com PIX
            await Task.Delay(100); 
            return true;
        }
    }

}
