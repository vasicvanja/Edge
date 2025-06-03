namespace Edge.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentIntentId { get; set; }
        public string ReceiptUrl { get; set; }
        public string Description { get; set; }
        public string BillingAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Dictionary<string, string> Metadata { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
