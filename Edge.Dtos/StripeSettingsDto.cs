namespace Edge.Dtos
{
    public class StripeSettingsDto
    {
        public string SecretKey { get; set; }
        public string PublicKey { get; set; }
        public string WebhookSecret { get; set; }
    }
}
