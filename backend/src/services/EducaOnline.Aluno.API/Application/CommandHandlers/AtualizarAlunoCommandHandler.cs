using EducaOnline.Aluno.API.Application.Commands;
using EducaOnline.Aluno.API.Models;
using EducaOnline.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace EducaOnline.Aluno.API.Application.CommandHandlers
{
    public class AtualizarAlunoCommandHandler : CommandHandler,
    IRequestHandler<AtualizarAlunoCommand, ValidationResult>
    {
        private readonly IAlunoRepository _alunoRepository;

        public AtualizarAlunoCommandHandler(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<ValidationResult> Handle(AtualizarAlunoCommand command, CancellationToken cancellationToken)
        {
            if (!command.EhValido())
                return command.ValidationResult!;

            var aluno = await _alunoRepository.BuscarAlunoPorId(command.Id, cancellationToken);
            if (aluno is null)
            {
                AdicionarErro("Aluno não encontrado.");
                return ValidationResult;
            }

            if (!string.IsNullOrWhiteSpace(command.Email))
            {
                var emailExistente = await _alunoRepository.BuscarPorEmail(command.Email, cancellationToken);
                if (emailExistente is not null && emailExistente.Id != aluno.Id)
                {
                    AdicionarErro($"Já existe um aluno cadastrado com o e-mail '{command.Email}'.");
                    return ValidationResult;
                }
            }

            aluno.AtualizarDados(command.Nome, command.Email);

            _alunoRepository.AtualizarAluno(aluno);
            return await PersistirDados(_alunoRepository.UnitOfWork);
        }

    }
}