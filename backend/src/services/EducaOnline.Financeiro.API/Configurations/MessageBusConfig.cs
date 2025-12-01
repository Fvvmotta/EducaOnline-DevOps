using EducaOnline.MessageBus;
using EducaOnline.Core.Utils;
using EducaOnline.Financeiro.API.Services;

namespace EducaOnline.Financeiro.API.Configurations
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}
