using EducaOnline.Bff.Models;
using EducaOnline.Bff.Services;
using EducaOnline.WebAPI.Core.Controllers;
using EducaOnline.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducaOnline.Bff.Controllers
{
    [Authorize]
    [Route("api/pedido-bff")]
    public class PedidosController : MainController
    {
        private readonly IPedidoService _pedidoService;
        private readonly IAlunoService _alunoService;
        private readonly IConteudoService _conteudoService;
        private readonly IAspNetUser _user;

        public PedidosController(IPedidoService pedidoService, IAlunoService alunoService, IConteudoService conteudoService, IAspNetUser user)
        {
            _pedidoService = pedidoService;
            _alunoService = alunoService;
            _conteudoService = conteudoService;
            _user = user;
        }


        [HttpPost]
        [Route("compras/pedido")]
        public async Task<IActionResult> AdicionarPedido(PagamentoCartaoViewModel viewModel)
        {   
            var matricula = await _alunoService.ObterMatricula(_user.ObterUserId());
            if (matricula is null)
            {
                AdicionarErro("Aluno não possui matrícula ativa");
                return CustomResponse();
            }

            if(matricula.Status != 1) // 1 = AguardandoPagamento
            {
                AdicionarErro("Matrícula não está com o status aguardando pagamento");
                return CustomResponse();
            }

            var curso = await _conteudoService.BuscarCurso(matricula.CursoId);

            if (curso is null)
            {
                AdicionarErro($"Curso inexistente  com id {matricula.CursoId}");
                return CustomResponse();
            }


            
            var pedido = PopularDadosPedido(matricula, curso , viewModel);            

            var result = await _pedidoService.IniciarPedido(pedido);

            return CustomResponse(result);
        }

        private PedidoDTO PopularDadosPedido(MatriculaDto matricula, CursoDto curso, PagamentoCartaoViewModel dadosCartao)
        {
            var pedido = new PedidoDTO();
            
            pedido.PedidoItems = new List<ItemDTO>()
            {
                new ItemDTO()
                {
                    ProdutoId = matricula.CursoId,
                    Nome = curso.Nome,
                    Valor = curso.Valor                    
                }
            };
            pedido.ValorTotal = curso.Valor;
            
            pedido.NumeroCartao = dadosCartao.NumeroCartao;
            pedido.NomeCartao = dadosCartao.NomeCartao;
            pedido.ExpiracaoCartao = dadosCartao.ExpiracaoCartao;
            pedido.CvvCartao = dadosCartao.CvvCartao;

            return pedido;
        }
    }
}
