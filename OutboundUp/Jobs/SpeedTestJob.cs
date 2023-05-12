﻿using OutboundUp.Database;
using OutboundUp.SpeedTests;
using OutboundUp.SpeedTests.Ookla;
using Quartz;

namespace OutboundUp.Jobs
{
    public class SpeedTestJob : IJob
    {
        private readonly OoklaSpeedTest _speedtestClient;
        private readonly OutboundUpDbContext _dbContext;
        private readonly ILogger<SpeedTestJob> logger;

        public SpeedTestJob(OoklaSpeedTest speedtestClient, OutboundUpDbContext dbContext, ILogger<SpeedTestJob> logger)
        {
            _speedtestClient = speedtestClient;
            _dbContext = dbContext;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var output = await _speedtestClient.RunSpeedTest();

                await _dbContext.SpeedTestResults.AddAsync(output);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"SpeedTest failed");
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
