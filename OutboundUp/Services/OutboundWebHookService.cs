using OutboundUp.Database;
using OutboundUp.Models;

namespace OutboundUp.Services
{
    public interface IWebHookService
    {
        Task SendResultsToWebHooks(SpeedTestResult result, CancellationToken cancellation);
    }

    public class OutboundWebHookService : IWebHookService
    {
        private readonly ILogger<OutboundWebHookService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OutboundUpDbContext _dbContext;

        public OutboundWebHookService(ILogger<OutboundWebHookService> logger, IHttpClientFactory httpClientFactory, OutboundUpDbContext dbContext)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method will send the SpeedTestResult as a JSON payload to all registered web hook urls
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task SendResultsToWebHooks(SpeedTestResult result, CancellationToken cancellation)
        {
            var webhookTargets = _dbContext.OutboundWebHooks;

            if (!webhookTargets.Any())
            {
                _logger.LogInformation("No webhooks registered, results will not be forwarded");
                return;
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                var webhookTasks = webhookTargets.ToList().Select(async x =>
                {
                    var resp = await client.PostAsJsonAsync(x.TargetUrl, result, cancellationToken: cancellation);

                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Failed to send results to webhook url '${x.TargetUrl}'");

                        var responseBody = await resp.Content.ReadAsStringAsync(cancellation);
                        _logger.LogInformation($"HTTP status code '${resp.StatusCode}' with body '${responseBody}'");

                        await _dbContext.OutboundWebHookResult.AddAsync(new OutboundWebhookResult
                        {
                            ResponseCode = (int)resp.StatusCode,
                            ResponseBody = responseBody,
                            WebHook = x
                        }, cancellation);

                        return;
                    }

                    _logger.LogInformation($"Succeeded in sending results to webhook url '${x.TargetUrl}'");

                    await _dbContext.OutboundWebHookResult.AddAsync(new OutboundWebhookResult
                    {
                        IsSuccess = true,
                        ResponseCode = (int)resp.StatusCode,
                        WebHook = x
                    }, cancellation);
                }).ToArray();

                Task.WaitAll(webhookTasks);

                await _dbContext.SaveChangesAsync(cancellation);
            }
        }
    }
}
