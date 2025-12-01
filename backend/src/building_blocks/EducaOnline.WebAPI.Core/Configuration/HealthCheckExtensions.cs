using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace EducaOnline.WebAPI.Core.Configuration
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthCheckConfig(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("API is running"),
                    tags: new[] { "ready", "live" });

            return services;
        }

        public static IApplicationBuilder UseHealthCheckConfig(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = WriteHealthCheckResponse
            });

            app.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = WriteHealthCheckResponse
            });

            app.UseHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("live"),
                ResponseWriter = WriteHealthCheckResponse
            });

            return app;
        }

        private static Task WriteHealthCheckResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                timestamp = DateTime.UtcNow,
                duration = report.TotalDuration.TotalMilliseconds,
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds,
                    error = e.Value.Exception?.Message
                })
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        }
    }
}
