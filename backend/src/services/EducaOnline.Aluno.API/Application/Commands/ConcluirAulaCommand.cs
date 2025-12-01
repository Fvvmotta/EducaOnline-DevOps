using EducaOnline.Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class ConcluirAulaCommand : Command
    {
        public ConcluirAulaCommand(Guid alunoId, Guid cursoId, Guid aulaId)
        {
            AlunoId = alunoId;
            AulaId = aulaId;
            CursoId = cursoId;
        }

        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public Guid AulaId { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new ConcluirAulaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ConcluirAulaValidation : AbstractValidator<ConcluirAulaCommand>
    {
        public ConcluirAulaValidation()
        {
            RuleFor(c => c.AulaId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id da aula inválido");

            RuleFor(c => c.AlunoId)
                   .NotEqual(Guid.Empty)
                   .WithMessage("Id do aluno inválido");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do curso inválido");
        }
    }
}
