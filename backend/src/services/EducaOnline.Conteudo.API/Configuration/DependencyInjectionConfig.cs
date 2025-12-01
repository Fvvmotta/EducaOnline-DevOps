using EducaOnline.Conteudo.API.Data.Repository;
using EducaOnline.Conteudo.API.Models;
using EducaOnline.Conteudo.API.Services;
using EducaOnline.Core.Communication;

namespace EducaOnline.Conteudo.API.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static void AddDependencyConfig(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Conteudo
            services.AddScoped<IConteudoService, ConteudoService>();
            services.AddScoped<IConteudoRepository, ConteudoRepository>();
        }
    }
}
