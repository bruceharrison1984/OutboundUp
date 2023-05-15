namespace OutboundUp.Models
{
    public class ChartData
    {
        public IEnumerable<NgxLineChartLine>? Bandwidth { get; set; }
        public IEnumerable<NgxLineChartLine>? Latency { get; set; }
    }
}
