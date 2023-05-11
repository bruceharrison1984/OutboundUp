using Microsoft.EntityFrameworkCore;
using OutboundUp.SpeedTests;

namespace OutboundUp.Database
{
    public class OutboundUpDbContext : DbContext
    {

        public OutboundUpDbContext(DbContextOptions<OutboundUpDbContext> options) : base(options)
        {

        }

        public DbSet<SpeedTestResult> SpeedTestResults { get; set; }
    }
}
