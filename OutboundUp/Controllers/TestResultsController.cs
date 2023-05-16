using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutboundUp.Database;
using OutboundUp.Models;

namespace OutboundUp.Controllers
{
    public class TestResultsController : Controller
    {
        private readonly OutboundUpDbContext _dbContext;

        public TestResultsController(OutboundUpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApiResponse<ChartData> ChartData()
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

            return new ApiResponse<ChartData>(new ChartData
            {
                Bandwidth = new NgxLineChartLine[] { downloadSpeedLine, uploadSpeedLine },
                Latency = new NgxLineChartLine[] { pingLine, downloadLatencyLine, uploadLatencyLine }
            });
        }

        public async Task<ApiResponse<DataTableResults>> Raw(int pageNumber = 0, int pageSize = 10)
        {
            var data = _dbContext.SpeedTestResults.OrderByDescending(x => x.Id).Skip(pageNumber * pageSize).Take(pageSize);
            var totalCount = await _dbContext.SpeedTestResults.CountAsync();

            var response = new DataTableResults
            {
                TotalCount = totalCount,
                TableData = data,
                CurrentPage = pageNumber,
            };

            return new ApiResponse<DataTableResults>(response);
        }

        public async Task<ApiResponse<Statistics>> Statistics()
        {
            var baseQuery = _dbContext.SpeedTestResults.OrderByDescending(x => x.Id).Take(50);

            var statistics = new Statistics
            {
                AverageDownloadSpeed = Math.Round(await baseQuery.AverageAsync(x => x.DownloadSpeed)),
                AverageUploadSpeed = Math.Round(await baseQuery.AverageAsync(x => x.UploadSpeed)),
                FailedHealthChecks = await baseQuery.CountAsync(x => !x.IsSuccess),
                SuccessfulHealthChecks = await baseQuery.CountAsync(x => x.IsSuccess),
                AveragePing = Math.Round(await baseQuery.AverageAsync(x => x.PingAverage), 2)
            };

            return new ApiResponse<Statistics>(statistics);
        }

        public class DataTableResults
        {
            public int TotalCount { get; set; }
            public int CurrentPage { get; set; }
            public IEnumerable<SpeedTestResult> TableData { get; set; }
        }
    }
}

