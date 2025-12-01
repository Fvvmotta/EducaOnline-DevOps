using EducaOnline.Aluno.API.Data.Repository;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Aluno.API.Services;
using EducaOnline.Core.Communication;
using EducaOnline.Core.Data;
using EducaOnline.WebAPI.Core.Usuario;

namespace EducaOnline.Aluno.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyConfig(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();


            // Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Conteudo
            services.AddScoped<IAlunoRepository, AlunoRepository>();

        }
    }
}
