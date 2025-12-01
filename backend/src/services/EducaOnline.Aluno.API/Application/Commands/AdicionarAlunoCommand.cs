using EducaOnline.Core.Messages;
using FluentValidation;

namespace EducaOnline.Aluno.API.Application.Commands
{
    public class AdicionarAlunoCommand : Command
    {
        public AdicionarAlunoCommand(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }


        public override bool EhValido()
        {
            ValidationResult = new AdicionarAlunoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AdicionarAlunoValidation : AbstractValidator<AdicionarAlunoCommand>
    {
        public AdicionarAlunoValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do aluno inválido");

            RuleFor(c => c.Nome)
                .NotEqual(string.Empty)
                .WithMessage("Nome não informado.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("E-mail inválido.");
        }
    }
}
