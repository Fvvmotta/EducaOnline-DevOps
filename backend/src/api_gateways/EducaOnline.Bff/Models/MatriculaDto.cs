namespace EducaOnline.Bff.Models
{
    public class MatriculaDto
    {
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }        
        public int Status { get; set; }
        public string? CursoNome { get; set; }
        public int TotalAulas { get; set; }
        public int CargaHorariaTotal { get; set; }
    }
}
