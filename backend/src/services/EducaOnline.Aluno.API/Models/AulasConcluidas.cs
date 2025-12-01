using EducaOnline.Core.DomainObjects;

namespace EducaOnline.Aluno.API.Models
{
    public class AulaConcluida : Entity
    {
        public AulaConcluida(Guid aulaId)
        {
            AulaId = aulaId;
        }

        public Guid AlunoId { get; private set; }
        public Guid AulaId { get; private set; }
        public Aluno? Aluno { get; private set; }
    }
}
