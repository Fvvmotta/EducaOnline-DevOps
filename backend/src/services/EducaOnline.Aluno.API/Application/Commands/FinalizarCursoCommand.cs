using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class FinalizarCursoCommand : Command
    {
        public FinalizarCursoCommand(Guid alunoId, Guid matriculaId, Guid cursoId, string alunoNome, string cursoNome, int cargaHoraria)
        {
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CursoId = cursoId;
            AlunoNome = alunoNome;
            CursoNome = cursoNome;
            CargaHoraria = cargaHoraria;
        }

        public Guid AlunoId { get; private set; }
        public Guid MatriculaId { get; private set; }
        public Guid CursoId { get; private set; }
        public string AlunoNome { get; private set; }
        public string CursoNome { get; private set; }
        public int CargaHoraria { get; private set; }

        public override bool EhValido()
        {
            ValidationResult = new FinalizarCursoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinalizarCursoValidation : AbstractValidator<FinalizarCursoCommand>
    {
        public FinalizarCursoValidation()
        {
            RuleFor(c => c.AlunoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido.");

            RuleFor(c => c.MatriculaId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id da matrícula inválido.");

            RuleFor(c => c.CursoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do curso inválido.");

            RuleFor(c => c.CursoNome)
                .NotEmpty()
                .WithMessage("Nome do curso inválido.");

            RuleFor(c => c.CargaHoraria)
                .GreaterThan(0)
                .WithMessage("Carga horária inválida.");
        }
    }
}
