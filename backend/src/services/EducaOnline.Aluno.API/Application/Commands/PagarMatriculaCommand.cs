using EducaOnline.Core.Messages;
using FluentValidation;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class PagarMatriculaCommand : Command
    {
        
        public PagarMatriculaCommand(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
        }

        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }        

        public override bool EhValido()
        {
            ValidationResult = new PagarMatriculaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class PagarMatriculaValidation : AbstractValidator<PagarMatriculaCommand>
    {
        public PagarMatriculaValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno não informado");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Curso não informado.");
        }
    }
}
