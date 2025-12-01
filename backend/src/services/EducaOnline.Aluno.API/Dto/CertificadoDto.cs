namespace EducaOnline.Aluno.API.DTO
{
    public class CertificadoDto
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string? Curso { get; set; }
    }
}
