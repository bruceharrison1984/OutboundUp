using Microsoft.AspNetCore.Mvc;
using OutboundUp.Models;
using Quartz;

namespace OutboundUp.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public HealthCheckController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<ApiResponse<HealthCheckData>> Index()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var currentJobs = await scheduler.GetCurrentlyExecutingJobs();

            return new ApiResponse<HealthCheckData>(new HealthCheckData { IsJobRunning = currentJobs.Any() });
        }
    }
}
