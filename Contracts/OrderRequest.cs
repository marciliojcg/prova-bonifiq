namespace ProvaPub.Contracts
{
    public class OrderRequest
    {
        public string paymentMethod { get; set; }
        public decimal paymentValue { get; set; }
        public int customerId { get; set; }
    }
}
