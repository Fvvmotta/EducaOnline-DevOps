using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class EmitirCertificadoCommandHandler : CommandHandler,
        IRequestHandler<EmitirCertificadoCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public EmitirCertificadoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(EmitirCertificadoCommand command, CancellationToken cancellationToken)
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

            if (!matricula.PodeEmitirCertificado())
            {
                AdicionarErro("Progresso insuficiente para emitir certificado.");
                return ValidationResult;
            }

            var certificado = new Certificado(matricula.CursoNome);
            aluno.EmitirCertificado(matricula.CursoId, certificado);

            _alunoRepository.AtualizarAluno(aluno);
            return await PersistirDados(_alunoRepository.UnitOfWork);
        }
    }
}
