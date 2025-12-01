using Dapper;
using Educaonline.Pedidos.API.Application.DTO;
using EducaOnline.Pedidos.API.Domain;
using SQLitePCL;

namespace EducaOnLine.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO?> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId);
        Task<PedidoDTO?> ObterPedidosAutorizados();
    }

    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDTO?> ObterUltimoPedido(Guid clienteId)
        {
            const string sql = @"SELECT
                                P.ID AS 'ProdutoId', P.VALORTOTAL,P.PEDIDOSTATUS,                                
                                PIT.ID AS 'ProdutoItemId', PIT.VALORUNITARIO 
                                FROM PEDIDOS P 
                                INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID 
                                WHERE P.CLIENTEID = @clienteId 
                                AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.PEDIDOSTATUS = 1 
                                ORDER BY P.DATACADASTRO DESC";

            var pedido = await _pedidoRepository.ObterConexao()
                .QueryAsync<dynamic>(sql, new { clienteId });

            return MapearPedido(pedido);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            return pedidos.Select(PedidoDTO.ParaPedidoDTO);
        }

        public async Task<PedidoDTO?> ObterPedidosAutorizados()
        {
            // Correção para pegar todos os itens do pedido e ordernar pelo pedido mais antigo
            const string sql = @"SELECT
                                P.ID as PedidoId, P.CLIENTEID, 
                                PI.ID as PedidoItemId, PI.PRODUTOID 
                                FROM PEDIDOS P 
                                INNER JOIN PEDIDOITEMS PI ON P.ID = PI.PEDIDOID 
                                WHERE P.PEDIDOSTATUS = 1                                
                                ORDER BY P.DATACADASTRO";

            // Utilizacao do lookup para manter o estado a cada ciclo de registro retornado
            var lookup = new Dictionary<Guid, PedidoDTO>();

            await _pedidoRepository.ObterConexao().QueryAsync<PedidoRaw, PedidoItemRaw, PedidoDTO>(sql,
                (pRaw, piRaw) =>
                {
                    var pId = Guid.Parse(pRaw.PedidoId);
                    var p = lookup.GetValueOrDefault(pId) ?? new PedidoDTO
                    {
                        Id = pId,
                        ClienteId = Guid.Parse(pRaw.ClienteId),
                        PedidoItems = new List<PedidoItemDTO>()
                    };

                    var pi = new PedidoItemDTO
                    {
                        PedidoId = pId,
                        ProdutoId = Guid.Parse(piRaw.ProdutoId)
                    };

                    p.PedidoItems.Add(pi);
                    lookup[pId] = p;

                    return p;


                }, splitOn: "PedidoId,PedidoItemId");

            // Obtendo dados o lookup
            return lookup.Values.OrderBy(p=>p.Data).FirstOrDefault();
        }

        private PedidoDTO MapearPedido(dynamic result)
        {
            var pedido = new PedidoDTO
            {
                Status = result[0].PEDIDOSTATUS,
                ValorTotal = result[0].VALORTOTAL,               
                PedidoItems = new List<PedidoItemDTO>()                
            };

            foreach (var item in result)
            {
                var pedidoItem = new PedidoItemDTO
                {                    
                    Valor = item.VALORUNITARIO,                    
                };

                pedido.PedidoItems.Add(pedidoItem);
            }

            return pedido;
        }
    }

    public class PedidoRaw
    {
        public string PedidoId { get; set; }
        public string ClienteId { get; set; }
    }

    public class PedidoItemRaw
    {
        public string PedidoItemId { get; set; }
        public string ProdutoId { get; set; }
    }


}