using EducaOnline.Core.DomainObjects;
using EducaOnline.Core.Messages.Integration;
using EducaOnline.MessageBus;
using EducaOnline.Pedidos.API.Domain;

namespace Educaonline.Pedidos.API.Services
{
    public class PedidoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PedidoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago",
               async request => await FinalizarPedido(request));
        }

        private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

                var pedido = await pedidoRepository.ObterPorId(message.PedidoId);

                if (pedido is null)
                {
                    throw new DomainException($"Pedido inexistente {message.PedidoId}");
                }

                pedido.FinalizarPedido();

                pedidoRepository.Atualizar(pedido);

                if (!await pedidoRepository.UnitOfWork.Commit())
                {
                    throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");
                }
            }
        }
    }
}
