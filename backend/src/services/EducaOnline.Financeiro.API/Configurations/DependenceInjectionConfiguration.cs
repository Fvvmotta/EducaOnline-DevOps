
using EducaOnline.Financeiro.API.Data;
using EducaOnline.Financeiro.API.Data.Repository;
using EducaOnline.Financeiro.API.Facade;
using EducaOnline.Financeiro.API.Models;
using EducaOnline.Financeiro.API.Services;

namespace EducaOnline.Financeiro.API.Configurations
{
    public static class DependenceInjectionConfiguration
    {
        public static IServiceCollection AddDependenceInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<FinanceiroContext>();
            return services;
        }
    }
}
