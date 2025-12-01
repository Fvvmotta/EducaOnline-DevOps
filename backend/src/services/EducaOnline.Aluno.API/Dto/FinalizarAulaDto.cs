namespace EducaOnline.Aluno.API.Dto
{
    public class FinalizarAulaDto
    {
        public Guid AlunoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid AulaId { get; set; }
    }

}
