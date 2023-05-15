using OutboundUp.Database;
using OutboundUp.Models;
using OutboundUp.Services;
using OutboundUp.SpeedTests.Ookla;
using Quartz;

namespace OutboundUp.Jobs
{
    public class SpeedTestJob : IJob
    {
        private readonly OoklaSpeedTest _speedtestClient;
        private readonly OutboundUpDbContext _dbContext;
        private readonly IWebHookService _webhookService;
        private readonly ILogger<SpeedTestJob> _logger;

        public SpeedTestJob(OoklaSpeedTest speedtestClient, OutboundUpDbContext dbContext, IWebHookService webhookService, ILogger<SpeedTestJob> logger)
        {
            _speedtestClient = speedtestClient;
            _dbContext = dbContext;
            _webhookService = webhookService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var output = await _speedtestClient.RunSpeedTest();

                await _dbContext.SpeedTestResults.AddAsync(output);
                await _dbContext.SaveChangesAsync();

                await _webhookService.SendResultsToWebHooks(output, context.CancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"SpeedTest failed to complete");
                await _dbContext.SpeedTestResults.AddAsync(new SpeedTestResult
                {
                    UnixTimestampMs = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    IsSuccess = false,
                });
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
