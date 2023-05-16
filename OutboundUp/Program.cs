using Microsoft.EntityFrameworkCore;
using OutboundUp.Database;
using OutboundUp.Jobs;
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
            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("SpeedTestJob");
                q.AddJob<SpeedTestJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SpeedTestJob-Trigger")
                    .WithCronSchedule("0 * * ? * *")
                );

                var cleanupJobKey = new JobKey("DataCleanupJob");
                q.AddJob<DataCleanupJob>(opts => opts.WithIdentity(cleanupJobKey));

                q.AddTrigger(opts => opts
                    .ForJob(cleanupJobKey)
                    .WithIdentity("DataCleanupJob-Trigger")
                    .WithCronSchedule("0 0 0 ? * *") // run every midnight to clean up data
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


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
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

            app.Run();
        }
    }
}