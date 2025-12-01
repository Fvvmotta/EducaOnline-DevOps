using EducaOnline.Core.Messages;

namespace EducaOnline.Aluno.API.Application.Events
{
    public class CertificadoEmitidoEvent : Event
    {
        public Guid AlunoId { get; }
        public Guid CursoId { get; }
        public Guid CertificadoId { get; }

        public CertificadoEmitidoEvent(Guid alunoId, Guid cursoId, Guid certificadoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            CertificadoId = certificadoId;
        }
    }

}
