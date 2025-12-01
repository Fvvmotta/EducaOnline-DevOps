using EducaOnline.Aluno.API.Models.Enum;
using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Aluno.API.Models
{
    public class Matricula : Entity
    {
        protected Matricula()
        {
            CursoNome = string.Empty;
            DataMatricula = DateTime.UtcNow;
        }

        public Matricula(Guid alunoId, Guid cursoId, string cursoNome, int totalAulas, int cargaHorariaTotal)
        {
            if (alunoId == Guid.Empty) throw new DomainException("AlunoId inválido.");
            if (cursoId == Guid.Empty) throw new DomainException("CursoId inválido.");
            if (string.IsNullOrWhiteSpace(cursoNome)) throw new DomainException("Nome do curso inválido.");
            if (totalAulas < 0) throw new DomainException("Total de aulas inválido.");
            if (cargaHorariaTotal < 0) throw new DomainException("Carga horária inválida.");

            Id = Guid.NewGuid();
            AlunoId = alunoId;
            CursoId = cursoId;
            CursoNome = cursoNome;
            TotalAulas = totalAulas;
            CargaHorariaTotal = cargaHorariaTotal;

            DataMatricula = DateTime.UtcNow;
            Status = StatusMatriculaEnum.PENDENTE_PAGAMENTO;
            AulasConcluidas = 0;
        }
        
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public string CursoNome { get; private set; }
        public int TotalAulas { get; private set; }
        public int CargaHorariaTotal { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public StatusMatriculaEnum Status { get; private set; }
        public int AulasConcluidas { get; private set; }

        public Aluno? Aluno { get; private set; }

        public void AtualizarStatus(StatusMatriculaEnum status) => Status = status;

        public void AtualizarTotalAulas(int totalAulas)
        {
            if (totalAulas <= 0)
                throw new DomainException("Total de aulas inválido.");

            TotalAulas = totalAulas;
        }

        public void RegistrarConclusaoAula(int horasDaAula = 0)
        {
            if (Status == StatusMatriculaEnum.CURSO_CONCLUIDO)
                throw new DomainException("Curso já concluído.");

            if (TotalAulas == 0)
                throw new DomainException("Total de aulas do curso não informado.");

            AulasConcluidas = Math.Min(AulasConcluidas + 1, TotalAulas);
        }

        public int ObterProgressoPercentual()
        {
            if (TotalAulas <= 0) return 0;
            return (int)Math.Round((decimal)AulasConcluidas / TotalAulas * 100, 0);
        }

        public bool PodeEmitirCertificado() => ObterProgressoPercentual() >= 100;

        public void Pagar()
        {
            if (!PodeSerPaga()) throw new DomainException("Matricula não está pendente de pagamento");
            Status = StatusMatriculaEnum.EM_ANDAMENTO;
        }

        public bool PodeSerPaga()
        {
            return Status == StatusMatriculaEnum.PENDENTE_PAGAMENTO;
        }
    }

}
