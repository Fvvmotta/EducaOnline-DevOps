using EducaOnline.Aluno.API.Application.Commands; 
using EducaOnline.Aluno.API.Dto;
using EducaOnline.Aluno.API.DTO;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Communication;
using EducaOnline.Core.Enums;
using EducaOnline.MessageBus;
using EducaOnline.WebAPI.Core.Controllers;
using EducaOnline.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EducaOnline.Aluno.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : MainController
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediatorHandler _mediator;
        private readonly IMessageBus _messageBus;
        private readonly IAspNetUser _user;

        public AlunosController(
            IAlunoRepository alunoRepository,
            IMediatorHandler mediator,
            IMessageBus messageBus,
            IAspNetUser user)
        {
            _alunoRepository = alunoRepository;
            _mediator = mediator;
            _messageBus = messageBus;
            _user = user;
        }

        [HttpPost]
        public async Task<IActionResult> CriarAluno([FromBody] AlunoDto alunoDto)
        {
            if (!ModelState.IsValid)
            {
                AdicionarErro("Dados do aluno inválidos.");
                return CustomResponse();
            }

            var novoId = Guid.NewGuid();
            var cmd = new AdicionarAlunoCommand(novoId, alunoDto.Nome, alunoDto.Email);

            var result = await _mediator.EnviarComando(cmd);
            if (!result.IsValid)
                return CustomResponse(result);

            // Publicar evento caso necessário
            return Ok("Aluno criado com sucesso!");
        }

        [Authorize(Roles = nameof(PerfilUsuarioEnum.ADM))]
        [HttpGet]
        public async Task<IActionResult> ObterTodos(CancellationToken cancellationToken)
        {
            var alunos = await _alunoRepository.BuscarAlunos(cancellationToken);

            if (alunos == null || !alunos.Any())
            {
                AdicionarErro("Nenhum aluno encontrado.");
                return CustomResponse();
            }

            return CustomResponse(alunos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterAlunoPorId(Guid id, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.BuscarAlunoPorId(id, cancellationToken);
            if (aluno is null)
            {
                AdicionarErro("Aluno não encontrado");
                return CustomResponse();
            }
            return Ok(aluno);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarAluno(Guid id, [FromBody] AlunoDto alunoDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                AdicionarErro("Dados do aluno inválidos.");
                return CustomResponse();
            }
            var cmd = new AtualizarAlunoCommand(id, alunoDto.Nome, alunoDto.Email);

            var result = await _mediator.EnviarComando(cmd);
            if (!result.IsValid)
                return CustomResponse(result);

            var aluno = await _alunoRepository.BuscarAlunoPorId(id, cancellationToken);
            return CustomResponse(aluno);
        }

        [HttpPost("matricular")]
        public async Task<IActionResult> RealizarMatricula([FromBody] MatriculaDto dto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var cmd = new RealizarMatriculaCommand(
                alunoId: dto.AlunoId,
                cursoId: dto.CursoId,
                cursoNome: dto.CursoNome,
                totalAulas: dto.TotalAulas,
                cargaHorariaTotal: dto.CargaHorariaTotal
            );

            var result = await _mediator.EnviarComando(cmd);
            return CustomResponse(result);
        }

        [HttpPost("aulas/finalizar")]
        public async Task<IActionResult> FinalizarAula([FromBody] FinalizarAulaDto dto, CancellationToken cancellationToken)
        {
            var cmd = new FinalizarAulaCommand(dto.AlunoId, dto.MatriculaId, dto.AulaId);
            var result = await _mediator.EnviarComando(cmd);
            if (!result.IsValid)
                return CustomResponse(result);

            return Ok("Aula finalizada com sucesso!");
        }

        [HttpGet("{alunoId}/matriculas/{matriculaId}/progresso")]
        public async Task<IActionResult> ObterProgresso(Guid alunoId, Guid matriculaId, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.BuscarAlunoPorId(alunoId, cancellationToken);
            if (aluno == null)
            {
                AdicionarErro("Aluno não encontrado.");
                return CustomResponse();
            }

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null)
            {
                AdicionarErro("Matrícula não encontrada para o aluno informado.");
                return CustomResponse();
            }

            var historico = aluno.HistoricoAprendizado;

            var totalAulas = historico?.TotalAulas ?? matricula.TotalAulas;
            var concluidas = historico?.TotalAulasConcluidas ?? matricula.AulasConcluidas;

            var progresso = historico?.Progresso ??
                            (totalAulas == 0 ? 0 : Math.Round((double)concluidas / totalAulas * 100, 2));

            var todasAulasConcluidas = totalAulas > 0 && concluidas >= totalAulas;

            var resultado = new
            {
                AlunoId = aluno.Id,
                MatriculaId = matricula.Id,
                CursoId = matricula.CursoId,
                CursoNome = matricula.CursoNome,
                TotalAulas = totalAulas,
                AulasConcluidas = concluidas,
                PorcentagemConcluida = progresso,
                TodasAulasConcluidas = todasAulasConcluidas
            };

            return Ok(resultado);
        }


        [HttpPost("emitir-certificado")]
        public async Task<IActionResult> EmitirCertificado([FromBody] CertificadoDto certificadoDto)
        {
            var cmd = new EmitirCertificadoCommand(
                cursoId: certificadoDto.CursoId,
                alunoId: certificadoDto.AlunoId
            );

            var result = await _mediator.EnviarComando(cmd);
            if (!result.IsValid)
                return CustomResponse(result);

            return CustomResponse("Certificado emitido com sucesso!");
        }

        [HttpGet("{id:guid}/matricula")]
        public async Task<IActionResult> ObterMatricula(Guid id, CancellationToken cancellationToken)
        {
            var matricula = await _alunoRepository.BuscarMatriculaPorAlunoId(id, cancellationToken);
            if (matricula is null)
            {
                AdicionarErro("Matricula não encontrada");
                return CustomResponse();
            }
            return Ok(matricula);
        }
    }
}
