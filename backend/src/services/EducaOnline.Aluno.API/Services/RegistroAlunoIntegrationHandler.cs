using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Core.Communication;
using EducaOnline.Core.DomainObjects;
using EducaOnline.Core.Messages.Integration;
using EducaOnline.MessageBus;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Services
{
    public class RegistroAlunoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegistroAlunoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarAluno(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private void SetSubscribers()
        {
            //_bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado",
            //    async request => await CancelarPedido(request));
            _bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado",
               async request => await PagarMatriculaAluno(request));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void OnConnect(object? s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarAluno(UsuarioRegistradoIntegrationEvent message)
        {
            var command = new AdicionarAlunoCommand(message.Id, message.Nome, message.Email);
            ValidationResult resultado;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                resultado = await mediator.EnviarComando(command);
            }

            return new ResponseMessage(resultado);
        }

        private async Task PagarMatriculaAluno(PedidoAutorizadoIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var command = new PagarMatriculaCommand(message.ClienteId, message.Itens.First());
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                var response = await mediator.EnviarComando(command);

                if (!response.IsValid)
                    throw new DomainException($"Falha ao pagar setar matricula como paga {message.PedidoId}");

                await _bus.PublishAsync(new AlunoMatriculaPagaIntegrationEvent(message.ClienteId, message.PedidoId) );
            }
        }
    }
}
