using EducaOnline.Financeiro.API.Data;
using EducaOnline.WebAPI.Core.Configuration;

namespace EducaOnline.Financeiro.API.Configurations
{
    public class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            var services = app.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var financeiroContext = scope.ServiceProvider.GetRequiredService<FinanceiroContext>();

            await DbHealthChecker.TestConnection(financeiroContext);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
                await financeiroContext.Database.EnsureCreatedAsync();
        }
    }
}
