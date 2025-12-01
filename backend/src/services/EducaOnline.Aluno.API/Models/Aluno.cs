using EducaOnline.Aluno.API.Application.Events;
using EducaOnline.Aluno.API.Models.Enum;
using EducaOnline.Aluno.API.Models.ValueObjects;
using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Aluno.API.Models
{
    public class Aluno : Entity, IAggregateRoot
    {
        protected Aluno()
        {
            Nome = string.Empty;
            Email = string.Empty;
            DataCadastro = DateTime.UtcNow;
            AulasConcluidas = new HashSet<AulaConcluida>();
            Matriculas = new HashSet<Matricula>();
            HistoricoAprendizado = new HistoricoAprendizado(0, 0);
        }

        public Aluno(Guid id, string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome inválido.");
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("E-mail inválido.");

            Id = id;
            Nome = nome;
            Email = email;
            DataCadastro = DateTime.UtcNow;
            AulasConcluidas = new HashSet<AulaConcluida>();
            Matriculas = new HashSet<Matricula>();
            HistoricoAprendizado = new HistoricoAprendizado(0, 0);
        }

        public string Nome { get; private set; }
        public int Ra { get; private set; }
        public string Email { get; private set; }
        public DateTime? DataCadastro { get; private set; }
        public HistoricoAprendizado HistoricoAprendizado { get; private set; }

        public ICollection<AulaConcluida> AulasConcluidas { get; private set; }
        public ICollection<Matricula> Matriculas { get; private set; }
        public ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();

        public void AtualizarDados(string? nome, string? email)
        {
            var nomeInformado = !string.IsNullOrWhiteSpace(nome);
            var emailInformado = !string.IsNullOrWhiteSpace(email);

            if (!nomeInformado && !emailInformado)
                throw new DomainException("Nenhum dado informado para atualização.");

            if (nomeInformado)
                Nome = nome!;

            if (emailInformado && !Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                if (!email!.Contains("@") || !email.Contains("."))
                    throw new DomainException("E-mail inválido.");
                Email = email;
            }
        }

        public void VincularRa(int ra) => Ra = ra;

        public void RealizarMatricula(Matricula nova)
        {
            if (nova is null)
                throw new DomainException("Matrícula inválida.");
            if (Matriculas.Any(m => m.CursoId == nova.CursoId && m.Status != StatusMatriculaEnum.CANCELADO))
                throw new DomainException("Aluno já possui matrícula ativa nesse curso.");
            if (nova.AlunoId != Id)
                throw new DomainException("Matrícula não pertence a este aluno.");

            Matriculas.Add(nova);
        }

        public Matricula ObterMatricula(Guid cursoId)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.CursoId == cursoId);
            if (matricula is null)
                throw new DomainException($"Aluno não está matriculado no curso {cursoId}");
            return matricula;
        }

        public AulaConcluida ConcluirAula(Guid cursoId, Guid aulaId)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.CursoId == cursoId);
            if (matricula is null)
                throw new DomainException("Matrícula não encontrada para o curso informado.");

            //if (matricula.Status == StatusMatriculaEnum.PENDENTE_PAGAMENTO)
            //    throw new DomainException("Não é possível concluir aula sem pagamento.");

            if (matricula.Status == StatusMatriculaEnum.CURSO_CONCLUIDO)
                throw new DomainException("Curso já concluído.");

            if (AulasConcluidas.Any(a => a.AulaId == aulaId))
                throw new DomainException("Aula já concluída.");

            var aulaConcluida = new AulaConcluida(aulaId);
            AulasConcluidas.Add(aulaConcluida);

            matricula.RegistrarConclusaoAula();

            HistoricoAprendizado = HistoricoAprendizado.Atualizar(
                matricula.AulasConcluidas,
                matricula.TotalAulas
            );

            AdicionarEvento(new AulaFinalizadaEvent(Id, matricula.Id, matricula.CursoId, aulaId));

            return aulaConcluida;
        }



        public void EmitirCertificado(Guid cursoId, Certificado certificado)
        {
            var matricula = ObterMatricula(cursoId);

            var progresso = matricula.ObterProgressoPercentual();
            if (progresso >= 100 && matricula.AulasConcluidas >= matricula.TotalAulas)
            {
                matricula.AtualizarStatus(StatusMatriculaEnum.CURSO_CONCLUIDO);
                Certificados.Add(certificado);
            }
            else
            {
                throw new DomainException($"Curso não concluído. Progresso atual {progresso}%.");
            }
        }

        public void PagarMatricula(Guid cursoId)
        {
            var matricula = ObterMatricula(cursoId);
            matricula.Pagar();
        }
        public void AtualizarHistoricoAprendizado(int aulasConcluidas, int totalAulas)
        {
            HistoricoAprendizado = HistoricoAprendizado.Atualizar(aulasConcluidas, totalAulas);
        }
        public bool JaEstaMatriculadoNoCurso(Guid cursoId)
        {
            return Matriculas.Any(m => m.CursoId == cursoId && m.Status != StatusMatriculaEnum.CANCELADO);
        }
        public void AtualizarStatusMatricula(Guid matriculaId, StatusMatriculaEnum status)
        {
            var matricula = Matriculas.FirstOrDefault(m => m.Id == matriculaId);
            if (matricula == null) throw new DomainException("Matrícula não encontrada.");
            matricula.AtualizarStatus(status);
        }
    }
}
