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

        public async Task<ApiResponse<IEnumerable<OutboundWebHook>>> Index(CancellationToken cancellationToken)
        {
            var results = await _dbContext.OutboundWebHooks.Include(x => x.Results.Take(50)).ToListAsync(cancellationToken);
            return new ApiResponse<IEnumerable<OutboundWebHook>>(results);
        }

        public async Task<IActionResult> Post(CreateWebHookRequest req, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(new OutboundWebHook { TargetUrl = req.TargetUrl });
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _dbContext.OutboundWebHooks.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);

            return Ok();
        }
    }
}
