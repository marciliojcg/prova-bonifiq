namespace ProvaPub.Contracts
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
