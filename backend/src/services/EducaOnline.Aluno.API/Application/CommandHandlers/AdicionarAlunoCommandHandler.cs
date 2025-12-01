using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class AdicionarAlunoCommandHandler : CommandHandler,
     IRequestHandler<AdicionarAlunoCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public AdicionarAlunoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(AdicionarAlunoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido()) return command.ValidationResult!;

            var alunoExistente = await _alunoRepository.BuscarPorEmail(command.Email, cancellationToken);
            if (alunoExistente is not null)
            {
                AdicionarErro($"Já existe um aluno cadastrado com o e-mail '{command.Email}'.");
                return ValidationResult;
            }
            var aluno = new Models.Aluno(command.Id, command.Nome, command.Email);

            var ra = await _alunoRepository.BuscarProximoRa(cancellationToken);
            aluno.VincularRa(ra);

            await _alunoRepository.AdicionarAluno(aluno, cancellationToken);
            return await PersistirDados(_alunoRepository.UnitOfWork);
        }
    }

}
