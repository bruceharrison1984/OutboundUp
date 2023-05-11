using Microsoft.AspNetCore.Mvc;
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

        public async Task<HealthCheckResponse> Index()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var currentJobs = await scheduler.GetCurrentlyExecutingJobs();

            return new HealthCheckResponse { IsJobRunning = currentJobs.Any() };
        }

        public class HealthCheckResponse
        {
            public bool IsJobRunning { get; set; } = false;
        }
    }
}
