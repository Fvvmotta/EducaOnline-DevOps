using System;
using EducaOnline.Pedidos.API.Domain;

namespace Educaonline.Pedidos.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal Valor { get; set; }
        
        

        public static PedidoItem ParaPedidoItem(PedidoItemDTO pedidoItemDTO)
        {
            return new PedidoItem(pedidoItemDTO.ProdutoId, pedidoItemDTO.Valor);
        }
    }
}