using Microsoft.AspNetCore.Mvc;
using OutboundUp.Database;
using OutboundUp.Models;
using OutboundUp.SpeedTests;

namespace OutboundUp.Controllers
{
    public class SpeedTestResultsController : Controller
    {
        private readonly OutboundUpDbContext _dbContext;

        public SpeedTestResultsController(OutboundUpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SpeedTestResultsResponse Index()
        {
            var speedTestData = _dbContext.SpeedTestResults.OrderBy(x => x.UnixTimestampMs);

            var downloadSpeedData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.UnixTimestampMs, Value = x.DownloadSpeed });
            var downloadSpeedLine = new NgxLineChartLine { Name = "Download Speed", Series = downloadSpeedData };

            var uploadSpeedData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.UnixTimestampMs, Value = x.UploadSpeed });
            var uploadSpeedLine = new NgxLineChartLine { Name = "Upload Speed", Series = uploadSpeedData };

            var pingData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.UnixTimestampMs, Value = x.PingAverage, Max = x.PingHigh, Min = x.PingLow });
            var pingLine = new NgxLineChartLine { Name = "Ping", Series = pingData };

            var downloadLatencyData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.UnixTimestampMs, Value = x.DownloadLatencyAverage, Max = x.DownloadLatencyHigh, Min = x.DownloadLatencyLow });
            var downloadLatencyLine = new NgxLineChartLine { Name = "Download Latency", Series = downloadLatencyData };

            var uploadLatencyData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.UnixTimestampMs, Value = x.UploadLatencyAverage, Max = x.UploadLatencyHigh, Min = x.UploadLatencyLow });
            var uploadLatencyLine = new NgxLineChartLine { Name = "Upload Latency", Series = uploadLatencyData };

            return new SpeedTestResultsResponse(
                new NgxLineChartLine[] { downloadSpeedLine, uploadSpeedLine },
                new NgxLineChartLine[] { pingLine, downloadLatencyLine, uploadLatencyLine });
        }

        public RawSpeedTestResultsResponse Raw() => new RawSpeedTestResultsResponse(_dbContext.SpeedTestResults.OrderByDescending(x => x.Id));

        public class SpeedTestResultsResponse
        {
            public SpeedTestResultsResponse(IEnumerable<NgxLineChartLine> bandwidthData, IEnumerable<NgxLineChartLine> latencyData)
            {
                Bandwidth = bandwidthData;
                Latency = latencyData;
            }
            public IEnumerable<NgxLineChartLine> Bandwidth { get; set; }
            public IEnumerable<NgxLineChartLine> Latency { get; set; }
        }

        public class RawSpeedTestResultsResponse
        {
            public RawSpeedTestResultsResponse(IEnumerable<SpeedTestResult> data)
            {
                Data = data;
            }
            public IEnumerable<SpeedTestResult> Data { get; set; }
        }
    }
}
