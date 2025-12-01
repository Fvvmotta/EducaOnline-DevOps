using EducaOnline.Core.Data;
using EducaOnline.Financeiro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EducaOnline.Financeiro.API.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly FinanceiroContext _context;

        public PagamentoRepository(FinanceiroContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarPagamento(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Pagamento?> ObterPagamento(Guid pedidoId)
        {
            return await _context.Pagamentos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }

        public async Task<IEnumerable<Transacao>> ObterTransacaoes(Guid pedidoId)
        {
            return await _context.Transacoes.AsNoTracking()
                .Where(t => t.Pagamento!.PedidoId == pedidoId).ToListAsync();
        }
    }
}
