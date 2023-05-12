using Microsoft.EntityFrameworkCore;
using OutboundUp.SpeedTests;

namespace OutboundUp.Database
{
    public class OutboundUpDbContext : DbContext
    {
        const string DbPath = "/app/outbound-up.db";

        public OutboundUpDbContext(DbContextOptions<OutboundUpDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<SpeedTestResult> SpeedTestResults { get; set; }
    }
}
