using EducaOnline.Core.Communication;
using EducaOnline.Pedidos.API.Domain;
using EducaOnline.WebAPI.Core.Usuario;
using EducaOnLine.Pedidos.API.Application.Commands;
using EducaOnLine.Pedidos.API.Application.Queries;
using EducaOnLine.Pedidos.API.Data;
using EducaOnLine.Pedidos.API.Data.Repository;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Pedidos.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependenceInjectionConfiguration(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Commands
            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();            
            services.AddScoped<IPedidoQueries, PedidoQueries>();

            // Data
            services.AddScoped<IPedidoRepository, PedidoRepository>();            
            services.AddScoped<PedidosContext>();

            return services;
        }
    }
}
