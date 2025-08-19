namespace ProvaPub.Contracts
{
    public class OrderRequest
    {
        public string PaymentMethod { get; set; }
        public decimal PaymentValue { get; set; }
        public int CustomerId { get; set; }
    }
}
