namespace OutboundUp.Models
{
    public class OutboundWebhookResult
    {
        public int Id { get; set; }
        public int ResponseCode { get; set; }
        public string? ResponseBody { get; set; }
        public bool IsSuccess { get; set; } = false;

        public int WebhookId { get; set; }
        public int SpeedTestResultId { get; set; }

        public virtual OutboundWebHook WebHook { get; set; }
        public virtual SpeedTestResult SpeedTestResult { get; set; }
    }
}
