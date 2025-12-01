using Microsoft.EntityFrameworkCore;
using EducaOnline.Financeiro.API.Data;
using EducaOnline.WebAPI.Core.Identidade;
using EducaOnline.Financeiro.API.Facade;

namespace EducaOnline.Financeiro.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FinanceiroContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddControllers();

            services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

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

            app.MapControllers();

            return app;
        }
    }

    
}
