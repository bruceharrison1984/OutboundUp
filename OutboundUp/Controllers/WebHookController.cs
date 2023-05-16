using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutboundUp.Database;
using OutboundUp.Models;

namespace OutboundUp.Controllers
{
    public class WebHookController : Controller
    {
        private readonly OutboundUpDbContext _dbContext;

        public WebHookController(OutboundUpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OutboundWebHook>>> Index(CancellationToken cancellationToken)
        {
            var results = _dbContext.OutboundWebHooks
                .OrderBy(x => x.Id)
                .Include(x => x.Results.OrderByDescending(x => x.Id).Take(1));
            return new ApiResponse<IEnumerable<OutboundWebHook>>(results);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CreateWebHookRequest req, CancellationToken cancellationToken)
        {
            await _dbContext.OutboundWebHooks.AddAsync(new OutboundWebHook { TargetUrl = req.TargetUrl });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
        {
            await _dbContext.OutboundWebHooks.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }
    }
}
