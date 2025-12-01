using EducaOnline.Bff.Models;
using EducaOnline.Bff.Services;
using EducaOnline.Core.Communication;
using EducaOnline.WebAPI.Core.Controllers;
using EducaOnline.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducaOnline.Bff.Controllers
{
    [Authorize]
    [Route("api/aluno-bff")]
    public class AlunosController : MainController
    {

        private readonly IConteudoService _conteudoService;
        private readonly IAspNetUser _user;
        private readonly IAlunoService _alunoService;

        public AlunosController(IConteudoService conteudoService, IAspNetUser user, IAlunoService alunoService)
        {
            _conteudoService = conteudoService;
            _user = user;
            _alunoService = alunoService;
        }

        [HttpPost("matricular")]
        public async Task<IActionResult> RealizarMatricula([FromBody] MatricularAlunoViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var curso = await _conteudoService.BuscarCurso(viewModel.CursoId!.Value);

            if (curso is null)
            {
                AdicionarErro($"Curso não encontrado com id {viewModel.CursoId}");
                return CustomResponse();
            }

            var result = await _alunoService.MatricularAluno(new MatriculaDto
            {
                AlunoId = _user.ObterUserId(),
                CursoId = curso.Id,
                CursoNome = curso.Nome,
                TotalAulas = curso.TotalAulas,
                CargaHorariaTotal = curso.CargaHorariaTotal
            });

            
            return CustomResponse(result);
        }
    }
}
