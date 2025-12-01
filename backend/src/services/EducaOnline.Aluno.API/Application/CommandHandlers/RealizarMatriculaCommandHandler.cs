using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class RealizarMatriculaCommandHandler : CommandHandler,
        IRequestHandler<RealizarMatriculaCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public RealizarMatriculaCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(RealizarMatriculaCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido()) return command.ValidationResult!;

            var aluno = await _alunoRepository.BuscarAlunoPorId(command.AlunoId, cancellationToken);
            if (aluno is null)
            {
                AdicionarErro("Aluno não encontrado.");
                return ValidationResult;
            }

            if (aluno.JaEstaMatriculadoNoCurso(command.CursoId))
            {
                AdicionarErro("Aluno já possui matrícula ativa neste curso.");
                return ValidationResult;
            }

            if (string.IsNullOrWhiteSpace(command.CursoNome))
            {
                AdicionarErro("Nome do curso inválido.");
                return ValidationResult;
            }

            var matricula = new Matricula(
                alunoId: command.AlunoId,
                cursoId: command.CursoId,
                cursoNome: command.CursoNome,
                totalAulas: command.TotalAulas,
                cargaHorariaTotal: command.CargaHorariaTotal
            );

            aluno.RealizarMatricula(matricula);
            await _alunoRepository.AdicionarMatricula(matricula, cancellationToken);

            return await PersistirDados(_alunoRepository.UnitOfWork);
        }
    }
}
