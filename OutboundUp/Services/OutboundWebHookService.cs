using OutboundUp.Database;
using OutboundUp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        private readonly JsonSerializerOptions _jsonOptions;

        public OutboundWebHookService(ILogger<OutboundWebHookService> logger, IHttpClientFactory httpClientFactory, OutboundUpDbContext dbContext)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;

            _jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
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
                    if (!Uri.IsWellFormedUriString(x.TargetUrl, UriKind.Absolute))
                    {
                        _logger.LogInformation($"Invalid webhook url '{x.TargetUrl}', URLs must be absolute");

                        await _dbContext.OutboundWebHookResult.AddAsync(new OutboundWebhookResult
                        {
                            ResponseCode = 0,
                            ResponseBody = "Error: URLs must be absolute.",
                            WebhookId = x.Id,
                            SpeedTestResultId = result.Id
                        }, cancellation);

                        return;
                    }

                    var resp = await client.PostAsJsonAsync(x.TargetUrl, new WebHookPayload(result), _jsonOptions, cancellationToken: cancellation);

                    if (!resp.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Failed to send results to webhook url '${x.TargetUrl}'");

                        var responseBody = await resp.Content.ReadAsStringAsync(cancellation);
                        _logger.LogInformation($"HTTP status code '${resp.StatusCode}' with body '${responseBody}'");

                        await _dbContext.OutboundWebHookResult.AddAsync(new OutboundWebhookResult
                        {
                            ResponseCode = (int)resp.StatusCode,
                            ResponseBody = responseBody,
                            WebhookId = x.Id,
                            SpeedTestResultId = result.Id
                        }, cancellation);

                        return;
                    }

                    _logger.LogInformation($"Succeeded in sending results to webhook url '${x.TargetUrl}'");

                    await _dbContext.OutboundWebHookResult.AddAsync(new OutboundWebhookResult
                    {
                        IsSuccess = true,
                        ResponseCode = (int)resp.StatusCode,
                        WebhookId = x.Id,
                        SpeedTestResultId = result.Id
                    }, cancellation);
                }).ToArray();

                Task.WaitAll(webhookTasks);

                await _dbContext.SaveChangesAsync(cancellation);
            }
        }
    }
}
