namespace OutboundUp.Models
{
    public class NgxLineChartLine
    {
        public string Name { get; set; }
        public IEnumerable<NgxLineChartSeriesData> Series { get; set; }
    }

    public class NgxLineChartSeriesData
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
    }
}
