using EducaOnline.Bff.Extensions;
using EducaOnline.WebAPI.Core.Identidade;
using EducaOnline.WebAPI.Core.Configuration;

namespace EducaOnline.Bff.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<AppServicesSettings>(configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
            services.AddHealthCheckConfig(configuration);
            return services;
        }
        public static WebApplication UseApiConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseCors("Total");
            app.UseAuthConfiguration();
            app.UseHealthCheckConfig();
            app.MapControllers();
            return app;
        }
    }
}
