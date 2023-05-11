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
            var speedTestData = _dbContext.HttpCheckResults.OrderByDescending(x => x.Timestamp).ToList();

            var downloadSpeedData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.Timestamp.ToString(), Value = x.DownloadSpeed });
            var downloadSpeedLine = new NgxLineChartLine { Name = "Download Speed", Series = downloadSpeedData };

            var uploadSpeedData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.Timestamp.ToString(), Value = x.UploadSpeed });
            var uploadSpeedLine = new NgxLineChartLine { Name = "Upload Speed", Series = uploadSpeedData };

            var pingData = speedTestData.Select(x => new NgxLineChartSeriesData { Name = x.Timestamp.ToString(), Value = x.PingAverage, Max = x.PingHigh, Min = x.PingLow });
            var pingLine = new NgxLineChartLine { Name = "Response Time", Series = pingData };

            return new SpeedTestResultsResponse(new NgxLineChartLine[] { downloadSpeedLine, uploadSpeedLine, pingLine });
        }

        public RawSpeedTestResultsResponse Raw()
        {
            return new RawSpeedTestResultsResponse(_dbContext.HttpCheckResults.OrderByDescending(x => x.Timestamp));
        }

        public class SpeedTestResultsResponse
        {
            public SpeedTestResultsResponse(IEnumerable<NgxLineChartLine> data)
            {
                LineChartData = data;
            }
            public IEnumerable<NgxLineChartLine> LineChartData { get; set; }
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
