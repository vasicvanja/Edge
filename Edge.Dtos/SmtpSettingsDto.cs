namespace Edge.Dtos
{
    public class SmtpSettingsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Authentication { get; set; }
        public bool EnableSsl { get; set; }
        public bool EnableSmtpSettings { get; set; }
    }
}
