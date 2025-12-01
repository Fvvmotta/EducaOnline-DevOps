using EducaOnline.Core.Messages;
using FluentValidation;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class FinalizarAulaCommand : Command
    {
        public FinalizarAulaCommand(Guid alunoId, Guid matriculaId, Guid aulaId)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            AulaId = aulaId;
        }

        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }
        public Guid AulaId { get; private set; }

        public override bool EhValido()
        {
            ValidationResult = new FinalizarAulaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinalizarAulaValidation : AbstractValidator<FinalizarAulaCommand>
    {
        public FinalizarAulaValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido.");

            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id da matrícula inválido.");

            RuleFor(c => c.AulaId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id da aula inválido.");
        }
    }
}
