namespace EducaOnline.Bff.Models
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public decimal Valor { get; set; }
        public int TotalAulas { get; set; }
        public int CargaHorariaTotal { get; set; }
    }
}
