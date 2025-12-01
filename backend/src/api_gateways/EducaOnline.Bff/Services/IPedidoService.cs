using EducaOnline.Bff.Extensions;
using EducaOnline.Bff.Models;
using EducaOnline.Core.Communication;
using Microsoft.Extensions.Options;

namespace EducaOnline.Bff.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult?> IniciarPedido(PedidoDTO pedido);
    }

    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        public async Task<ResponseResult?> IniciarPedido(PedidoDTO pedido)
        {

            var pedidoContent = ObterConteudo(pedido);

            var response = await _httpClient.PostAsync("/pedido/", pedidoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}
