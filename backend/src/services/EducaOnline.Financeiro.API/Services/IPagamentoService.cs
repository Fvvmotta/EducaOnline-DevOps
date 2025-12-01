using EducaOnline.Core.Messages.Integration;
using EducaOnline.Financeiro.API.Models;

namespace EducaOnline.Financeiro.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);        
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
    }
}
