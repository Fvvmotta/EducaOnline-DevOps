using EducaOnline.Bff.Extensions;
using EducaOnline.Bff.Models;
using EducaOnline.Core.Communication;
using Microsoft.Extensions.Options;

namespace EducaOnline.Bff.Services
{
    public interface IAlunoService
    {
        Task<MatriculaDto?> ObterMatricula(Guid id);
        Task<ResponseResult?> MatricularAluno(MatriculaDto matricula);
    }

    public class AlunoService : Service, IAlunoService
    {
        private readonly HttpClient _httpClient;

        public AlunoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.AlunoUrl);
        }

        public async Task<MatriculaDto?> ObterMatricula(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/alunos/{id}/matricula");            
            if (!TratarErrosResponse(response)) return default;
            return await DeserializarObjetoResponse<MatriculaDto>(response);
        }

        public async Task<ResponseResult?> MatricularAluno(MatriculaDto matricula)
        {
            var conteudo = ObterConteudo(matricula);

            var response = await _httpClient.PostAsync($"/api/alunos/matricular", conteudo);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }    
}
