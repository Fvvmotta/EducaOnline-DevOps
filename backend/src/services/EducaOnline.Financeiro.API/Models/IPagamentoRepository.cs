using EducaOnline.Core.Data;

namespace EducaOnline.Financeiro.API.Models
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void AdicionarPagamento(Pagamento pagamento);
        void AdicionarTransacao(Transacao transacao);
        Task<Pagamento?> ObterPagamento(Guid pedidoId);
        Task<IEnumerable<Transacao>> ObterTransacaoes(Guid pedidoId);
    }
}
