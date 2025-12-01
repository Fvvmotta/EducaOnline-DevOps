using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class ConcluirAulaCommandHandler : CommandHandler,
        IRequestHandler<ConcluirAulaCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public ConcluirAulaCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(ConcluirAulaCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido()) return command.ValidationResult!;

            var aluno = await _alunoRepository.BuscarAlunoPorId(command.AlunoId, cancellationToken);
            if (aluno is null)
            {
                AdicionarErro("Aluno não encontrado.");
                return ValidationResult;
            }

            aluno.ConcluirAula(command.CursoId, command.AulaId);

            _alunoRepository.AtualizarAluno(aluno);
            return await PersistirDados(_alunoRepository.UnitOfWork);
        }
    }
}
