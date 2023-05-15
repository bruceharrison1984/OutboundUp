using Microsoft.EntityFrameworkCore;
using OutboundUp.Models;

namespace OutboundUp.Database
{
    public class OutboundUpDbContext : DbContext
    {
        const string DbPath = "/app/outbound-up.db";

        public OutboundUpDbContext(DbContextOptions<OutboundUpDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboundWebHook>()
                .HasMany(x => x.Results)
                .WithOne(x => x.WebHook)
                .HasForeignKey(x => x.Id)
                .HasPrincipalKey(x => x.Id);
        }

        public DbSet<SpeedTestResult> SpeedTestResults { get; set; }
        public DbSet<OutboundWebHook> OutboundWebHooks { get; set; }
        public DbSet<OutboundWebhookResult> OutboundWebHookResult { get; set; }


    }
}
