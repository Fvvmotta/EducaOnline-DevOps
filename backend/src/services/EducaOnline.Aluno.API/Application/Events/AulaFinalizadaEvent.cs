using EducaOnline.Core.Messages;

namespace EducaOnline.Aluno.API.Application.Events
{
    public class AulaFinalizadaEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid MatriculaId { get; }
        public Guid CursoId { get; }
        public Guid AulaId { get; }

        public AulaFinalizadaEvent(Guid alunoId, Guid matriculaId, Guid cursoId, Guid aulaId)
        {
            AggregateId = alunoId;
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CursoId = cursoId;
            AulaId = aulaId;
        }
    }
}
