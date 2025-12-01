using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Pedidos.API.Domain
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }                
        public decimal ValorUnitario { get; private set; }        

        // EF Rel.
        public Pedido? Pedido { get; set; }

        public PedidoItem(Guid produtoId, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            ValorUnitario = valorUnitario;            
        }

        // EF ctor
        protected PedidoItem() { }

        internal decimal CalcularValor()
        {
            return ValorUnitario;
        }
    }


}
