using EducaOnline.WebAPI.Core.Identidade;
using EducaOnLine.Pedidos.API.Data;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Pedidos.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PedidosContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddControllers();
            

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

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            app.MapControllers();

            return app;
        }
    }


}
