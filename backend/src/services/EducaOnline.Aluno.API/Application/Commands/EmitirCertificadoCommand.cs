using EducaOnline.Core.Messages;
using FluentValidation;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class EmitirCertificadoCommand : Command
    {
        public EmitirCertificadoCommand(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
        }

        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }

        public override bool EhValido()
        {
            ValidationResult = new EmitirCertificadoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class EmitirCertificadoValidation : AbstractValidator<EmitirCertificadoCommand>
    {
        public EmitirCertificadoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido.");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do curso inválido.");
        }
    }
}
