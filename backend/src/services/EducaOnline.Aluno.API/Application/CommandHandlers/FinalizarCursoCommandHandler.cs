using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Aluno.API.Models.Enum;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class FinalizarCursoCommandHandler : CommandHandler,
        IRequestHandler<FinalizarCursoCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public FinalizarCursoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(FinalizarCursoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido()) return command.ValidationResult!;

            var aluno = await _alunoRepository.BuscarAlunoPorId(command.AlunoId, cancellationToken);
            if (aluno is null)
            {
                AdicionarErro("Aluno não encontrado.");
                return ValidationResult;
            }

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.CursoId == command.CursoId);
            if (matricula is null)
            {
                AdicionarErro("Matrícula do curso não encontrada para este aluno.");
                return ValidationResult;
            }

            if (matricula.AulasConcluidas < matricula.TotalAulas)
            {
                AdicionarErro("O aluno ainda não concluiu todas as aulas do curso.");
                return ValidationResult;
            }

            try
            {
                matricula.AtualizarStatus(StatusMatriculaEnum.CURSO_CONCLUIDO);

                var certificado = new Certificado(matricula.CursoNome);
                aluno.EmitirCertificado(matricula.CursoId, certificado);

                aluno.AtualizarHistoricoAprendizado(matricula.AulasConcluidas, matricula.TotalAulas);

                _alunoRepository.AtualizarAluno(aluno);
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
