using EducaOnline.Aluno.API.Data;
using EducaOnline.Aluno.API.Middlewares;
using EducaOnline.WebAPI.Core.Identidade;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Aluno.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AlunoDbContext>(options =>
               options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                 });

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                         .WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                         .WithExposedHeaders("X-Pagination"));
            });

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfig();

            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        }

        public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfig();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            app.MapControllers();
        }
    }
}
