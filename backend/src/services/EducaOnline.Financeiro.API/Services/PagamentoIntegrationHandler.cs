using EducaOnline.Core.DomainObjects;
using EducaOnline.Core.Messages.Integration;
using EducaOnline.Financeiro.API.Models;
using EducaOnline.MessageBus;

namespace EducaOnline.Financeiro.API.Services
{
    public class PagamentoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PagamentoIntegrationHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {   
            _bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request =>
                await AutorizarPagamento(request));
        }

        private void SetSubscribers()
        {            
            _bus.SubscribeAsync<AlunoMatriculaPagaIntegrationEvent>("AlunoMatriculaPaga", async request =>
            await CapturarPagamento(request));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
        {
            
                using var scope = _serviceProvider.CreateScope();
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();
                var pagamento = new Pagamento
                {
                    PedidoId = message.PedidoId,
                    TipoPagamento = (TipoPagamento)message.TipoPagamento,
                    Valor = message.Valor,
                    CartaoCredito = new CartaoCredito(
                        message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
                };

                var response = await pagamentoService.AutorizarPagamento(pagamento);

                return response;
            
        }


        private async Task CapturarPagamento(AlunoMatriculaPagaIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

                var response = await pagamentoService.CapturarPagamento(message.PedidoId);

                if (!response.ValidationResult.IsValid)
                    throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

                await _bus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
            }
        }
    }
}
