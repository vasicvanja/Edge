namespace Edge.DomainModels
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public string PaymentIntentId { get; set; }
        public string ReceiptUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Metadata { get; set; }
    }
}
