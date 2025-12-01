using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Pedidos.API.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public Pedido(Guid clienteId, decimal valorTotal, List<PedidoItem> pedidoItems)
        {
            ClienteId = clienteId;
            ValorTotal = valorTotal;
            _pedidoItems = pedidoItems;
        }

        // EF ctor
        protected Pedido() { }

        
        public Guid ClienteId { get; private set; }                
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItems = [];
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public void AutorizarPedido()
        {
            PedidoStatus = PedidoStatus.Autorizado;
        }
        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());            
        }
    }
}
