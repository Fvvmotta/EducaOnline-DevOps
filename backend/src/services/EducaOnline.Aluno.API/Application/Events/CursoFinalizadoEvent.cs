using EducaOnline.Core.Messages;

namespace EducaOnline.Aluno.API.Application.Events
{
    public class CursoFinalizadoEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid MatriculaId { get; }
        public Guid CursoId { get; }

        public CursoFinalizadoEvent(Guid alunoId, Guid matriculaId, Guid cursoId)
        {
            AggregateId = alunoId;
            AlunoId = alunoId;
            MatriculaId = matriculaId;
            CursoId = cursoId;
        }
    }
}
