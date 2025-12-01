using EducaOnline.Bff.Extensions;
using EducaOnline.Bff.Models;
using Microsoft.Extensions.Options;

namespace EducaOnline.Bff.Services
{
    public interface IConteudoService
    {
        Task<CursoDto?> BuscarCurso(Guid cursoId);
    }

    public class ConteudoService : Service, IConteudoService
    {
        private readonly HttpClient _httpClient;

        public ConteudoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ConteudoUrl);
        }

        public async Task<CursoDto?> BuscarCurso(Guid cursoId)
        {
            var response = await _httpClient.GetAsync($"/api/conteudos/{cursoId}");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<CursoDto>(response);
        }
    }
}
