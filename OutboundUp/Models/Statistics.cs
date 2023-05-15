namespace OutboundUp.Models
{
    public class Statistics
    {
        public int FailedHealthChecks { get; set; }
        public int SuccessfulHealthChecks { get; set; }
        public double AveragePing { get; set; }
        public double AverageDownloadSpeed { get; set; }
        public double AverageUploadSpeed { get; set; }
    }
}
