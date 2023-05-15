namespace OutboundUp.Models
{
    public class OutboundWebHook
    {
        public int Id { get; set; }
        public string? TargetUrl { get; set; }
        public string HttpMethod { get; set; } = "POST";

        public virtual ICollection<OutboundWebhookResult>? Results { get; set; }
    }
}
