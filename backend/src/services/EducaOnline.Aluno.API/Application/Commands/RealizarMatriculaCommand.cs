using EducaOnline.Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class RealizarMatriculaCommand : Command
    {
        public RealizarMatriculaCommand(
            Guid alunoId,
            Guid cursoId,
            string? cursoNome,
            int totalAulas,
            int cargaHorariaTotal 
        )
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            CursoNome = cursoNome;
            TotalAulas = totalAulas;
            CargaHorariaTotal = cargaHorariaTotal;
        }

        public Guid AlunoId { get; }
        public Guid CursoId { get; }
        public string? CursoNome { get; }
        public int TotalAulas { get; }
        public int CargaHorariaTotal { get; }

        public override bool EhValido()
        {
            ValidationResult = new RealizarMatriculaValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RealizarMatriculaValidation : AbstractValidator<RealizarMatriculaCommand>
    {
        public RealizarMatriculaValidation()
        {
            RuleFor(c => c.AlunoId).NotEqual(Guid.Empty).WithMessage("Id do aluno inválido");
            RuleFor(c => c.CursoId).NotEqual(Guid.Empty).WithMessage("Id do curso inválido");
            RuleFor(c => c.CursoNome).NotEmpty().WithMessage("Nome do curso obrigatório");
            RuleFor(c => c.TotalAulas).GreaterThanOrEqualTo(0).WithMessage("Total de aulas inválido");
            RuleFor(c => c.CargaHorariaTotal).GreaterThanOrEqualTo(0).WithMessage("Carga horária total inválida");
        }
    }
}
