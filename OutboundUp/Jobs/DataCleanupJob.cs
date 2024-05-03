using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OutboundUp.Database;
using OutboundUp.Models;
using Quartz;

namespace OutboundUp.Jobs
{
    public class DataCleanupJob : IJob
    {
        private readonly OutboundUpDbContext _dbContext;
        private readonly ILogger<DataCleanupJob> _logger;
        private readonly IOptions<OutboundUpOptions> _options;

        public DataCleanupJob(OutboundUpDbContext dbContext, ILogger<DataCleanupJob> logger, IOptions<OutboundUpOptions> options)
        {
            _dbContext = dbContext;
            _logger = logger;
            _options = options;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cutoffDate = DateTimeOffset.UtcNow.AddDays(-_options.Value.StaleEntryTTLDays).ToUnixTimeMilliseconds();
                _logger.LogInformation("Beginning data cleanup, deleting any entries older than 30 days");
                var oldResults = _dbContext.SpeedTestResults.Where(x => x.UnixTimestampMs < cutoffDate);
                var recordsDeleted = await oldResults.ExecuteDeleteAsync(context.CancellationToken);
                _logger.LogInformation($"{recordsDeleted} SpeedTestResult records were removed");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Data cleanup job failed. This should be investigated before disk space becomes an issue.");
            }
        }
    }
}
