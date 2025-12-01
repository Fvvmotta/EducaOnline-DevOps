using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class FinalizarAulaCommandHandler : CommandHandler,
        IRequestHandler<FinalizarAulaCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public FinalizarAulaCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(FinalizarAulaCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido()) return command.ValidationResult;

            var aluno = await _alunoRepository.BuscarAlunoPorId(command.AlunoId, cancellationToken);
            if (aluno == null)
            {
                AdicionarErro("Aluno não encontrado.");
                return ValidationResult;
            }

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == command.MatriculaId);
            if (matricula == null)
            {
                AdicionarErro("Matrícula não encontrada para o aluno informado.");
                return ValidationResult;
            }

            try
            {
                var novaAula = aluno.ConcluirAula(matricula.CursoId, command.AulaId);
                _alunoRepository.AdicionarAulaConcluida(novaAula);
            }
            catch (Exception ex)
            {
                AdicionarErro(ex.Message);
                return ValidationResult;
            }

            return await PersistirDados(_alunoRepository.UnitOfWork);
        }
    }
}
