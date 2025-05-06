using Hangfire;
using HangFire.Configuration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HangFire.Configuration
{
    public static class HangFireServiceExtensions
    {
        public static void AddHangFireServices(this IServiceCollection services, string hangFireConnectionString)
        {
            // Add services to the container.
            // Configure Hangfire
            // Database has to be created beforehand
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangFireConnectionString)); // Use SQL Server

            services.AddTransient<IHangfireBackgroundService, HangfireBackgroundService>();

            // Add the processing server as a hosted service.  This starts Hangfire
            // server within your ASP.NET Core application.  This is what processes the background jobs.
            services.AddHangfireServer();
        }

        public static void RegisterHangfireDashboardMiddleware(this WebApplication app, string path)
        {
            app.UseHangfireDashboard(path);
        }
    }
}
