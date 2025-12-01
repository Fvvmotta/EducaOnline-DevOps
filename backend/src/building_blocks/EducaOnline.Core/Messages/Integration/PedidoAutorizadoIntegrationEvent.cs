namespace EducaOnline.Core.Messages.Integration
{
    public class PedidoAutorizadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public IEnumerable<Guid> Itens { get; private set; }

        public PedidoAutorizadoIntegrationEvent(Guid clienteId, Guid pedidoId, IEnumerable<Guid> itens)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Itens = itens;
        }
    }
}