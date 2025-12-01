using EducaOnline.Pedidos.API.Domain;
using System;
using System.Collections.Generic;


namespace Educaonline.Pedidos.API.Application.DTO
{
    public class PedidoDTO
    {
        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public List<PedidoItemDTO>? PedidoItems { get; set; }


        public static PedidoDTO ParaPedidoDTO(Pedido pedido)
        {
            var pedidoDTO = new PedidoDTO
            {
                Id = pedido.Id,                
                Status = (int)pedido.PedidoStatus,
                Data = pedido.DataCadastro,
                ValorTotal = pedido.ValorTotal,                
                PedidoItems = new List<PedidoItemDTO>()
                
            };

            foreach (var item in pedido.PedidoItems)
            {
                pedidoDTO.PedidoItems.Add(new PedidoItemDTO
                {                    
                    ProdutoId = item.ProdutoId,
                    Valor = item.ValorUnitario,
                    PedidoId = item.PedidoId
                });
            }
            return pedidoDTO;
        }
    }
}