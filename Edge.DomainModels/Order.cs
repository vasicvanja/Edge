using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Edge.DomainModels
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PaymentIntentId { get; set; }
        public string ReceiptUrl { get; set; }
        public string Description { get; set; }
        public string BillingAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(max)")]
        public string MetadataJson { get; set; }

        [NotMapped]
        public Dictionary<string, string> Metadata
        {
            get => string.IsNullOrEmpty(MetadataJson)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(MetadataJson);

            set => MetadataJson = JsonSerializer.Serialize(value);
        }

        public ApplicationUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
