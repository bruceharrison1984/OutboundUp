using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OutboundUp.Database;
using OutboundUp.Jobs;
using OutboundUp.Models;
using OutboundUp.Services;
using OutboundUp.SpeedTests.Ookla;
using Quartz;
using System.Text.Json.Serialization;

namespace OutboundUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders().AddSimpleConsole(x =>
            {
                x.SingleLine = true;
                x.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Disabled;
                x.TimestampFormat = "[HH:mm:ss] ";
            });

            builder.Services.AddDbContext<OutboundUpDbContext>();
            builder.Services.Configure<QuartzOptions>(builder.Configuration.GetSection("Quartz"));

            builder.Services.Configure<OutboundUpOptions>(builder.Configuration.GetSection("OutboundUp"));

            // read options again so we have values for the job intervals
            var options = builder.Configuration.GetSection("OutboundUp").Get<OutboundUpOptions>();

            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("SpeedTestJob");
                q.AddJob<SpeedTestJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SpeedTestJob-Trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(options.SpeedTestIntervalMinutes).RepeatForever())
                );

                var cleanupJobKey = new JobKey("DataCleanupJob");
                q.AddJob<DataCleanupJob>(opts => opts.WithIdentity(cleanupJobKey));

                q.AddTrigger(opts => opts
                    .ForJob(cleanupJobKey)
                    .WithIdentity("DataCleanupJob-Trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInHours(options.DataCleanupIntervalHours).RepeatForever())
                );
            });
            builder.Services.AddQuartzServer(q =>
                {
                    q.WaitForJobsToComplete = true;
                });
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IWebHookService, OutboundWebHookService>();
            builder.Services.AddTransient<SpeedTestJob>();
            builder.Services.AddTransient<OoklaSpeedTest>();
            builder.Services.AddCors();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<OutboundUpDbContext>();
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Running DB migrations...");
                    dbContext.Database.Migrate();
                }
                else
                {
                    logger.LogInformation("No DB migrations required");
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var opts = scope.ServiceProvider.GetRequiredService<IOptions<OutboundUpOptions>>();
                logger.LogInformation($"Loaded configuration - SpeedTestIntervalMinutes: '{opts.Value.SpeedTestIntervalMinutes}' | DataCleanupIntervalHours: '{opts.Value.DataCleanupIntervalHours}' | StaleEntryTTLDays: '{opts.Value.StaleEntryTTLDays}'");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");
            app.Run();
        }
    }
}