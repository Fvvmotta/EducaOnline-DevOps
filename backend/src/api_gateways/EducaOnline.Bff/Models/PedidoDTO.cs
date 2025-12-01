using EducaOnline.Core.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducaOnline.Bff.Models
{
    public class PedidoDTO
    {
        #region Pedido        
        // Autorizado = 1,
        // Pago = 2,
        // Recusado = 3,
        // Entregue = 4,
        // Cancelado = 5
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public List<ItemDTO>? PedidoItems { get; set; }

        #endregion

        #region Cartão        
        public string? NumeroCartao { get; set; }        
        public string? NomeCartao { get; set; }        
        public string? ExpiracaoCartao { get; set; }
        public string? CvvCartao { get; set; }
        #endregion
    }
}
