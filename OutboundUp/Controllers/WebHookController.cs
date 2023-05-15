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

        public async Task<IActionResult> Index()
        {
            return Ok(await _dbContext.OutboundWebHooks.ToListAsync());
        }

        public async Task<IActionResult> Post(CreateWebHookRequest req)
        {
            await _dbContext.AddAsync(new OutboundWebHook { TargetUrl = req.TargetUrl });
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
