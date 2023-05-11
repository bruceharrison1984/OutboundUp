using Microsoft.EntityFrameworkCore;
using OutboundUp.Database;
using OutboundUp.Jobs;
using OutboundUp.SpeedTests.Ookla;
using Quartz;

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

            builder.Services.AddDbContext<OutboundUpDbContext>(o => o.UseInMemoryDatabase("OutboundUp"));
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
            });
            builder.Services.AddQuartzServer(q =>
                {
                    q.WaitForJobsToComplete = true;
                });
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
            builder.Services.AddHttpClient();
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

            app.Run();
        }
    }
}